using NuGist.Services;

namespace NuGist.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(PackService.CreateNuspec());
        }
    }
}
