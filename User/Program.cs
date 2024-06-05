using Server;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Client
{
    internal class Program
    {
        
        // static UdpClient udpClient = new UdpClient();
        // static IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);

        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++) 
            {
                SentMessage("Evg");
            }
            
            Console.ReadLine();
            


        }
        public static void SentMessage(string From, string ip = "127.0.0.1")
        {

            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);


        }
        public static void m1()
        {
            Message message = new Message() { Text = "Hey", NicknameFrom = "Evgeni", NicknameTo = "Server", DateTime = DateTime.Now , command = Commands.Register };
            string json = message.SerializeMessageToJson();


            byte[] data = Encoding.UTF8.GetBytes(json);
            udpClient.Send(data, data.Length, iPEndPoint);
        }
        public static void m2()
        {
            Message message = new Message() { Text = "Helllo", NicknameFrom = "Evgeny", NicknameTo = "Server", DateTime = DateTime.Now, command = Commands.Register };
            string json = message.SerializeMessageToJson();


            byte[] data = Encoding.UTF8.GetBytes(json);
            udpClient.Send(data, data.Length, iPEndPoint);
        }
        public static void m3()
        {
            Message message = new Message() { Text = "Holo", NicknameFrom = "Evgeni", NicknameTo = "Evgeny", DateTime = DateTime.Now};
            string json = message.SerializeMessageToJson();


            byte[] data = Encoding.UTF8.GetBytes(json);
            udpClient.Send(data, data.Length, iPEndPoint);
        }
        public static void m4()
        {
            Message message = new Message() { Text = "Helo", NicknameFrom = "Evgeni", NicknameTo = "Server", DateTime = DateTime.Now, command = Commands.Delete };
            string json = message.SerializeMessageToJson();


            byte[] data = Encoding.UTF8.GetBytes(json);
            udpClient.Send(data, data.Length, iPEndPoint);
        }


    }
}
