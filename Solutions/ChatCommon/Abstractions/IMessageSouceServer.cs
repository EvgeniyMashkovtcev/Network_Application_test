using System;

public class IMessageSouceServer
{
	public interface IMessageSouceServer<T>
	{
		Task SendAsync(NetMessage message, T ep);

		NetMessage Receive(ref T ep);

		T CreateEndpoint();

        T CopyEndpoint(IPEndPoint ep);
    }
}
