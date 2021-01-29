// Coded by Alex Danvy
// http://danvy.tv
// http://twitter.com/danvy
// http://github.com/danvy
// Licence Apache 2.0
// Use at your own risk, have fun

using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace LaserGRBL.WiFiDiscovery
{
	public static class IPAddressHelper
	{
		private static string AVAIL = "Available";

		[DllImport("iphlpapi.dll", ExactSpelling = true)]
		public static extern int SendARP(uint DestIP, uint SrcIP, byte[] pMacAddr, ref int PhyAddrLen);
		public static IPAddress GetBroadcastAddress(this IPAddress ip, IPAddress mask)
		{
			var ipBytes = ip.GetAddressBytes();
			var maskBytes = mask.GetAddressBytes();
			if (ipBytes.Length != maskBytes.Length)
				throw new ArgumentException("Lengths of IP address and subnet mask do not match.");
			var broadcast = new byte[ipBytes.Length];
			for (int i = 0; i < broadcast.Length; i++)
			{
				broadcast[i] = (byte)(ipBytes[i] | (maskBytes[i] ^ 255));
			}
			return new IPAddress(broadcast);
		}
		public static IPAddress GetNetworkAddress(this IPAddress ip, IPAddress mask)
		{
			byte[] ipBytes = ip.GetAddressBytes();
			byte[] maskBytes = mask.GetAddressBytes();
			if (ipBytes.Length != maskBytes.Length)
				throw new ArgumentException("Lengths of IP address and subnet mask do not match.");
			byte[] network = new byte[ipBytes.Length];
			for (int i = 0; i < network.Length; i++)
			{
				network[i] = (byte)(ipBytes[i] & (maskBytes[i]));
			}
			return new IPAddress(network);
		}
		public static IPAddress GetSubnetMask(this IPAddress address)
		{
			foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
			{
				foreach (var unicastIP in adapter.GetIPProperties().UnicastAddresses)
				{
					if (unicastIP.Address.AddressFamily == AddressFamily.InterNetwork)
					{
						if (address.Equals(unicastIP.Address))
						{
							return unicastIP.IPv4Mask;
						}
					}
				}
			}
			return null;
		}
		public static IPAddress GetLocalIP()
		{
			foreach (var nic in NetworkInterface.GetAllNetworkInterfaces().Where(o => o.OperationalStatus == OperationalStatus.Up))
			{
				var prop = nic.GetIPProperties();
				var address = prop.GatewayAddresses.FirstOrDefault();
				if (address != null)
				{
					var unicast = prop.UnicastAddresses.Where((o) => o.Address.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
					if (unicast != null)
						return unicast.Address;
				}
			}
			return null;
		}

		public class ScanResult
		{
			public string Ping;
			public string MAC;
			//public string HostName;
			public string Telnet;

			public IPAddress IP;
			public int Port;

			internal bool HasData()
			{
				return Ping != null || MAC != null /*|| HostName != null*/ || Telnet == AVAIL;
			}
		}

		public static ScanResult ScanIP(this IPAddress ip, int port, CancellationToken token)
		{
			ScanResult rv = new ScanResult();
			List<Task> T = new List<Task>();

			rv.Port = port;
			rv.IP = ip;

			T.Add(Task.Factory.StartNew(() => { rv.Ping = ip.Ping(); }));
			T.Add(Task.Factory.StartNew(() => { rv.MAC = ip.GetMAC(); }));
			T.Add(Task.Factory.StartNew(() => { rv.Telnet = ip.Telnet(port); }));

			Task.WaitAll(T.ToArray(), token);
			return rv;
		}

		private static string GetMAC(this IPAddress ip, int retryCount = 1)
		{
			var destIP = ip.ToUInt32();
			var mac = new byte[6];
			var macLen = mac.Length;
			for (var j = 0; j < retryCount; j++)
			{
				var rc = SendARP(destIP, 0, mac, ref macLen);
				if (rc == 0)
				{
					var str = new string[macLen];
					for (int i = 0; i < macLen; i++)
						str[i] = mac[i].ToString("x2");
					return string.Join(":", str);
				}
			}
			return null;
		}

		private static string Telnet(this IPAddress ip, int port)
		{
			try
			{
				var CLI = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				CLI.ReceiveTimeout = 2000;
				CLI.SendTimeout = 2000;
				// CLI.Connect(EP)

				IAsyncResult result = CLI.BeginConnect(new IPEndPoint(ip, port), null, null);
				bool success = result.AsyncWaitHandle.WaitOne(5000, true); // connect timeout
				if (success)
				{
					CLI.EndConnect(result);
				}
				else
				{
					CLI.Close();
					throw new SocketException(10060);
				} // timeout

				if (CLI.Connected)
				{
					CLI.Close();
					return AVAIL;
					//reply.Connected = true;
					//if (!mute)
					//	SendMessageSleep($"Telnet {EP.Port} success!", 1000);
					//try
					//{
					//	if (!mute)
					//		SendMessageSleep($"Check signature...", 1000);
					//	var Response = new byte[4];
					//	CLI.Send(Encoding.UTF8.GetBytes("PING"));
					//	CLI.Receive(Response);
					//	reply.GoodReply = ValidateToken(Response, Encoding.UTF8.GetBytes("SDBS"));
					//	if (reply.GoodReply)
					//	{
					//		if (!mute)
					//			SendMessageSleep($"Check signature success!", 1000);
					//	}
					//	else if (!mute)
					//		SendMessageSleep($"Check signature failed! (Wrong Response)", 3000);
					//}
					//catch (Exception ex)
					//{
					//	if (!mute)
					//		SendMessageSleep($"Check signature failed! ({ex.Message})", 3000);
					//}

					CLI.Close();
				}
			}
			catch { }

			return "Failed!";
		}

		private static string Ping(this IPAddress ip)
		{
			Ping P = new Ping();
			PingReply reply = P.Send(ip, 2000);

			if (reply.Status == IPStatus.Success)
				return $"{reply.RoundtripTime}ms";
			else
				return null;
		}

		public static bool IsInSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
		{
			IPAddress network1 = address.GetNetworkAddress(subnetMask);
			IPAddress network2 = address2.GetNetworkAddress(subnetMask);
			return network1.Equals(network2);
		}
		public static UInt32 ToUInt32(this IPAddress ip)
		{
			return BitConverter.ToUInt32(ip.GetAddressBytes(), 0);
		}
		public static void ScanIP(Action<IPAddress, ScanResult> callback, Action<int, int> progress, CancellationToken ct, int port, IPAddress paramIP = null)
		{
			int count = 0;

			if (callback == null)
				throw new ArgumentNullException(string.Format("Callback needed"));
			if (progress == null)
				throw new ArgumentNullException(string.Format("Progress needed"));

			var localIP = paramIP ?? IPAddressHelper.GetLocalIP();
			if (localIP == null)
				throw new Exception(string.Format("Can't find network adapter for IP {0}", paramIP));
			var localMask = localIP.GetSubnetMask();
			if (localMask == null)
				throw new Exception(string.Format("Can't find subnet mask for IP {0}", localIP));

			//var localMac = localIP.ScanIP(ct);
			//Debug.WriteLine(string.Format("Local IP={0}, Mask={1}, Mac={2}", localIP.ToString(), localMask.ToString(), localMac));

			var segment = new IPSegment(localIP, localMask);
			Debug.WriteLine(string.Format("Number of IPs={0}, NetworkAddress={1}, Broardcast={2}", segment.NumberOfIPs, segment.NetworkAddress, segment.BroadcastAddress));

			progress.Invoke(0, (int)segment.NumberOfIPs);

			ParallelOptions po = new ParallelOptions();
			po.CancellationToken = ct;
			po.MaxDegreeOfParallelism = 32;//Environment.ProcessorCount;
			
			try
			{
				Parallel.ForEach<IPAddress>(segment.Hosts(), po, (ip) =>
				{
					//Debug.WriteLine("Scanning " + ip.ToString());

					ScanResult rv = ip.ScanIP(port, ct);

					Interlocked.Increment(ref count);
					progress.Invoke(count, (int)segment.NumberOfIPs);

					if (rv.HasData())
						callback.Invoke(ip, rv);

					if (po.CancellationToken != null)
						po.CancellationToken.ThrowIfCancellationRequested();
				});
			}
			catch (OperationCanceledException e)
			{
				Debug.WriteLine(e.Message);
			}
		}
	}
	public static class UInt16Helper
	{
		public static UInt16 ReverseBytes(UInt16 value)
		{
			return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
		}
	}
	public static class UInt32Helper
	{
		public static IPAddress ToIPAddress(this UInt32 ip)
		{
			return new IPAddress(BitConverter.GetBytes(ip));
		}
		public static UInt32 ReverseBytes(this UInt32 value)
		{
			return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
				   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
		}
	}
	public static class UInt64Helper
	{
		public static UInt64 ReverseBytes(this UInt64 value)
		{
			return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
				   (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
				   (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
				   (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
		}
	}

	class IPSegment
	{
		private UInt32 ip;
		private UInt32 mask;
		private UInt32 networkAddress;
		private UInt32 broadcastAddress;
		public IPSegment(IPAddress ip, IPAddress mask)
		{
			this.ip = ip.ToUInt32();
			this.mask = mask.ToUInt32();
			networkAddress = this.ip & this.mask;
			broadcastAddress = networkAddress + ~this.mask;
		}
		public UInt32 NumberOfIPs
		{
			get
			{
				return ~(mask.ReverseBytes()) - 1;
			}
		}

		public IPAddress NetworkAddress
		{
			get
			{
				return networkAddress.ToIPAddress();
			}
		}
		public IPAddress BroadcastAddress
		{
			get
			{
				return broadcastAddress.ToIPAddress();
			}
		}
		public IEnumerable<IPAddress> Hosts()
		{
			var net = networkAddress.ReverseBytes();
			var broad = broadcastAddress.ReverseBytes();
			for (var host = net + 1; host < broad; host++)
			{
				yield return host.ReverseBytes().ToIPAddress();
			}
		}
	}
}
