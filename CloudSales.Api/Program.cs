using CloudSales.Api.Middlewares;
using CloudSales.Api.Validators;
using CloudSales.Infrastructure.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CloudSales.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = builder.Configuration;

        // Add services to the container.
        builder.Services.AddInfrastructure(configuration);
        builder.Services.AddApplication(configuration);

        builder.Services.TryAddSingleton<ErrorFactory>();

        builder.Services.AddControllers();

        builder.Services
            .AddValidatorsFromAssemblyContaining<CreateSubscriptionRequestModelValidator>()
            .AddFluentValidationAutoValidation();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            options.RoutePrefix = string.Empty; // To serve the Swagger UI at the root URL (optional)
        });

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
