using Notes.Application.Interfaces;
using Notes.Persistence;
using System.Reflection;
using Notes.Application.Common.Mappings;
using Notes.Application;
using Notes.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using Notes.WebApi;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

const string corsPolicy = "_AlloyAll";
const string _appUrl = "https://localhost:7207";


var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var app = builder.Build();

Configure(app);


//app.MapGet("/", () => "Hello World!");

app.Run();


void ConfigureServices(IServiceCollection services)
{
    services.AddAutoMapper(config =>
    {
        config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
        config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
    });

    services.AddApplication();
    services.AddPersistence(builder.Configuration);
    services.AddControllers();

    services.AddCors(option =>
    {
        option.AddPolicy(corsPolicy,
            policy =>
         {
             policy.AllowAnyHeader();
             policy.AllowAnyMethod();
             policy.AllowAnyOrigin();
         });
    });

    services.AddAuthentication(config =>
    {
        config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer("Bearer", options =>
    {
        options.Authority = _appUrl;
        options.Audience = "NotesWebApi";
        options.RequireHttpsMetadata = false;
    });

    services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
    services.AddTransient<IConfigureOptions<SwaggerGenOptions>,ConfigureSwaggerOptions>();

    services.AddSwaggerGen();
    services.AddApiVersioning();
}

void Configure(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        try
        {
            var context = serviceProvider.GetRequiredService<NotesDbContext>();
            DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {

        }
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseSwagger();
    app.UseSwaggerUI(config=>
    {
        var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
        foreach (var description in provider?.ApiVersionDescriptions)
        {
            config.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());

        }

        config.RoutePrefix = String.Empty;
        config.SwaggerEndpoint("swagger/v1/swagger.json", "Notes API");
    });
    app.UseCustomExceptionHandler();
    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseCors(corsPolicy);
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseApiVersioning();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}