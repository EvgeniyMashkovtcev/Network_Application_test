using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Server("Hello");


        }
        public static void task1()
        {
            Message msg = new Message() { Text = "Hello", DateTime = DateTime.Now, NicknameFrom = "Evg", NicknameTo = "All" };
            string json = msg.SerializeMessageToJson();
            Console.WriteLine(json);
            Message? msgDesirialized = Message.DesirializeFromJson(json);
        }

        public static void Server(string name)
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("Сервер ждет сообщение от клиента");

            while (true)
            {
                try
                {
                    byte[] buffer = udpClient.Receive(ref iPEndPoint);
                    var messageText = Encoding.UTF8.GetString(buffer);
                    Message message = Message.DesirializeFromJson(messageText);
                    message.Print();

                    string confirmation = "Сообщение получено: " + message.Text;
                    byte[] confirmationData = Encoding.UTF8.GetBytes(confirmation);
                    udpClient.Send(confirmationData, confirmationData.Length, iPEndPoint);
                    Console.WriteLine("Подтверждение отправлено: " + confirmation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при получении или отправке сообщения: " + ex.Message);
                }
            }
        }

    }
}