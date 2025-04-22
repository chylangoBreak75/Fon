using Confluent.Kafka;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace FonApi.Kafka
{
    public class Confluent
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            IConfiguration config = readConfig();
            const string topic = "topic_0";
            //consume(topic, config);
        }

        public string CreateMessage(string key, string value)
        {
            IConfiguration config = readConfig();
            const string topic = "topic_0";
            produce(topic, config, key, value);
            return "OK";
        }

        public string ReadMessages()
        {
            IConfiguration config = readConfig();
            const string topic = "topic_0";
            consume(topic);
            return "OK_READ";
        }



        public static string produce(string topic, IConfiguration config, string _key, string _msg)
        {
            string res = "";
            // creates a new producer instance
            using (var producer = new ProducerBuilder<string, string>(config.AsEnumerable()).Build())
            {
                producer.Produce(topic, new Message<string, string> { Key = _key, Value = _msg },
                  (deliveryReport) => {
                      if (deliveryReport.Error.Code != ErrorCode.NoError)
                      {
                          //Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                          res = $"Failed to deliver message: {deliveryReport.Error.Reason}";
                      }
                      else
                      {
                          //Console.WriteLine($"Produced event to topic {topic}: key = {deliveryReport.Message.Key,-10} value = {deliveryReport.Message.Value}");
                          res = $"Produced event to topic {topic}: key = {deliveryReport.Message.Key,-10} value = {deliveryReport.Message.Value}";
                      }
                  }
                );

                // send any outstanding or buffered messages to the Kafka broker
                producer.Flush(TimeSpan.FromSeconds(10));
            }
            return res;
        }

        public static string consume(string topic)
        {
            bool waiting = true;
            string res = "";

            IConfiguration config = readConfig();
            config = readConfig();
            config["group.id"] = "csharp-group-1";
            config["auto.offset.reset"] = "earliest";

            // creates a new consumer instance
            using (var consumer = new ConsumerBuilder<string, string>(config.AsEnumerable()).Build())
            {
                consumer.Subscribe(topic);
                while (waiting)
                {
                    var cr = consumer.Consume();
                    //Console.WriteLine($"Consumed event from topic {topic}: key = {cr.Message.Key,-10} value = {cr.Message.Value}");
                    waiting = false;
                    //res = $"Consumed event from topic {topic}: key = {cr.Message.Key,-10} value = {cr.Message.Value}";
                    res = "CONSUMED";
                }

                // closes the consumer connection
                consumer.Close();
            }

            return res;
        }

        public static IConfiguration readConfig()
        {
            return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddIniFile("client.properties", false)
            .Build();
        }

    }
}
