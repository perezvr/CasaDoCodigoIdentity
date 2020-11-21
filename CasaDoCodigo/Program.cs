using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CasaDoCodigo
{
    //MELHORIA: 7) Projeto atualizado para ASP.NET Core 2.2
    //Veja propriedades do projeto
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost webHost = BuildWebHost(args);
            webHost.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
