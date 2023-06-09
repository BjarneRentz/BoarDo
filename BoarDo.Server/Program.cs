using BoarDo.Server.Configs;
using BoarDo.Server.Database;
using BoarDo.Server.Repos;
using BoarDo.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<GoogleClientConfig>(builder.Configuration.GetSection(GoogleClientConfig.POSITION));
builder.Services.AddDbContext<BoarDoContext>();

builder.Services.AddScoped<IAuthClientsRepo, AuthClientRepo>();


builder.Services.AddSingleton<GoogleCalendarService>();
builder.Services.AddSingleton<IRenderService, RenderService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddHostedService<EInkDisplay>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();