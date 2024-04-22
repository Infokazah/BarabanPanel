using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserStatisticDb;
using UserStatisticDb.Context;

namespace BarabanPanel.Data.StatisticDb
{
    static class DbRegistrator
    {
        public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration Configuration) => services
            .AddDbContext<UserStatisticContext>(opt =>
            {
                var type = Configuration["Type"];
                opt.UseSqlServer(Configuration.GetConnectionString(type));
            })
        .AddTransient<DbInitializer>()
        .AddRepositoriesInDb();
    }
}
