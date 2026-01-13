using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MediatRAPI;

public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        ApiDescription apiDescription = context.ApiDescription;

        operation.Deprecated = apiDescription.IsDeprecated();

        foreach (ApiResponseType responseType in context.ApiDescription.SupportedResponseTypes)
        {
            string responseKey = responseType.IsDefaultResponse
                ? "default"
                : responseType.StatusCode.ToString();

            if (operation.Responses != null && operation.Responses.TryGetValue(responseKey, out IOpenApiResponse? response) && response != null)
            {
                foreach (string? contentType in response.Content?.Keys.ToList() ?? Enumerable.Empty<string>())
                {
                    if (responseType.ApiResponseFormats.All(x => x.MediaType != contentType))
                    {
                        response.Content?.Remove(contentType);
                    }
                }
            }
        }

        if (operation.Parameters == null)
        {
            return;
        }

        foreach (IOpenApiParameter parameter in operation.Parameters)
        {
            ApiParameterDescription? description = apiDescription.ParameterDescriptions
                .FirstOrDefault(p => p.Name == parameter.Name);

            if (description != null)
            {
                if (string.IsNullOrEmpty(parameter.Description))
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }
            }
        }
    }
}
