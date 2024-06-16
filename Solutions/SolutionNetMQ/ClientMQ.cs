using System;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;


public class Client<T>
{
    private readonly string _name;

    private readonly IMessageSourceClient<T> _messageSource;
    private T remoteEndPoint;

    public Client(IMessageSourceClient<T> messageSourceClient, string name)
    {
        _name = name;
        _messageSource = messageSourceClient;
        remoteEndPoint = _messageSource.CreateEndpoint();
    }

    async Task ClientListener()
    {
        while (true)
        {
            try
            {
                var messageReceived = _messageSource.Receive(ref remoteEndPoint);

                Console.WriteLine($"Получено сообщение от {messageReceived.NickNameFrom}: ");
                Console.WriteLine(messageReceived.Text);

                await Confirm(messageReceived, remoteEndPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении сообщения: " + ex.Message);
            }
        }
    }

    async Task Confirm(NetMessage message, T remoteEndPoint)
    {
        message.Command = Command.Confirmation;
        await _messageSource.SendAsync(message, remoteEndPoint);
    }

    void Register(T remoteEndPoint)
    {
        var message = new NetMessage() { NickNameFrom = _name, Command = Command.Register };
        _messageSource.SendAsync(message, remoteEndPoint);
    }

    async Task ClientSender()
    {
        Register(remoteEndPoint);

        while (true)
        {
            try
            {
                Console.Write("Введите имя получателя: ");
                var nameTo = Console.ReadLine();

                Console.Write("Введите сообщение и нажмите Enter: ");
                var messageText = Console.ReadLine();

                var message = new NetMessage() { Command = Command.Message, NickNameFrom = _name, NickNameTo = nameTo, Text = messageText };

                await _messageSource.SendAsync(message, remoteEndPoint);

                Console.WriteLine("Сообщение отправлено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
            }
        }
    }

    public async Task Start()
    {

        new Thread(async () => await ClientListener()).Start();

        await ClientSender();
    }
}


