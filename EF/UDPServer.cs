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
            Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
            private readonly IMessageSouce _messageSouce;
            private IPEndPoint ep;

            public Server()
            {
                _messageSouce = new UdpMessageSouce();
                ep = = new IPEndPoint(IPAddress.Any, 0);
            }

            private async Task Register(NetMessage message)
            {
                Console.WriteLine($"Message Register name = {message.NickNameFrom}");

                if (clients.TryAdd(message.NickNameFrom, message.EndPoint)
        
                {
                    using (ChatContext context = new ChatContext())
                    {
                        context.Users.Add(new User() { FullName = message.NickNameFrom });
                        await context.SaveChangesAsync();
                    }

                }
            }

            private async Task RelyMessage(NetMessage message)
            {
                if (clients.TryGetValue(message.NickNameTo, out IPEndPoint ep))
                {
                    int id = 0;
                    using (var ctx = new ChatContext())
                    {
                        var fromUser = ctx.Users.First(x => x.FullName == message.NicknameFrom);
                        var toUser = ctx.Users.First(x => x.FullName == message.NicknameTo);
                        var msg = new Message() { UserFrom = fromUser, UserTo = toUser, IsSent = false, Text = message.Text };
                        ctx.Messages.Add(msg);

                        ctx.SaveChanges();

                        id = msg.MessageId;
                    }

                    messsage.Id = id;

                    await _messageSouce.SendAsync(message, ep);

                    Console.WriteLine($"Message Relied, from = {message.NicknameFrom} to = {message.NicknameTo}");
                }
                else
                {
                    Console.WriteLine("Пользователь не найден.");
                }
            }

            async Task ConfirmMessageReceived(int? id)
            {
                Console.WriteLine("Message confirmation id = " + id);

                using (var ctx = new ChatContext())
                {
                    var msg = ctx.Messages.FirstOrDefault(x => x.MessageId == id);

                    if (msg != null)
                    {
                        msg.IsSent = true;
                        await ctx.SaveChangesAsync();
                    }
                }
            }

            async Task ProcessMessage(NetMessage message)
            {
                switch (message.Command)
                {
                    case Command.Register:
                        await Register(message);
                        break;
                    case Command.Message:
                        await RelyMessage(message);
                        break;
                    case Command.Confirmation:
                        await ConfirmMessageReceived(message.Id);
                        break;
                    default:
                        break;
                }
            }

            public async Task Start()
            {

                Console.WriteLine("Сервер ожидает сообщения ");

                while (true)
                {
                    try
                    {
                        _messageSouce.Receive(ref ep);
                        Console.WriteLine(message.ToString());
                        await ProcessMessage(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                }
            }
        }

    }  
}
