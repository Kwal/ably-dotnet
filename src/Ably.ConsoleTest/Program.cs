using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IO.Ably.ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            IO.Ably.DefaultLogger.LoggerSink = new MyLogger();
            DefaultLogger.LogLevel = LogLevel.Debug;
            try
            {
                var client = new AblyRealtime(new ClientOptions("<API Key Here>"));
                var channel = client.Channels.Get(
                    Guid.NewGuid().ToString(),
                    new ChannelOptions(Convert.FromBase64String("dDGE8dYl8M9+uyUTIv0+ncs1hEa++HiNDu75Dyj4kmw="))
                );

                await channel.PublishAsync(new Message(null, "This is a test", Guid.NewGuid().ToString()));

                Console.ReadLine();
                ConsoleColor.Green.WriteLine("Success!");
            }
            catch (Exception ex)
            {
                ex.LogError();
            }
        }

        private static void Presence_MessageReceived2(PresenceMessage obj)
        {
            Console.WriteLine(obj.ConnectionId + "\t" + obj.Timestamp);
        }
    }
}
