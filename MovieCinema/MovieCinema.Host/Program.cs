using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using MovieCinema.Host.Extensions;
using MovieCinema.Host.HealthChecks;
using MovieCinema.Host.Middleware;
using MovieCinema.Models.Configurations;
using MovieStore.BL.CommandHandler.DirectorCommandHandlers;
using MovieStore.BL.KafkaProducerConsumer.KafkaConfiguration;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace MovieCinema.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<MongoDbConfiguration>(builder.Configuration.GetSection(nameof(MongoDbConfiguration)));
            builder.Services.Configure<KafkaConsumerSettings>(builder.Configuration.GetSection(nameof(KafkaConsumerSettings)));
            builder.Services.Configure<KafkaProducerSettings>(builder.Configuration.GetSection(nameof(KafkaProducerSettings)));

            // Add services to the container.
            builder.Services.RegisterRepositories()
                .RegisterHostedServices()
                .AddAutoMapper(typeof(Program));

            builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Health checks
            builder.Services.AddHealthChecks()
                .AddCheck<SqlHealthCheck>("SQL Server")
                .AddCheck<MongoHealthCheck>("MongoDB Health Check");

            builder.Services.AddMediatR(typeof(GetAllDirectorsCommandHandler).Assembly);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.RegisterHealthChecks();
            app.MapHealthChecks("/healthz");

            app.MapControllers();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.Run();
        }
    }
}