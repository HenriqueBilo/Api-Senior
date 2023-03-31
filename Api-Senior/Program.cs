using Api_Senior;
using Api_Senior.Data;
using Api_Senior.Interfaces;
using Api_Senior.Jobs;
using Api_Senior.Services;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

Configuration.Cidades = app.Configuration.GetSection("Cidades").Get<List<string>>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<OpenWeatherScheduler>("fetch-temperatures", job => job.FetchTemperatures(), "*/2 * * * *");


app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddHttpClient();
    builder.Services.AddLogging(logging =>
    {
        logging.AddConsole();
    });

    builder.Services.AddControllers();

    builder.Services.AddScoped<ICidadeDataContext, CidadeDataContext>();
    builder.Services.AddDbContext<CidadeDataContext>(options => options.UseInMemoryDatabase(databaseName: "CidadesDatabase"));

    builder.Services.AddHangfire(config =>
    {
        config.UseRecommendedSerializerSettings();
        config.UseMemoryStorage();
    });

    builder.Services.AddScoped<OpenWeatherService>();
    builder.Services.AddScoped<List<string>>();
    builder.Services.AddScoped<OpenWeatherScheduler>();

    GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
    {
        Attempts = 3,
        DelaysInSeconds = new int[] { 120 }
    });

    builder.Services.AddHangfireServer();
    builder.Services.AddEndpointsApiExplorer();
    
}