using System;

public class IMessageSourceClient
{
    public interface IMessageSourceClient<T>
    {
        T CreateEndpoint();

        void SendAsync(NetMessage message, T endpoint);

        NetMessage Receive(ref T endpoint);
    }
}