using System;

public class IMessageSouceClient
{
	public interface IMessageSouceClient<T>
	{
		Task SendAsync(NetMessage message, T ep);

		NetMessage Receive(ref T ep);

        T CreateEndpoint();

        T GetServer();
    }
}
