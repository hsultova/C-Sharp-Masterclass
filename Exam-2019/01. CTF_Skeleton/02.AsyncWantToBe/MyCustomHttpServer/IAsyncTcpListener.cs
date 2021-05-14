using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyCustomHttpServer
{
	public interface IAsyncTcpListener
	{
		Task StartAsync();
		Task StopAsync();
		Task<TcpClient> AcceptTcpClientAsync();
	}
}
