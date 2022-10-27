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

//ValidateAutoMapperConfiguration(app.Services);

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
    UseRestaurantEndpoints(endpointsBase);
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

    //get foods by restaurant id
    foodEndpoints.MapGet("restaurant/{id:guid}", (Guid id, IFoodFacade foodFacade) => foodFacade.GetByRestaurantId(id));

    //get by name
    foodEndpoints.MapGet("name/{regex}", (string regex, IFoodFacade foodFacade) => foodFacade.GetByName(regex));

    //get without alergens example - /api/food/restaurant/{someId}/noalergnes/{1_2_7_11}
    foodEndpoints.MapGet("restaurant/{id:guid}/noalergens/{alergensstr}", (Guid id, string alergensstr, IFoodFacade foodFacade) => foodFacade.GetWithoutAlergens(id, new HashSet<Alergens>(alergensstr.Split("_").Select(a => (Alergens)Enum.Parse(typeof(Alergens), a)))));

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
    
    //get orders by food id
    orderEndpoints.MapGet("food/{id:guid}", (Guid id, IOrderFacade orderFacade) => orderFacade.GetByFoodId(id));

    //get orders by restaurant id
    orderEndpoints.MapGet("restaurant/{id:guid}", (Guid id, IOrderFacade orderFacade) => orderFacade.GetByRestaurantId(id));

    //get orders by status and restaurant id
    orderEndpoints.MapGet("restaurant/{id:guid}/status/{status}", (Guid id, OrderStatus status,IOrderFacade orderFacade) => orderFacade.GetByStatus(id, status));

    orderEndpoints.MapPost("", (OrderDetailModel order, IOrderFacade orderFacade) => orderFacade.Create(order));
    orderEndpoints.MapPut("", (OrderDetailModel order, IOrderFacade orderFacade) => orderFacade.Update(order));
    orderEndpoints.MapPost("upsert", (OrderDetailModel order, IOrderFacade orderFacade) => orderFacade.CreateOrUpdate(order));
    orderEndpoints.MapDelete("{id:guid}", (Guid id, IOrderFacade orderFacade) => orderFacade.Delete(id));
}

void UseRestaurantEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var restaurantEndpoints = routeGroupBuilder.MapGroup("restaurant")
    .WithTags("restaurant");

    restaurantEndpoints.MapGet("", (IRestaurantFacade restarantFacade) => restarantFacade.GetAll());

    //get by id
    restaurantEndpoints.MapGet("{id:guid}", Results<Ok<RestaurantDetailModel>, NotFound<string>> (Guid id, IRestaurantFacade restaurantFacade, IStringLocalizer<RestaurantEndpointsResources> restaurantEndpointsLocalizer)
    => restaurantFacade.GetById(id) is { } restaurant
        ? TypedResults.Ok(restaurant)
        : TypedResults.NotFound(restaurantEndpointsLocalizer[nameof(RestaurantEndpointsResources.GetById_NotFound), id].Value));

    //get by name regex
    restaurantEndpoints.MapGet("name/{regex}", (string regex, IRestaurantFacade restaurantFacade) => restaurantFacade.GetByName(regex));
    //get by address regex
    restaurantEndpoints.MapGet("address/{regex}", (string regex, IRestaurantFacade restaurantFacade) => restaurantFacade.GetByAddress(regex));
    //get by food id
    restaurantEndpoints.MapGet("food/{id:guid}", (Guid id, IRestaurantFacade restaurantFacade) => restaurantFacade.GetByFoodId(id));


    //get earnings
    restaurantEndpoints.MapGet("earnings/{id:guid}", (Guid id, IRestaurantFacade restaurantFacade) => restaurantFacade.GetEarnings(id));
    /*
    restaurantEndpoints.MapGet("earnings/{id:guid}", Results<Ok<double>, NotFound<string>>, (Guid id, IRestaurantFacade restaurantFacade, IStringLocalizer<RestaurantEndpointsResources> restaurantEndpointsLocalizer)
        => restaurantFacade.GetEarnings(id) is { } earnings
        ? TypedResults.Ok(earnings)
        : TypedResults.NotFound(restaurantEndpointsLocalizer[nameof(RestaurantEndpointsResources.GetById_NotFound), id].Value));
    */
    restaurantEndpoints.MapPost("", (RestaurantDetailModel restaurant, IRestaurantFacade restaurantFacade) => restaurantFacade.Create(restaurant));
    restaurantEndpoints.MapPut("", (RestaurantDetailModel restaurant, IRestaurantFacade restaurantFacade) => restaurantFacade.Update(restaurant));
    restaurantEndpoints.MapPost("upsert", (RestaurantDetailModel restaurant, IRestaurantFacade restaurantFacade) => restaurantFacade.CreateOrUpdate(restaurant));
    restaurantEndpoints.MapDelete("", (Guid id, IRestaurantFacade restaurantFacade) => restaurantFacade.Delete(id));

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
