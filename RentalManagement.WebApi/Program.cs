using MassTransit;
using MediatR;
using Microsoft.OpenApi.Models;
using RentalManagement.Application.CommandStack.Consumers;
using RentalManagement.Application.CommandStack.Motorcyle;
using RentalManagement.Domain.Request;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var infoSdk = RuntimeInformation.FrameworkDescription;
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = $"RentalManegement {infoSdk}",
            Description = $"API of manage motorbike rentals and delivery drivers ({infoSdk})",
            Version = "v1",
            Contact = new OpenApiContact()
            {
                Name = "Cleiton Justino",
                Url = new Uri("https://github.com/cleitonjustino/RentalManagement.git"),
            },
            License = new OpenApiLicense()
            {
                Name = "MIT",
                Url = new Uri("http://opensource.org/licenses/MIT"),
            }
        });
});

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<MotorcycleConsumer>();

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", 5672, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("motorcycle-add", ep =>
        {
            ep.ConfigureConsumer<MotorcycleConsumer>(context);
        });
    });


    //config.UsingInMemory((context, cfg) =>
    //{
    //    cfg.ConfigureEndpoints(context);
    //});
});

builder.Services.AddTransient(typeof(IRequestHandler<MotorcycleRequest, MotorcycleResponse>), typeof(MotorcycleRequestHandler));


builder.Services.AddSwaggerGen();

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
