using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using ODataAlternateKeySample.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<IAlternateKeyRepository, AlternateKeyRepositoryInMemory>();

// source code reference

// ODataOutputFormatter.cs
// ODataOptions.cs
// HttpRequestExtensions.cs

// global scope service
// NOTE: It seems that modify global scope service didn't affect the odata?
// Please look at the source code in the source code at ODataOptions.cs
// builder.Services.AddSingleton<IODataSerializerProvider, IngoreNullEntityPropertiesSerializerProvider>();

builder.Services.AddControllers()
    .AddOData((opt, d) =>
        {
            // IServiceCollection services = new ServiceCollection();
            var odataOptions = opt.EnableQueryFeatures()
                .AddRouteComponents("odata", EdmModelBuilder.GetEdmModel(), (services) =>
                {
                    // this service is for odata scope only ... 
                    // by looking at the source code at ODataOptions.cs
                    services.AddSingleton<IODataSerializerProvider, IngoreNullEntityPropertiesSerializerProvider>();
                });
            // debug
            // var routeServiceProvider = odataOptions.GetRouteServices("odata");
            //var serializerProvider = routeServiceProvider.GetRequiredService<IODataSerializerProvider>();
        });

builder.Services.AddTransient<IAlternateKeyRepository, AlternateKeyRepositoryInMemory>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseODataRouteDebug();

app.UseAuthorization();

app.MapControllers();

app.Run();
