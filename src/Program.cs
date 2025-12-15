using EXAMPLE.SOURCE.API;
using EXAMPLE.SOURCE.API.Data.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<MockDataService>();

// Configure swagger
builder.Services.AddSwaggerGen(options =>
{
    options.DocInclusionPredicate((name, api) => true);
    options.OrderActionsBy(api => api.GroupName ?? api.ActionDescriptor.RouteValues["controller"]);

    // Custom filtering for API endpoints within the swaggerUI
    options.DocumentFilter<CleanTagNamesFilter>();
    options.TagActionsBy(api =>
    {
        return new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] };
    });

    //Removes the specified schema(s) from the swaggerUI
    options.DocumentFilter<RemoveSchemasDocumentFilter>();

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        // Basic information about the API and who to contact.
        Version = "1.0",
        Title = "Example Source API",
        Description = "This example API specifies the minimal requirements for developing a new API that will be used as an HR source system for HelloID provisioning."
    });

    // The XML is where all code comments are stored and is used to display information in the swagger interface and yaml.
    // Make sure to enable XML documentation file in project settings.
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
