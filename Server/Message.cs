using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json;

namespace Server
{
    public enum Commands
    {
        Register,
        Delete
    }
    public class Message
    {
        public Commands command {  get; set; }

        public string Text { get; set; }

        public DateTime DateTime { get; set; }

        public string NicknameFrom { get; set; }

        public string NicknameTo { get; set; }

        public string SerializeMessageToJson() => JsonSerializer.Serialize(this);

        public static Message? DesirializeFromJson(string message) => JsonSerializer.Deserialize<Message>(message);

        public void Print()
        { 
            Console.WriteLine(ToString());
        }

        public override string ToString()
        {
            return $"{this.DateTime} получено сообщение {this.Text} от {this.NicknameFrom}";
        }

        public static Message DeserializeFromJson(string jsonData)
        {
            return System.Text.Json.JsonSerializer.Deserialize<Message>(jsonData);
        }
    }
}
