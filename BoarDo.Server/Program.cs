using System.Text.Json.Serialization;
using BoarDo.Server.Configs;
using BoarDo.Server.Database;
using BoarDo.Server.Jobs;
using BoarDo.Server.Repos;
using BoarDo.Server.Services;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<GoogleClientConfig>(builder.Configuration.GetSection(GoogleClientConfig.POSITION));
builder.Services.Configure<SyncSettings>(builder.Configuration.GetSection(SyncSettings.POSITION));
builder.Services.AddDbContext<BoarDoContext>();

builder.Services.AddScoped<IAuthClientsRepo, AuthClientRepo>();


builder.Services.AddScoped<GoogleCalendarService>();
builder.Services.AddSingleton<IRenderService, RenderService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
    });
}




builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
});


builder.Services.AddQuartzHostedService(c => c.WaitForJobsToComplete = true);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddHostedService<EInkDisplay>();
var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<BoarDoContext>();
dbContext.Database.Migrate();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();

}

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();