using NetArchTest.Rules;
using NetSdrClientApp.Networking; // include namespace
using NUnit.Framework;

namespace NetSdrClientAppTests
{
    public class ArchitectureTests
    {
        [Test]
        public void Interfaces_Should_Have_Prefix_I()
        {
            // тут таке правило що інтерфейси у проекті NetSdrClientApp повинні починатися на I
            var result = Types.InAssembly(typeof(ITcpClient).Assembly)
                .That()
                .AreInterfaces()
                .Should()
                .HaveNameStartingWith("I")
                .GetResult();

            Assert.That(result.IsSuccessful, Is.True, "All interfaces must start with 'I'");
        }
    }
}
