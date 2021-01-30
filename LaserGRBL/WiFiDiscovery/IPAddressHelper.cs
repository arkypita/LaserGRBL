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

		public static byte[] pbuff = new byte[] { 1, 2, 3, 4 };
		private static string AVAIL = "Available";

		[DllImport("iphlpapi.dll", ExactSpelling = true)]
		public static extern int SendARP(uint DestIP, uint SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);
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
			public event System.EventHandler Update;
			
			private bool notified;

			private long mPingTime;
			public string MAC;
			public string HostName;
			public string Telnet;

			public IPAddress IP;
			public int Port;

			public ScanResult(IPAddress address, int port, long roundtripTime)
			{
				IP = address;
				Port = port;
				mPingTime = roundtripTime;
				notified = true;
			}

			public ScanResult(IPAddress address, int port)
			{
				IP = address;
				Port = port;
				mPingTime = -1;
				notified = false;
			}

			internal bool HasData()
			{
				return mPingTime >= 0 || MAC != null || HostName != null || Telnet == AVAIL;
			}

			internal void DeepScan(Action<ScanResult> mCallback, CancellationToken ct)
			{
				string oldval = ToString();
				List<Task> T = new List<Task>();

				T.Add(Task.Factory.StartNew(() => { SetPing(IP.Ping()); }));
				T.Add(Task.Factory.StartNew(() => { MAC = IP.GetMAC(); }));
				T.Add(Task.Factory.StartNew(() => { HostName = IP.GetHostName(); }));
				T.Add(Task.Factory.StartNew(() => { Telnet = IP.Telnet(Port); }));

				while (T.Count > 0)
				{
					int et = Task.WaitAny(T.ToArray(), -1, ct);
					if (et >= 0)
					{
						T.RemoveAt(et);
					
						if (!notified && HasData())
						{
							notified = true;
							mCallback.Invoke(this);
						}

						string newval = ToString();
						if (HasData() && Update != null && oldval != newval)
							Update.Invoke(null, null);

						oldval = newval;
					}
				}
			}

			private void SetPing(long v)
			{
				if (v >= 0)
					mPingTime = Math.Min(mPingTime, v);
			}

			public string Ping => mPingTime >= 0 ? $"{mPingTime}ms" : null;

			public override string ToString()
			{
				return $"{Ping}{HostName}{MAC}{Telnet}";
			}
		}

		private static string GetMAC(this IPAddress ip, int retryCount = 1)
		{
			byte[] mac = new byte[6];
			uint macLen = (uint)mac.Length;
			for (var j = 0; j < retryCount; j++)
			{
				var rc = SendARP(ip.ToUInt32(), 0, mac, ref macLen);
				if (rc == 0)
				{
					var str = new string[macLen];
					for (int i = 0; i < macLen; i++)
						str[i] = mac[i].ToString("x2");
					return string.Join(":", str);
				}
				else 
				{
				}
			}
			return null;
		}

		private static string GetHostName(this IPAddress ip)
		{
			try
			{
				IPHostEntry entry = Dns.GetHostEntry(ip);
				if (entry != null)
					return entry.HostName;
			}
			catch (SocketException ex)
			{
				//unknown host or
				//not every IP has a name
				//log exception (manage it)
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
				}
			}
			catch { }

			return "Failed!";
		}

		private static long Ping(this IPAddress ip)
		{
			Ping P = new Ping();

			PingOptions PO = new PingOptions(16, true);
			PingReply reply = P.Send(ip, 5000, pbuff, PO);

			if (reply.Status == IPStatus.Success)
				return reply.RoundtripTime;
			else
				return -1;
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
		public static void ScanIP(Action<ScanResult> callback, Action<int, int, int> progress, CancellationToken ct, int port, IPAddress paramIP = null)
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

			IPSegment segment = new IPSegment(localIP, localMask);
			IPSegmentScanner scanner = new IPSegmentScanner(segment, callback, progress, port, ct);

			scanner.FastScan();
			//scanner.FullScan();
	
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

	class IPSegmentScanner
	{
		IPSegment mSegment;
		Action<IPAddressHelper.ScanResult> mCallback;
		Action<int, int, int> mProgress;
		CancellationToken mCt;
		private int mRespCount;
		private int mPort;

		List<IPAddressHelper.ScanResult> mList;

		public IPSegmentScanner(IPSegment segment, Action<IPAddressHelper.ScanResult> callback, Action<int, int, int> progress, int port, CancellationToken ct)
		{
			mList = new List<IPAddressHelper.ScanResult>();

			mSegment = segment;
			mCallback = callback;
			mProgress = progress;
			mCt = ct;
			mPort = port;
		}


		public void FullScan()
		{
			BuildFullList();
			Analize();
		}

		public void FastScan()
		{
			BuildRespondingList();
			Analize();
		}

		private void BuildFullList()
		{
			foreach (IPAddress host in mSegment.HostToScan)
				mList.Add(new IPAddressHelper.ScanResult(host, mPort));
		}

		private void BuildRespondingList()
		{
			mRespCount = 0;
			mProgress(0, 0, mSegment.HostToScan.Count);
			SpinWait wait = new SpinWait();

			PingOptions PO = new PingOptions(16, true);

			for (int i = 0; i < mSegment.HostToScan.Count && !mCt.IsCancellationRequested; i++)
			{
				Ping P = new Ping();
				P.PingCompleted += PingCompleted;
				P.SendAsync(mSegment.HostToScan[i], 5000, IPAddressHelper.pbuff, PO);
				if (i % 16 == 1)
					Thread.Sleep(100);
			}

			while (mRespCount < mSegment.HostToScan.Count && !mCt.IsCancellationRequested)
			{
				mProgress(0, mRespCount, mSegment.HostToScan.Count);
				wait.SpinOnce();
			}

			mProgress(0, mRespCount, mSegment.HostToScan.Count);
		}

		private void PingCompleted(object sender, PingCompletedEventArgs e)
		{
			Ping P = sender as Ping;
			try
			{
				if (e.Reply.Status == IPStatus.Success)
				{
					IPAddressHelper.ScanResult SR = new IPAddressHelper.ScanResult(e.Reply.Address, mPort, e.Reply.RoundtripTime);
					mList.Add(SR);
					mCallback.Invoke(SR);
				}
			}
			finally
			{
				lock (this)
					mRespCount++;

				P.PingCompleted -= PingCompleted;
				P.Dispose();
			}
		}


		private void Analize()
		{
			int done = 0;
			mProgress.Invoke(1, done, mList.Count);

			ParallelOptions po = new ParallelOptions();
			po.CancellationToken = mCt;
			po.MaxDegreeOfParallelism = -1;

			try
			{
				if (!mCt.IsCancellationRequested)
				{
					Parallel.ForEach<IPAddressHelper.ScanResult>(mList, po, (sr) =>
					{
						if (!mCt.IsCancellationRequested)
						{
							sr.DeepScan(mCallback, mCt);

							Interlocked.Increment(ref done);
							mProgress.Invoke(1, done, mList.Count);

							mCt.ThrowIfCancellationRequested();
						}
					});
				}
			}
			catch (OperationCanceledException e)
			{
				Debug.WriteLine(e.Message);
			}
		}
	}

	class IPSegment
	{
		private UInt32 ip;
		private UInt32 mask;
		private UInt32 networkAddress;
		private UInt32 broadcastAddress;

		private List<IPAddress> mAllHost;

		public IPSegment(IPAddress ip, IPAddress mask)
		{
			this.ip = ip.ToUInt32();
			this.mask = mask.ToUInt32();
			networkAddress = this.ip & this.mask;
			broadcastAddress = networkAddress + ~this.mask;

			mAllHost = new List<IPAddress>();
			uint net = networkAddress.ReverseBytes();
			uint broad = broadcastAddress.ReverseBytes();
			for (uint host = net + 1; host < broad; host++)
				mAllHost.Add(host.ReverseBytes().ToIPAddress());
		}

		public IPAddress NetworkAddress => networkAddress.ToIPAddress();
		public IPAddress BroadcastAddress => broadcastAddress.ToIPAddress();

		public List<IPAddress> HostToScan => mAllHost;
	}
}
