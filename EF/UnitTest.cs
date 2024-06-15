using System;

public class UnitTest
{
    [TestFixture]
    public class ClientTests
    {
        [Test]
        public void Client_Register_SendsRegistrationMessage()
        {
            var client = new Client("Evgeny", "127.0.0.1", 12345);
            var mockMessageSource = new Mock<IMessageSource>();
            client.SetMessageSource(mockMessageSource.Object);

            client.Register();

            mockMessageSource.Verify(ms => ms.SendAsync(It.IsAny<NetMessage>(), It.IsAny<IPEndPoint>()), Times.Once());
        }

        [Test]
        public async Task Client_Confirm_SendsConfirmationMessage()
        {
            var client = new Client("Evge", "127.0.0.1", 12345);
            var mockMessageSource = new Mock<IMessageSource>();
            client.SetMessageSource(mockMessageSource.Object);
            var message = new NetMessage { Command = Command.Message, NickNameFrom = "Nisa", Text = "Привет, Hi" };

            await client.Confirm(message, new IPEndPoint(IPAddress.Loopback, 12345));

            mockMessageSource.Verify(ms => ms.SendAsync(It.Is<NetMessage>(m => m.Command == Command.Confirmation), It.IsAny<IPEndPoint>()), Times.Once());
        }

        [Test]
        public void ClientListener_ReceivesMessages_PrintsToConsole()
        {
            var client = new Client("Evgenyii", "127.0.0.1", 12345);
            var mockMessageSource = new Mock<IMessageSource>();
            client.SetMessageSource(mockMessageSource.Object);
            var message = new NetMessage { Command = Command.Message, NickNameFrom = "Bob", Text = "Привет, Hello" };
            mockMessageSource.Setup(ms => ms.Receive(It.IsAny<IPEndPoint>())).Returns(message);

            var listenerTask = client.ClientListener();

        }
    }

}

