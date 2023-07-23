using System.Net;

namespace SmsApiFreeMobile.CliClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string user, pass, msg;
            if (args.Length > 2)
            {
                user = args[0];
                pass = args[1];
                msg = args[2];
            }
            else
            {
                Console.Write("Utilisateur : ");
                user = Console.ReadLine() ?? string.Empty;
                Console.Write("Clé d'identification : ");
                pass = Console.ReadLine() ?? string.Empty;
                Console.Write("Message : ");
                msg = Console.ReadLine() ?? string.Empty;
            }

            SmsApi sms = new(user, pass, msg);
            HttpStatusCode statusCode = await sms.CallApiAsync();

            Console.WriteLine(SmsApi.DetermineMessage(statusCode));
        }
    }
}