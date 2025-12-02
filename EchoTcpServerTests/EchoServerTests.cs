using NUnit.Framework;
using EchoTcpServer;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EchoTcpServerTests
{
    public class EchoServerTests
    {
        private EchoServer _server;
        private const int Port = 5001; // Використовуємо порт 5001 для тестів

        [SetUp]
        public void Setup()
        {
            _server = new EchoServer(Port);
            _ = _server.StartAsync(); // Start server
        }

        [TearDown]
        public void TearDown()
        {
            _server.Stop(); // Stop server
        }

        [Test]
        public async Task Server_Should_Echo_Message()
        {
            // Arrange
            using var client = new TcpClient();
            await client.ConnectAsync("127.0.0.1", Port);
            using var stream = client.GetStream();

            string message = "Hello Lab 6!";
            byte[] data = Encoding.UTF8.GetBytes(message);
            byte[] buffer = new byte[1024];

            // Act
            await stream.WriteAsync(data, 0, data.Length);
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Assert
            Assert.That(response, Is.EqualTo(message));
        }
    }
}
