using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace FCG.Api.Filters
{
    public class CorrelationIdHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
           if(operation.Parameters != null)
           {
                operation.Parameters.Add(new OpenApiParameter
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
    }
}
