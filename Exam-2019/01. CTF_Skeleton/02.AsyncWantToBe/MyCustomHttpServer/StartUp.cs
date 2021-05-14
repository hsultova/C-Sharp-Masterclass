using System;
using System.Threading;

namespace MyCustomHttpServer
{
    public static class StartUp
    {
		public static void Main(string[] args)
        {
            HttpServer server = new HttpServer();
			_ = server.StartAsync();

			Console.WriteLine("Press to stop");
            Console.ReadKey();
			_ = server.StopAsync();
		}
    }
}