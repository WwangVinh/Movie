using Microsoft.OpenApi.Models;
using Movie.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class FileUploadOperation : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParams = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile));

        if (!fileParams.Any()) return;

        var methodName = context.MethodInfo.Name;

        var schemaProperties = new Dictionary<string, OpenApiSchema>();

        schemaProperties["posterFile"] = new OpenApiSchema { Type = "string", Format = "binary" };
        schemaProperties["AvatarUrlFile"] = new OpenApiSchema { Type = "string", Format = "binary" };
        schemaProperties["Title"] = new OpenApiSchema { Type = "string" };
        schemaProperties["Description"] = new OpenApiSchema { Type = "string" };
        schemaProperties["Rating"] = new OpenApiSchema { Type = "number" };
        schemaProperties["DirectorId"] = new OpenApiSchema { Type = "integer" };
        schemaProperties["IsHot"] = new OpenApiSchema { Type = "boolean" };
        schemaProperties["YearReleased"] = new OpenApiSchema { Type = "number" };
        schemaProperties["CategoryIds"] = new OpenApiSchema { Type = "number" };
        schemaProperties["ActorIds"] = new OpenApiSchema { Type = "number" };

        if (methodName == "AddSeries")
        {
            schemaProperties["LinkFilmUrl"] = new OpenApiSchema { Type = "string" };
            schemaProperties["EpisodeNumber"] = new OpenApiSchema { Type = "string" };
            schemaProperties["EpisodesTitle"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Season"] = new OpenApiSchema { Type = "integer" };
            schemaProperties["Nation"] = new OpenApiSchema { Type = "string" };
        }
        else if (methodName == "AddMovie")
        {
            schemaProperties["LinkFilmUrl"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Nation"] = new OpenApiSchema { Type = "string" };
        }

        operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = schemaProperties,
                        Required = new HashSet<string> { "Title", "posterFile", "AvatarUrlFile" }
                    }
                }
            }
        };
    }
}