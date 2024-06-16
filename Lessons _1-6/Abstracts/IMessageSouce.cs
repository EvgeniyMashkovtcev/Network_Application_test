using System;

public class IMessageSouce
{
	public interface IMessageSouce
	{
		Task SendAsync(NetMessage message, IPEndPoint ep);

		NetMessage Receive(ref IPEndPoint ep);
	}
}
