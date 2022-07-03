using Notes.Application.Interfaces;
using Notes.Persistence;
using System.Reflection;
using Notes.Application.Common.Mappings;
using Notes.Application;
using Notes.WebApi.Middleware;

const string corsPolicy = "_AlloyAll";

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

    app.UseCustomExceptionHandler();
    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseCors(corsPolicy);

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}