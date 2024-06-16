using System;

public class Server<T>
{
	Dictionary<string, T> clients = new Dictionary<string, T>();
	private readonly IMessageSouceServer<T> _messageSouce;
	private T ep;

    public Server(IMessageSouceServer<T> messageSouce)
	{
		_messageSouce = messageSouce;
		ep = = _messageSouce.CreateEndpoint();
    }

	bool work = true;
	public void Stop()
	{
		work = false;
	}

	private async Task Register(NetMessage message)
	{
		Console.WriteLine($"Message Register name = {message.NickNameFrom}");

		if(clients.TryAdd(message.NickNameFrom, _messageSouce.CopyEndpoint(message.IPEndPoint)))
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
		if (clients.TryGetValue(message.NickNameTo, out T ep))
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
