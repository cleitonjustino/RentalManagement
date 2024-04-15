using FluentValidation.AspNetCore;
using MassTransit;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Minio;
using MongoFramework;
using RentalManagement.Application.CommandStack.Consumers;
using RentalManagement.Application.CommandStack.DeliveryMan.AddDeliveryMan;
using RentalManagement.Application.CommandStack.Motorcyle;
using RentalManagement.Application.QueryStack.Helpers;
using RentalManagement.Application.QueryStack.Motorcycle;
using RentalManagement.Application.QueryStack.Plans;
using RentalManagement.Domain.Notification;
using RentalManagement.Domain.Request;
using RentalManagement.Infrastructure;
using RentalManagement.Infrastructure.ExternalServices.Storage;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var infoSdk = RuntimeInformation.FrameworkDescription;
builder.Services.AddSwaggerGen(c =>
{
    c.UseInlineDefinitionsForEnums();
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
    c.DescribeAllParametersInCamelCase();
    var xmlFilename = "RentalManagement.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
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
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
    cfg.Lifetime = ServiceLifetime.Scoped;
});

builder.Services.AddScoped(typeof(IRequestHandler<MotorcycleRequest, MotorcycleResponse>), typeof(MotorcycleRequestHandler));
builder.Services.AddScoped(typeof(IRequestHandler<MotorcycleUpdateRequest, MotorcycleUpdateResponse>), typeof(MotorcycleUpdateRequestHandler));
builder.Services.AddScoped(typeof(IRequestHandler<MotorcycleRemoveRequest, MotorcycleRemoveResponse>), typeof(MotorcycleRemoveRequestHandler));
builder.Services.AddScoped(typeof(IRequestHandler<RentMotorcycleRequest, RentMotorcycleResponse>), typeof(RentMotorcycleRequestHandler));
builder.Services.AddScoped(typeof(IRequestHandler<ReturnRentMotorcycleRequest, ReturnRentMotorcycleResponse>), typeof(ReturnRentMotorcycleRequestHandler));

builder.Services.AddScoped(typeof(IRequestHandler<DeliveryManAddRequest, DeliveryManAddResponse>), typeof(DeliveryManAddRequestHandler));
builder.Services.AddScoped(typeof(IRequestHandler<DeliveryManAddCnhRequest, DeliveryManAddCnhResponse>), typeof(DeliveryManAddCnhRequestHandler));

builder.Services.AddScoped(typeof(IRequestHandler<MotorcycleQuery, PagedList<MotorcycleReadModel>>), typeof(MotorcycleQueryHandler));
builder.Services.AddScoped(typeof(IRequestHandler<DeliveryManQuery, PagedList<DeliveryManReadModel>>), typeof(DeliveryManQueryHandler));
builder.Services.AddScoped(typeof(IRequestHandler<PlansQuery, List<string>>), typeof(PlansQueryHandler));
builder.Services.AddScoped(typeof(IRequestPreProcessor<MotorcycleQuery>), typeof(MotorcycleQueryCustomFilter));
builder.Services.AddScoped(typeof(IRequestPreProcessor<DeliveryManQuery>), typeof(DeliveryManQueryCustomFilter));
builder.Services.AddScoped(typeof(IRequestPreProcessor<RentMotoQuery>), typeof(RentMotoQueryCustomFilter));
builder.Services.AddScoped(typeof(IRequestHandler<RentMotoQuery, List<RentMotoReadModel>>), typeof(RentMotoQueryHandler));

builder.Services.AddScoped<IStorageService, StorageService>();


builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationRequestBehavior<,>));
builder.Services.AddScoped<IDomainNotificationContext, DomainNotificationContext>();

//MongoDB
builder.Services.AddTransient<IMongoDbConnection>(s => MongoDbConnection.FromUrl(new MongoDB.Driver.MongoUrl("mongodb://cleiton:479114@localhost:27017/local?authSource=admin")));
builder.Services.AddTransient<RentalDbContext>();

//Fluent Validator
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RentMotorcycleRequestValidator>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<DeliveryManAddRequestValidator>());

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DataBase"));

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Add Minio using the default endpoint
builder.Services.AddMinio(builder.Configuration["STORAGE_ACCESS_KEY"], builder.Configuration["STORAGE_SECRET_KEY"]);

// Add Minio using the custom endpoint and configure additional settings for default MinioClient initialization
builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(new Uri("http://localhost:9000"))
    .WithCredentials(builder.Configuration["STORAGE_ACCESS_KEY"], builder.Configuration["STORAGE_SECRET_KEY"]));

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
