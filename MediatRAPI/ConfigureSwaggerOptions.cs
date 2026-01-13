using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MediatRAPI;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static Microsoft.OpenApi.OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        OpenApiInfo info = new()
        {
            Title = "CustomerService.MediatR API",
            Version = description.ApiVersion.ToString(),
            Description = "API for managing customer data using MediatR pattern"
        };

        if (description.IsDeprecated)
        {
            info.Description += " (This API version has been deprecated)";
        }

        return info;
    }
}
