using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using NSwag;
using NSwag.AspNetCore;
using Truhome.Api.Extensions;
using Truhome.Api.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60);
});

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddResponseCompression();

builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration["DbConnectionString"]!,
        name: "postgresql",
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
        tags: new[] { "db", "sql", "postgresql" });

builder.Services.AddOpenApi();

var tempProvider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

foreach (var description in tempProvider.ApiVersionDescriptions)
{
    builder.Services.AddOpenApiDocument(settings =>
    {
        settings.DocumentName = description.GroupName;
        settings.ApiGroupNames = new[] { description.GroupName };

        settings.OperationProcessors.Add(new NSwag.Generation.Processors.OperationProcessor(opc =>
        {
            opc.OperationDescription.Operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-api-key",
                Kind = OpenApiParameterKind.Header,
                Type = NJsonSchema.JsonObjectType.String,
                IsRequired = false,
                Description = "API key used for authenticating requests. Required for accessing protected endpoints such as the Deduplication API."
            });

            return true;
        }));

        settings.OperationProcessors.Add(new NSwag.Generation.Processors.OperationProcessor(opc =>
        {
            opc.OperationDescription.Operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-origin-system",
                Kind = OpenApiParameterKind.Header,
                Type = NJsonSchema.JsonObjectType.String,
                IsRequired = false,
                Description = "Identifier for the source system calling the API."
            });

            return true;
        }));

        settings.PostProcess = doc =>
        {
            doc.Info = new OpenApiInfo
            {
                Title = $"Truhome API {description.ApiVersion}",
                Version = description.GroupName,
                Description = $"Truhome API documentation for version {description.ApiVersion}"
            };
        };
    });
}

builder.Services.AddServices(builder.Configuration["DbConnectionString"]!);

WebApplication app = builder.Build();

app.MapOpenApi();

app.UseOpenApi();

var versionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwaggerUi(options =>
{
    foreach (var description in versionProvider.ApiVersionDescriptions)
    {
        options.SwaggerRoutes.Add(new SwaggerUiRoute(name: description.GroupName, url: $"/swagger/{description.GroupName}/swagger.json"));
    }
});

foreach (var description in versionProvider.ApiVersionDescriptions)
{
    app.UseReDoc(options =>
    {
        options.Path = $"/redoc/{description.GroupName}";
        options.DocumentPath = $"/swagger/{description.GroupName}/swagger.json";
    });
}

#if !DEBUG
    app.UseHsts();
    app.UseHttpsRedirection();
#endif

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseResponseCompression();

# if DEBUG
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
#endif

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

app.MapGet("/", () => "API is operational!");

app.MapControllers();

app.Run();