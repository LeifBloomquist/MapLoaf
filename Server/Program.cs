namespace MapLoafServer
{
    internal class Program
    {
        static readonly CancellationTokenSource Cts = new();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting MapLoafServer...");

            bool local = false;

            if (args.Length >= 1)
            {
                if (args[0] == "local")
                {
                    Console.WriteLine("(Local)");
                    local = true;
                }
            }

            TextMap TheMap = new(@"C:\python\maze101.txt");
     
            new HttpServer().Start(local, TheMap);

            await Task.Delay(Timeout.Infinite, Cts.Token);
        }
    }
}
