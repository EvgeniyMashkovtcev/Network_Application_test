using System;

public class UdpMessageSouceClient
{
	public class UdpMessageSouceClient : IMessageSouceClient
	{
		private readonly UdpClient _udpClient;
		private readonly IPEndPoint _udpEndPoint;

		public UdpMessageSouceClient(string Ip = "172.0.0.1", int port)
		{
			_udpClient = new UdpClient(12345);
			_udpEndPoint = new IPEndPoint(EPAddress.Parse(Ip), port)
		}

        public IPEndPoint GetServer()
        {
            return _udpEndPoint;
        }

        public IPEndPoint CreateEndpoint()
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
