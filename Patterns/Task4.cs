using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
// using static Patterns.Task4;

namespace Patterns
{
    public interface IObserver
    {
        void Update(Message message);
    }
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(Message message);
    }
    public class MessageSubject : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(Message message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
    }
    public class ClientObserver : IObserver
    {
        public void Update(Message message)
        {
            Console.WriteLine($"Сообщение от {message.NicknameFrom}: {message.Text}");
        }
    }

namespace Client
    {
        internal class Program
        {
            public class ClientProgram
            {
                public static void Main(string[] args)
                {
                    var subject = new MessageSubject();
                    var observer = new ClientObserver();
                    subject.Attach(observer);

                    TcpClient client = new TcpClient("server_address", 12345);
                    NetworkStream stream = client.GetStream();

                    
                    string messageToSend = "Привет от клиента!";
                    byte[] dataToSend = Encoding.UTF8.GetBytes(messageToSend);
                    stream.Write(dataToSend, 0, dataToSend.Length);

                    
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
                    string messageJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Message message = Message.DeserializeFromJson(messageJson);

                    subject.Notify(message);

                    client.Close();
                }
            }
        }
    }

namespace Server
    {
        internal class Program
        {
            static void Main(string[] args)
            {
                var subject = new MessageSubject();

                CancellationTokenSource cts = new CancellationTokenSource();
                Server(subject, cts.Token);

                
            }

            public static void Server(ISubject subject, CancellationToken token)
            {
                TcpListener listener = new TcpListener(IPAddress.Any, 12345);
                listener.Start();

                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        if (listener.Pending())
                        {
                            TcpClient client = listener.AcceptTcpClient();
                            NetworkStream stream = client.GetStream();

                            byte[] buffer = new byte[client.ReceiveBufferSize];
                            int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
                            string messageJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            Message message = Message.DeserializeFromJson(messageJson);

                            subject.Notify(message);

                            client.Close();
                        }
                    }
                }
                finally
                {
                    listener.Stop();
                }
            }
        }
    }

}


