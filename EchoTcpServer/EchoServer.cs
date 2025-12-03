using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace EchoTcpServer
{
    public class EchoServer
    {
        private readonly int _port;
        private TcpListener _listener;
        private CancellationTokenSource _cancellationTokenSource;

        public int Port => _port; 

        public EchoServer(int port)
        {
            _port = port;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task StartAsync()
        {
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            Console.WriteLine($"Server started on port {_port}.");

            try
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync(_cancellationTokenSource.Token);
                    _ = Task.Run(() => HandleClientAsync(client, _cancellationTokenSource.Token));
                }
            }
            catch (Exception) { }
            finally { _listener.Stop(); }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[8192];
                int bytesRead;
                try
                {
                    while (!token.IsCancellationRequested && 
                           (bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token)) > 0)
                    {
                        await stream.WriteAsync(buffer, 0, bytesRead, token);
                    }
                }
                catch { }
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _listener.Stop(); 
        }
    }
}
