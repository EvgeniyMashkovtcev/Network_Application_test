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

            bool isRunning = true;

            while (isRunning)
            {
                byte[] buffer = udpClient.Receive(ref iPEndPoint);
                var messageText = Encoding.UTF8.GetString(buffer);

                if (messageText.ToLower() == "exit")
                {
                    isRunning = false;
                    continue;
                }

                ThreadPool.QueueUserWorkItem(obj => {
                    Message message = Message.DesirializeFromJson(messageText);
                    message.Print();

                    byte[] reply = Encoding.UTF8.GetBytes("Сообщение получено");
                    udpClient.Send(reply, reply.Length, iPEndPoint);
                });
            }
        }

    }
}