using System.Net.Sockets;
using System.Net;
using System.Text;
using EF;
using Server;
using Microsoft.Identity.Client;

namespace Entity_Framework
{
    internal class UDPServer
    {
        class Server
        {
            Dictionary<String, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
            UdpClient UdpClient;

            void Register(NetMessage message, IPEndPoint fromep)
            {
                Console.WriteLine("Message Register, name = " + message.NicknameFrom);
                clients.Add(message.NicknameFrom, fromep);


                using (var ctx = new ChatContext())
                {
                    if (ctx.Users.FirstOrDefault(x => x.FullName == message.NicknameFrom) != null) return;

                    ctx.Add(new User { FullName = message.NicknameFrom });

                    ctx.SaveChanges();
                }

                void ConfirmMessageReceived(int? id)
                {
                    Console.WriteLine("Message confirmation id = " + id);

                    using (var ctx = new ChatContext())
                    {
                        var msg = ctx.Messages.FirstOrDefault(x => x.MessageId == id);

                        if (msg != null)
                        {
                            msg.IsSent = true;
                            ctx.SaveChanges();
                        }
                    }
                }

                void RelyMessage(NetMessage message)
                {
                    int? id = null;
                    if (clients.TryGetValue(message.NicknameTo, out IPEndPoint ep))
                    {
                        using (var ctx = new ChatContext())
                        {
                            var fromUser = ctx.Users.First(x => x.FullName == message.NicknameFrom);
                            var toUser = ctx.Users.First(x => x.FullName == message.NicknameTo);
                            var msg = new Message() { UserFrom = fromUser, UserTo = toUser, IsSent = false, Text = message.Text };
                            ctx.Messages.Add(msg);

                            ctx.SaveChanges();

                            id = msg.MessageId;
                        }

                        var forwardMessageJson = new Message() { MessageId = (int)id };
                        // var forwardMessageJson = new Message() { Id = id, Command = Command.Message, ToName = message.ToName, FromName = message.NicknameFrom };

                        byte[] forwardBytes = Encoding.ASCII.GetBytes(forwardMessageJson);

                        UdpClient.Send(forwardBytes, forwardBytes.Length, ep);
                        Console.WriteLine($"Message Relied, from = {message.NicknameFrom} to = {message.NicknameTo}");
                    }
                    else
                    {
                        Console.WriteLine("Пользователь не найден.");
                    }
                }

                void ProcessMessage(NetMessage message, IPEndPoint fromep)
                {
                    Console.WriteLine($"Получено сообщение от {message.NicknameFrom} для {message.NicknameTo} с командой {message.command}: ");
                    Console.WriteLine(message.Text);

                    if (message.command == Command.Register)
                    {
                        Register(message, new IPEndPoint(fromep.Address, fromep.Port));

                    }
                    if (message.command == Command.Confirmation)
                    {
                        Console.WriteLine("Confirmation receiver");
                        ConfirmMessageReceived(message.Id);
                    }
                    if (message.command == Command.Message)
                    {
                        RelyMessage(message);
                    }
                }

                public void Work()
                {
                    IPEndPoint remoteEndPoint;

                    UdpClient = new UdpClient(12345);
                    remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                    Console.WriteLine("UDP Клиент ожидает сообщений. . .");

                    while (true)
                    {
                        byte[] receiveBytes = UdpClient.Receive(ref remoteEndPoint);
                        string receiveData = Encoding.ASCII.GetString(receiveBytes);

                        Console.WriteLine(receiveData);

                        try
                        {
                            var message = NetMessage.DeserializeFromJson(receiveData);

                            ProcessMessage(message, remoteEndPoint);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
                        }
                    }
                }
            }
        }

    }  
}
