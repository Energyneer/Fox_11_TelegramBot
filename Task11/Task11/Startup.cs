using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace Task11
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TBot.ITelegramService, TBot.TelegramService>();
            services.AddSingleton<Api.IRequestService, Api.RequestService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Run(async (context) =>
            {
                Thread thread = new Thread(new ThreadStart(app.ApplicationServices.GetRequiredService<TBot.ITelegramService>().Start));
                thread.Start();
            });
        }
    }
}
