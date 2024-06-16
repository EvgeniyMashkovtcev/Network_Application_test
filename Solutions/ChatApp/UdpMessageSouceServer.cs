using System;

public class UdpMessageSouceServer
{
	public class UdpMessageSouceServer : IMessageSouceServer<IPEndPoint>
	{
		private readonly UdpClient _udpClient;

		public UdpMessageSouceServer()
		{
			_udpClient = new UdpClient(12345);
		}

		public IPEndPoint CopyEndpoint(IPEndPoint ep)
		{
			return new IPEndPoint(ep.Address, ep.Port);
		}

        public IPEndPoint CreateEndpoint(IPEndPoint ep)
        {
            return new IPEndPoint(IPAddress.Any, 0);
        }

        public NetMessage Receive(ref IPEndPoint ep)
		{
			byte[] data = _udpClient.Receive(ref ep);
			string str = Encoding.UTF8.GetString(data);
			return NetMessage.DeserializeMessgeFromJson(str) ?? new NetMessage();
		}

		public async Task SendAsync(NetMessage message, IPEndPoint ep)
		{
			byte[] buffer = Encoding.UTF8.GetBytes
				(message.SerialazeMessageToJson());

			await _udpClient.SendAsync(buffer, buffer.Length, ep);
		}
	}

}
