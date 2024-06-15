using System;

public class UdpMessageSouce
{
	internal class UdpMessageSouce : IMessageSouce
	{
		private readonly UdpClient _udpClient;

		public UdpMessageSouce()
		{
			_udpClient = new UdpClient(12345);
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
