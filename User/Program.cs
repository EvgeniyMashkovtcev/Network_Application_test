using Server;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {

            SentMessage(args[0], args[1]);


        }
        public static void SentMessage(string From, string ip)
        {

            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);

            while (true)
            {
                string messageText;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Введите сообщение");
                    messageText = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(messageText));

                Message message = new Message() { Text = messageText, DateTime = DateTime.Now, NicknameFrom = From, NicknameTo = "All" };
                string json = message.SerializeMessageToJson();

                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(json);
                    udpClient.Send(data, data.Length, iPEndPoint);
                    Console.WriteLine("Сообщение отправлено: " + json);

                    var serverEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receivedData = udpClient.Receive(ref serverEndPoint);
                    string confirmation = Encoding.UTF8.GetString(receivedData);
                    Console.WriteLine("Подтверждение получено: " + confirmation);

                    string deliveryConfirmation = "Ваше сообщение доставлено";
                    byte[] deliveryConfirmationData = Encoding.UTF8.GetBytes(deliveryConfirmation);
                    udpClient.Send(deliveryConfirmationData, deliveryConfirmationData.Length, serverEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при отправке или получении подтверждения: " + ex.Message);
                }

            }


        }


    }
}
