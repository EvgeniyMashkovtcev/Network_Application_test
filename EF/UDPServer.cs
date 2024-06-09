using System.Net.Sockets;
using System.Net;
using System.Text;
using EF;
using Server;

namespace Entity_Framework
{
    internal class UDPServer
    {
        public async Task ServerListenerAsync()
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("Сервер ждет сообщение от клиента");

            CancellationTokenSource cts = new CancellationTokenSource();
            bool canWork = true;

            while (!cts.IsCancellationRequested)
            {
                byte[] buffer = udpClient.Receive(ref iPEndPoint);
                var messageText = Encoding.UTF8.GetString(buffer);

                byte[] reply = Encoding.UTF8.GetBytes("Сообщение получено");

                int bytes = await udpClient.SendAsync(reply, iPEndPoint);

                NetMessage? message = NetMessage.DesirializeFromJson(messageText);
                if (message.Text.ToLower().Equals("exit")) cts.Cancel();
                message.Print();
            }

        }

    }  
}
