using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using AutoMapper.Internal;
using DameChales.API.BL.Facades;
using DameChales.API.BL.Installers;
using DameChales.API.DAL.EF.Extensions;
using DameChales.Common.Extensions;
using DameChales.Common.Models;
using DameChales.Common.Resources;
using DameChales.API.App.Extensions;
using DameChales.API.DAL.Common.Entities;
using DameChales.API.DAL.Common;
using DameChales.API.DAL.EF.Installers;
using DameChales.API.DAL.Memory.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using DameChales.Common.Enums;

var builder = WebApplication.CreateBuilder();

ConfigureCors(builder.Services);
ConfigureLocalization(builder.Services);

ConfigureOpenApiDocuments(builder.Services);
ConfigureDependencies(builder.Services, builder.Configuration);
ConfigureAutoMapper(builder.Services);

var app = builder.Build();

ValidateAutoMapperConfiguration(app.Services);

UseDevelopmentSettings(app);
UseSecurityFeatures(app);
UseLocalization(app);
UseRouting(app);
UseEndpoints(app);
UseOpenApi(app);

app.Run();

void ConfigureCors(IServiceCollection serviceCollection)
{
    serviceCollection.AddCors(options =>
    {
        options.AddDefaultPolicy(o =>
            o.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
    });
}

void ConfigureLocalization(IServiceCollection serviceCollection)
{
    serviceCollection.AddLocalization(options => options.ResourcesPath = string.Empty);
}

void ConfigureOpenApiDocuments(IServiceCollection serviceCollection)
{
    serviceCollection.AddEndpointsApiExplorer();
    serviceCollection.AddOpenApiDocument();
}

void ConfigureDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
{
    if (!Enum.TryParse<DALType>(configuration.GetSection("DALSelectionOptions")["Type"], out var dalType))
    {
        throw new ArgumentException("DALSelectionOptions:Type");
    }

    switch (dalType)
    {
        case DALType.Memory:
            serviceCollection.AddInstaller<ApiDALMemoryInstaller>();
            break;
        case DALType.EntityFramework:
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentException("The connection string is missing");
            serviceCollection.AddInstaller<ApiDALEFInstaller>(connectionString);
            break;
    }

    serviceCollection.AddInstaller<ApiBLInstaller>();
}

void ConfigureAutoMapper(IServiceCollection serviceCollection)
{
    serviceCollection.AddAutoMapper(configuration =>
    {
        // This is a temporary fix - should be able to remove this when version 11.0.2 comes out
        // More information here: https://github.com/AutoMapper/AutoMapper/issues/3988
        configuration.Internal().MethodMappingEnabled = false;
    }, typeof(EntityBase), typeof(ApiBLInstaller));
}

void ValidateAutoMapperConfiguration(IServiceProvider serviceProvider)
{
    var mapper = serviceProvider.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
}

void UseEndpoints(WebApplication application)
{
    var endpointsBase = application.MapGroup("api")
        .WithOpenApi();

    UseFoodEndpoints(endpointsBase);
    UseOrderEndpoints(endpointsBase);
}

void UseFoodEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var foodEndpoints = routeGroupBuilder.MapGroup("food")
        .WithTags("food");

    foodEndpoints.MapGet("", (IFoodFacade foodFacade) => foodFacade.GetAll());

    foodEndpoints.MapGet("{id:guid}", Results<Ok<FoodDetailModel>, NotFound<string>> (Guid id, IFoodFacade foodFacade, IStringLocalizer<FoodEndpointsResources> foodEndpointsLocalizer)
        => foodFacade.GetById(id) is { } food
            ? TypedResults.Ok(food)
            : TypedResults.NotFound(foodEndpointsLocalizer[nameof(FoodEndpointsResources.GetById_NotFound), id].Value));

    //get by restaurant id
    foodEndpoints.MapGet("restaurant/{id:guid}", Results<Ok<FoodDetailModel>, NotFound<string>> (Guid id, IFoodFacade foodFacade, IStringLocalizer<FoodEndpointsResources> foodEndpointsLocalizer)
        => foodFacade.GetByRestaurantId(id) is { } food
            ? TypedResults.Ok(food)
            : TypedResults.NotFound(foodEndpointsLocalizer[nameof(FoodEndpointsResources.GetByRestaurantId_NotFound), id].Value));

    //get by name
    foodEndpoints.MapGet("{name:string}", Results<Ok<FoodDetailModel>, NotFound<string>> (string name, IFoodFacade foodFacade, IStringLocalizer<FoodEndpointsResources> foodEndpointsLocalizer)
        => foodFacade.GetByName(name) is { } food
            ? TypedResults.Ok(food)
            : TypedResults.NotFound(foodEndpointsLocalizer[nameof(FoodEndpointsResources.GetByName_NotFound), name].Value));

    //get without alergens example - /api/food/restaurant/{someId}/noalergnes/{1_2_7_11}
    foodEndpoints.MapGet("restaurant/{id:guid}/noalergens/{alergensstr}", Results<Ok<FoodDetailModel>, NotFound<string>> (Guid id, string alergensstr, IFoodFacade foodFacade, IStringLocalizer<FoodEndpointsResources> foodEndpointsLocalizer)
        => foodFacade.GetWithoutAlergens(id, new HashSet<Alergens>(alergensstr.Split("_").Select(a => (Alergens)Enum.Parse(typeof(Alergens), a)))) is { } food
            ? TypedResults.Ok(food)
            : TypedResults.NotFound(foodEndpointsLocalizer[nameof(FoodEndpointsResources.GetWithoutAlergens_NotFound), id, alergensstr].Value));


    foodEndpoints.MapPost("", (FoodDetailModel food, IFoodFacade foodFacade) => foodFacade.Create(food));
    foodEndpoints.MapPut("", (FoodDetailModel food, IFoodFacade foodFacade) => foodFacade.Update(food));
    foodEndpoints.MapPost("upsert", (FoodDetailModel food, IFoodFacade foodFacade) => foodFacade.CreateOrUpdate(food));
    foodEndpoints.MapDelete("{id:guid}", (Guid id, IFoodFacade foodFacade) => foodFacade.Delete(id));
    
}

void UseOrderEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var orderEndpoints = routeGroupBuilder.MapGroup("order")
        .WithTags("order");

    orderEndpoints.MapGet("", (IOrderFacade orderFacade) => orderFacade.GetAll());

    orderEndpoints.MapGet("{id:guid}", Results<Ok<OrderDetailModel>, NotFound<string>> (Guid id, IOrderFacade orderFacade, IStringLocalizer<OrderEndpointsResources> orderEndpointsLocalizer)
        => orderFacade.GetById(id) is { } order
            ? TypedResults.Ok(order)
            : TypedResults.NotFound(orderEndpointsLocalizer[nameof(OrderEndpointsResources.GetById_NotFound), id].Value));

    //get by food id
    orderEndpoints.MapGet("food/{id:guid}", Results<Ok<OrderDetailModel>, NotFound<string>> (Guid id, IOrderFacade orderFacade, IStringLocalizer<OrderEndpointsResources> orderEndpointsLocalizer)
        => orderFacade.GetByFoodId(id) is { } order
            ? TypedResults.Ok(order)
            : TypedResults.NotFound(orderEndpointsLocalizer[nameof(OrderEndpointsResources.GetByFoodId_NotFound), id].Value));


    //get by restaurant id
    orderEndpoints.MapGet("restaurant/{id:guid}", Results<Ok<OrderDetailModel>, NotFound<string>> (Guid id, IOrderFacade orderFacade, IStringLocalizer<OrderEndpointsResources> orderEndpointsLocalizer)
        => orderFacade.GetByRestaurantId(id) is { } order
            ? TypedResults.Ok(order)
            : TypedResults.NotFound(orderEndpointsLocalizer[nameof(OrderEndpointsResources.GetByRestaurantId_NotFound), id].Value));


    //get by status
    orderEndpoints.MapGet("restaurant/{id:guid}/status/{status}", Results<Ok<OrderDetailModel>, NotFound<string>> (Guid id, OrderStatus status,IOrderFacade orderFacade, IStringLocalizer<OrderEndpointsResources> orderEndpointsLocalizer)
        => orderFacade.GetByStatus(id, status) is { } order
            ? TypedResults.Ok(order)
            : TypedResults.NotFound(orderEndpointsLocalizer[nameof(OrderEndpointsResources.GetByStatus_NotFound), id, status].Value));

    orderEndpoints.MapPost("", (OrderDetailModel order, IOrderFacade orderFacade) => orderFacade.Create(order));
    orderEndpoints.MapPut("", (OrderDetailModel order, IOrderFacade orderFacade) => orderFacade.Update(order));
    orderEndpoints.MapPost("upsert", (OrderDetailModel order, IOrderFacade orderFacade) => orderFacade.CreateOrUpdate(order));
    orderEndpoints.MapDelete("{id:guid}", (Guid id, IOrderFacade orderFacade) => orderFacade.Delete(id));
}


void UseDevelopmentSettings(WebApplication application)
{
    var environment = application.Services.GetRequiredService<IWebHostEnvironment>();

    if (environment.IsDevelopment())
    {
        application.UseDeveloperExceptionPage();
    }
}

void UseSecurityFeatures(IApplicationBuilder application)
{
    application.UseCors();
    application.UseHttpsRedirection();
}

void UseLocalization(IApplicationBuilder application)
{
    application.UseRequestLocalization(new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture(new CultureInfo("en")),
        SupportedCultures = new List<CultureInfo> { new("en"), new("cs") }
    });

    application.UseRequestCulture();
}

void UseRouting(IApplicationBuilder application)
{
    application.UseRouting();
}

void UseOpenApi(IApplicationBuilder application)
{
    application.UseOpenApi();
    application.UseSwaggerUi3();
}


// Make the implicit Program class public so test projects can access it
public partial class Program
{
}
