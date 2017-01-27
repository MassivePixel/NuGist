using NuGist.Nuget;
using System;
using System.Threading.Tasks;

namespace NuGist.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            DoMain();
            System.Console.ReadKey();
        }

        static async Task DoMain()
        {
            //System.Console.WriteLine(PackService.CreateNuspec());
            var commands = new Commands();
            try
            {
                await commands.Push("testcnamespace.namespace.test.0.0.1.nupkg", new ConsoleLogger());
                System.Console.WriteLine("! Done !");
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(ex.ToString());
            }
        }
    }
}
