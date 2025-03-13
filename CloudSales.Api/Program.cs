using CloudSales.Api.Validators;
using CloudSales.Infrastructure.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;

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

        builder.Services.AddControllers();

        builder.Services
            .AddValidatorsFromAssemblyContaining<CreateSubscriptionRequestModelValidator>()
            .AddFluentValidationAutoValidation();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
