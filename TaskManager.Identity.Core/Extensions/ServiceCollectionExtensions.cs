using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TaskManager.Identity.Abstractions;
using TaskManager.Identity.Core.Options;
using TaskManager.Identity.Core.Services;
using TaskManager.Identity.Core.Token;
using TaskManager.Identity.Persistence.EFCore;

namespace TaskManager.Identity.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityComponent(
        this IServiceCollection services,
        IConfiguration cfg,
        string? connectionString = null)
    {
        services.Configure<IdentityOptions>(cfg.GetSection("Identity"));
        services.AddSingleton(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<IdentityOptions>>().Value);

        var cs = connectionString ?? cfg.GetConnectionString("DefaultConnection");

        services.AddDbContext<IdentityDbContext>(o =>
            o.UseSqlServer(cs, b =>
            b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)));

        services.TryAddScoped<JwtTokenFactory>();
        services.TryAddScoped<IAuthService, AuthService>();

        return services;
    }
}
