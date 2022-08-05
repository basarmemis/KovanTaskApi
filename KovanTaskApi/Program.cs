using Core.IRepositories;
using Core.Repositories;
using Data;
using GGraphQL;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.AuthenticationModel;
using Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(x => true);
                          //policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000", "https://localhost:3001", "http://localhost:7262", "https://localhost:7262").AllowCredentials();
                      });
});

builder.Services.AddIdentityCore<User>(options =>
{
    options.User.RequireUniqueEmail = true;
})
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration["JWTSettings:TokenKey"])),
            ClockSkew = TimeSpan.Zero
        };


        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                string accessToken = context.Request.Headers["Authorization"];
                if (accessToken != null)
                {
                    accessToken = accessToken.Replace("Bearer ", "");
                    accessToken = accessToken.Replace("\"", "");
                }


                //If the request is for Hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/api")))
                {
                    context.Token = accessToken;
                    //context.Token = context.Token.Replace("Bearer ", "");
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options
    .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("KovanTaskApi"));
}
);



builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddFiltering()
    .AddSorting();



builder.Services.AddScoped<TokenService>();



builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<ClientService>();

builder.Services.AddScoped<IKovanModelRepository, KovanModelRepository>();

builder.Services.AddScoped<IKovanBikeDetailModelRepository, KovanBikeDetailModelRepository>();

var app = builder.Build();

#region Database Initializer
var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
await dbContext.Database.MigrateAsync();
await DbInitializer.Initialize(dbContext, roleManager, userManager);
#endregion Database Initializer

app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UsePlayground(new PlaygroundOptions
    {
        QueryPath = "/api",
        Path = "/playground"
    });
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGraphQL("/api");
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");



app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}