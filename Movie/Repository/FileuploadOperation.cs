using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class FileUploadOperation : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParams = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile));

        if (fileParams.Any())
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["posterFile"] = new OpenApiSchema { Type = "string", Format = "binary" },
                                ["AvatarUrlFile"] = new OpenApiSchema { Type = "string", Format = "binary" },
                                ["Title"] = new OpenApiSchema { Type = "string" },
                                ["Description"] = new OpenApiSchema { Type = "string" },
                                ["Rating"] = new OpenApiSchema { Type = "number" },
                                ["LinkFilmUrl"] = new OpenApiSchema { Type = "string" },
                                ["DirectorId"] = new OpenApiSchema { Type = "integer" },
                                ["IsHot"] = new OpenApiSchema { Type = "boolean" },
                                ["YearReleased"] = new OpenApiSchema { Type = "integer" },
                                ["categoryIds"] = new OpenApiSchema { Type = "integer" },
                                ["actorIds"] = new OpenApiSchema { Type = "integer" },

                            }
                        }
                    }
                }
            };
        }
    }
}