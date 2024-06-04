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
            for (int i = 0; i < 10; i++) 
            {
                SentMessage("Evg");
            }
            
            // SentMessage("Evg");
            
            Console.ReadLine();
            


        }
        public static void SentMessage(string From, string ip = "127.0.0.1")
        {

            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);

            Console.WriteLine("Введите сообщение:");
            string messageText = Console.ReadLine();

            if (messageText.ToLower() == "exit")
            {
                return;
            }

            Message message = new Message() { Text = messageText, NicknameFrom = From, NicknameTo = "Server", DateTime = DateTime.Now };
            string json = message.SerializeMessageToJson();


            byte[] data = Encoding.UTF8.GetBytes(json);
            udpClient.Send(data, data.Length, iPEndPoint);

            byte[] buffer = udpClient.Receive(ref iPEndPoint);
            var answer = Encoding.UTF8.GetString(buffer);

            Console.WriteLine(answer);






        }


    }
}
