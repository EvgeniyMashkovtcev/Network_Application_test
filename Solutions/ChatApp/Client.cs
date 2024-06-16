using System;

public class Client<T>
{
	private readonly string _name;

	private readonly IMessageSouceClient<T> _messageSouce;
	private T remoteEndPoint;

    public Client(IMessageSouceClient<T> messageSouceClient, string name)
	{
		this._name = name;

		_messageSouce = messageSouceClient;
		remoteEndPoint = _messageSouce.CreateEndpoint();
    }

	UdpClient udpClientClient = new UdpClient();

	async Task ClientListener()
	{
		while (true)
		{
			try
			{
				var messageReceived = _messageSouce.Receive(ref remoteEndPoint);

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
		await _messageSouce.SendAsync(messag, remoteEndPoint);
	}

	void Register(T remoteEndPoint)
	{
		IPEndPoint ep = new IPEndPoint(IPadress.Any, 0);
		var message = new NetMessage() { NickNameFrom = _name, NickNameTo = null, Text = null, Command = Command.Register, EndPoint = ep };
		_messageSouce.SendAsync(message, remoteEndPoint);
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

				await _messageSouce.SendAsync(message, remoteEndPoint);

				Console.WriteLine("Сщщбщение отправлено.");
            }
			catch (Exception ex)
			{
				Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
			}
		}
	}

	public async Task Start()
	{
		// await ClientListener();
		new Thread(async () => await ClientListener()).Start();

		await ClientSender();
	}
	
}
