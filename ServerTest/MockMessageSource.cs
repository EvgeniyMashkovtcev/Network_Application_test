using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework.Interfaces;
using System;
using System.Data;
using System.Net;

public class MockMessageSource : IMessageSouce
{
	private Queue<NetMessage> messages = new ();

	private Server server;

	private IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);



	public MockMessageSource()
	{
		messages.Enqueue(new NetMessage { Command = Command.Register, NickNameFrom = "Вася" });
        messages.Enqueue(new NetMessage { Command = Command.Register, NickNameFrom = "Юля" });
        messages.Enqueue(new NetMessage { Command = Command.Message, NickNameFrom = "Юля", NickNameYo = "Вася", Text = "От Юли" });
        messages.Enqueue(new NetMessage { Command = Command.Message, NickNameFrom = "Вася", NickNameTo = "Юля", Text = "От Васи" });
    }

	public async Task SendAsync(NetMessage message, IPEndPoint ep)
	{
		// throw new NotImplementedException();
	}

	public NetMessage Receive(ref IPEndPoint ep)
	{
		ep = endPoint;

		if(messages.Count == 0)
		{
			server.Stop();
			return null;
		}
		var msg = messages.Dequeue();
		return msg;
	}

	public void AddServer(Server serv)
	{
		server = serv;
	}

}
