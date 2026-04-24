using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FCG.Api.Filters;

public class CorrelationIdHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters?.Add(new OpenApiParameter
            {
                Name = "x-correlation-id",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Id da correlação para Log",
                Schema = new OpenApiSchema
                {
                    Type = JsonSchemaType.String
                }
            });
    }
}
