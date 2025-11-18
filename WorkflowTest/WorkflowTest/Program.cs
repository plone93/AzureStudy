using System;
using System.Text;
using System.Text.Json;
using Azure.Storage.Queues;

namespace QueueConsoleApp
{
    public class QueueMessage
    {
        public string MessageText { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Priority { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=YOUR_ACCOUNT;AccountKey=YOUR_KEY;EndpointSuffix=core.windows.net";

            string queueName = "queue-test";

            var queueClient = new QueueClient(connectionString, queueName);

            queueClient.CreateIfNotExists();

            if (!queueClient.Exists())
            {
                Console.WriteLine("Queue 생성 실패");
                return;
            }

            // =====================
            // 5️⃣ 단순 문자열 메시지
            string simpleMessage = "Hello, Azure Queue!";
            queueClient.SendMessage(simpleMessage);
            Console.WriteLine("단순 메시지 전송 완료");

            // =====================
            // 6️⃣ JSON 객체 메시지 (Base64 인코딩)
            var messageObject = new QueueMessage
            {
                MessageText = "Hello, JSON Queue!",
                CreatedAt = DateTime.UtcNow,
                Priority = "High"
            };

            string jsonMessage = JsonSerializer.Serialize(messageObject);

            // Base64 인코딩 (안전하게 전달)
            string base64Message = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonMessage));
            queueClient.SendMessage(base64Message);
            Console.WriteLine("JSON 메시지(Base64) 전송 완료");
        }
    }
}
