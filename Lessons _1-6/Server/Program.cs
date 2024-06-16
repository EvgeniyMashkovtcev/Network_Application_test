using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {

            CancellationTokenSource cts = new CancellationTokenSource();
            Server("Hello", cts.Token);

            // Server("Hello");


        }
        public static void task1()
        {
            Message msg = new Message() { Text = "Hello", DateTime = DateTime.Now, NicknameFrom = "Evg", NicknameTo = "All" };
            string json = msg.SerializeMessageToJson();
            Console.WriteLine(json);
            Message? msgDesirialized = Message.DesirializeFromJson(json);
        }

        public static void Server(string name, CancellationToken token)
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("Сервер ждет сообщение от клиента");

            Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (udpClient.Available > 0)
                    {
                        byte[] buffer = udpClient.Receive(ref iPEndPoint);
                        var messageText = Encoding.UTF8.GetString(buffer);

                        if (messageText.ToLower() == "exit")
                        {
                            break;
                        }

                        Message message = Message.DesirializeFromJson(messageText);
                        message.Print();

                        byte[] reply = Encoding.UTF8.GetBytes("Сообщение получено");
                        udpClient.Send(reply, reply.Length, iPEndPoint);
                        // Console.WriteLine($"Отправлено {reply.Length}");
                    }
                }
            }, token);

            
            Console.ReadKey();
            udpClient.Close();
        }

    }
}