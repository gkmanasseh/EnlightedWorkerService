using EnlightedWorkService;
using EnlightedWorkService.Data;
using EnlightedWorkService.Services;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        //services..AddDbContext<EnlightedDbContext>(options => options.UseMySql(configuration.GetConnectionString("DefaultConnection")));

        services.AddLogging();

        var optionsBuilder = new DbContextOptionsBuilder<EnlightedDbContext>();

        optionsBuilder.UseMySQL(configuration.GetConnectionString("DefaultConnection"));

        services.AddScoped<EnlightedDbContext>(db => new EnlightedDbContext(optionsBuilder.Options));
        
        services.AddHttpClient();
        services.AddSingleton<IFetchData, FetchData>();
        services.AddHostedService<Worker>();
    })
    .Build();

CreateDbIfNoneExist(host);
await host.RunAsync();


static void CreateDbIfNoneExist(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var service = scope.ServiceProvider;

        try
        {
            var context = service.GetRequiredService<EnlightedDbContext>();
            context.Database.EnsureCreated();
        }
        catch (Exception)
        {
            throw;
        }
    }
}