using System.Text.Json;

namespace ChatCommon
{
    public enum Command
    {
        Register,
        Message,
        Delete,
        Confirmation
    }
    public class NetMessage
    {
        public int Id {  get; set; }

        public Command command { get; set; }

        public string? Text { get; set; }

        public DateTime DateTime { get; set; }

        public string? NicknameFrom { get; set; }

        public string? NicknameTo { get; set; }

        public IPEndPoint? EndPoint { get; set; }

        public string SerializeMessageToJson() => JsonSerializer.Serialize(this);

        public static NetMessage? DesirializeFromJson(string message) => JsonSerializer.Deserialize<NetMessage>(message);

        public void Print()
        {
            Console.WriteLine(ToString());
        }

        public override string ToString()
        {
            return $"{this.DateTime} получено сообщение {this.Text} от {this.NicknameFrom}";
        }

        public static NetMessage DeserializeFromJson(string jsonData)
        {
            return System.Text.Json.JsonSerializer.Deserialize<NetMessage>(jsonData);
        }
    }
}

