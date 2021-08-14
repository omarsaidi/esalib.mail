using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace EsaLib.Mail.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddLibMailServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ITokenizer, Tokenizer>();
            return services;
        }
    }
}
