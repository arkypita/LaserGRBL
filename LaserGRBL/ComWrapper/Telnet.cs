using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace LaserGRBL.ComWrapper
{
	class Telnet : IComWrapper
	{
		private string mAddress;

		private System.Net.Sockets.TcpClient cln;
		BinaryWriter bwriter;
		StreamReader sreader;
		StreamWriter swriter;

		public void Configure(params object[] param)
		{
			mAddress = (string)param[0];
		}

		public void Open()
		{

			if (cln != null)
				Close(true);

			if (string.IsNullOrEmpty(mAddress))
				throw new MissingFieldException("Missing HostName");

			cln = new System.Net.Sockets.TcpClient();
			Logger.LogMessage("OpenCom", "Open {0}", mAddress);
			cln.Connect(IPHelper.Parse(mAddress));

			Stream cst = cln.GetStream();
			bwriter = new BinaryWriter(cst);
			sreader = new StreamReader(cst, Encoding.ASCII);
			swriter = new StreamWriter(cst, Encoding.ASCII);
		}

		public void Close(bool auto)
		{
			if (cln != null)
			{
				try
				{
					Logger.LogMessage("CloseCom", "Close {0} [{1}]", mAddress, auto ? "CORE" : "USER");
					cln.Close();
				}
				catch { }

				cln = null;
				bwriter = null;
				sreader = null;
				swriter = null;
			}
		}

		public bool IsOpen
		{
			get { return cln != null && cln.Connected; }
		}

		public void Write(byte b)
		{
			bwriter.Write(b);
			bwriter.Flush();
		}

		public void Write(string text)
		{
			swriter.Write(text);
			swriter.Flush();
		}

		public string ReadLineBlocking()
		{
			string rv = null;
			while (IsOpen && rv == null) //wait for disconnect or data
			{
				rv = sreader.ReadLine();

				if (rv == null)
					System.Threading.Thread.Sleep(1);
			}

			return rv;
		}

		public bool HasData()
		{ return IsOpen && ((System.Net.Sockets.NetworkStream)sreader.BaseStream).DataAvailable; }
	}


	class IPHelper
	{
		public static IPEndPoint Parse(string endpointstring)
		{
			IPEndPoint rv =  Parse(endpointstring, -1);
			return rv;
		}

		public static IPEndPoint Parse(string endpointstring, int defaultport)
		{
			if (string.IsNullOrEmpty(endpointstring)
				|| endpointstring.Trim().Length == 0)
			{
				throw new ArgumentException("Endpoint descriptor may not be empty.");
			}

			if (defaultport != -1 &&
				(defaultport < IPEndPoint.MinPort
				|| defaultport > IPEndPoint.MaxPort))
			{
				throw new ArgumentException(string.Format("Invalid default port '{0}'", defaultport));
			}

			string[] values = endpointstring.Split(new char[] { ':' });
			IPAddress ipaddy;
			int port = -1;

			//check if we have an IPv6 or ports
			if (values.Length <= 2) // ipv4 or hostname
			{
				if (values.Length == 1)
					//no port is specified, default
					port = defaultport;
				else
					port = getPort(values[1]);

				//try to use the address as IPv4, otherwise get hostname
				if (!IPAddress.TryParse(values[0], out ipaddy))
					ipaddy = getIPfromHost(values[0]);
			}
			else if (values.Length > 2) //ipv6
			{
				//could [a:b:c]:d
				if (values[0].StartsWith("[") && values[values.Length - 2].EndsWith("]"))
				{
					string ipaddressstring = string.Join(":", values.Take(values.Length - 1).ToArray());
					ipaddy = IPAddress.Parse(ipaddressstring);
					port = getPort(values[values.Length - 1]);
				}
				else //[a:b:c] or a:b:c
				{
					ipaddy = IPAddress.Parse(endpointstring);
					port = defaultport;
				}
			}
			else
			{
				throw new FormatException(string.Format("Invalid endpoint ipaddress '{0}'", endpointstring));
			}

			if (port == -1)
				throw new ArgumentException(string.Format("No port specified: '{0}'", endpointstring));

			return new IPEndPoint(ipaddy, port);
		}

		private static int getPort(string p)
		{
			int port;

			if (!int.TryParse(p, out port)
			 || port < IPEndPoint.MinPort
			 || port > IPEndPoint.MaxPort)
			{
				throw new FormatException(string.Format("Invalid end point port '{0}'", p));
			}

			return port;
		}

		private static IPAddress getIPfromHost(string p)
		{
			var hosts = Dns.GetHostAddresses(p);

			if (hosts == null || hosts.Length == 0)
				throw new ArgumentException(string.Format("Host not found: {0}", p));

			return hosts[0];
		}
	}
}
