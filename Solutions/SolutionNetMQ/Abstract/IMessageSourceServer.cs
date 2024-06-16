using System;

public class IMessageSourceServer
{
    public interface IMessageSourceServer<T>
    {
        T CreateEndpoint();

        void SendAsync(NetMessage message, T endpoint);

        NetMessage Receive(ref T endpoint);
    }
}
