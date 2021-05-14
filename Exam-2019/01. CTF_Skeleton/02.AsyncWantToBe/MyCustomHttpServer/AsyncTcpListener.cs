using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MyCustomHttpServer
{
	public class AsyncTcpListener : IAsyncTcpListener
	{
		private readonly TcpListener _tcpListener;

		public AsyncTcpListener(IPAddress address, int port )
		{
			_tcpListener = new TcpListener(address, port);
		}

		public async Task<TcpClient> AcceptTcpClientAsync()
		{
			return await _tcpListener.AcceptTcpClientAsync();
		}

		public Task StartAsync()
		{
			_tcpListener.Start();
			return Task.CompletedTask;
		}

		public Task StopAsync()
		{
			_tcpListener.Stop();
			return Task.CompletedTask;
		}
	}
}
