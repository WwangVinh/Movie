using Microsoft.OpenApi.Models;
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
        var requiredFields = new HashSet<string>();

        // Common file parameters
        if (fileParams.Any(p => p.Name == "AvatarUrlFile"))
        {
            schemaProperties["AvatarUrlFile"] = new OpenApiSchema { Type = "string", Format = "binary" };
        }

        if (fileParams.Any(p => p.Name == "posterFile"))
        {
            schemaProperties["posterFile"] = new OpenApiSchema { Type = "string", Format = "binary" };
        }

        // Method-specific schema and required fields
        if (methodName == "AddSeries")
        {
            schemaProperties["Title"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Description"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Rating"] = new OpenApiSchema { Type = "number" };
            schemaProperties["DirectorId"] = new OpenApiSchema { Type = "integer" };
            schemaProperties["IsHot"] = new OpenApiSchema { Type = "boolean" };
            schemaProperties["YearReleased"] = new OpenApiSchema { Type = "number" };
            schemaProperties["CategoryIds"] = new OpenApiSchema { Type = "number" };
            schemaProperties["ActorIds"] = new OpenApiSchema { Type = "number" };
            schemaProperties["Season"] = new OpenApiSchema { Type = "integer" };
            schemaProperties["Nation"] = new OpenApiSchema { Type = "string" };

            requiredFields = new HashSet<string> { "Title", "posterFile", "AvatarUrlFile" };
        }
        else if (methodName == "AddMovie")
        {
            schemaProperties["Title"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Description"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Rating"] = new OpenApiSchema { Type = "number" };
            schemaProperties["DirectorId"] = new OpenApiSchema { Type = "integer" };
            schemaProperties["IsHot"] = new OpenApiSchema { Type = "boolean" };
            schemaProperties["YearReleased"] = new OpenApiSchema { Type = "number" };
            schemaProperties["CategoryIds"] = new OpenApiSchema { Type = "number" };
            schemaProperties["ActorIds"] = new OpenApiSchema { Type = "number" };
            schemaProperties["LinkFilmUrl"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Nation"] = new OpenApiSchema { Type = "string" };

            requiredFields = new HashSet<string> { "Title", "posterFile", "AvatarUrlFile" };
        }

        else if (methodName == "UpdateMovie")
        {
            schemaProperties["MovieId"] = new OpenApiSchema { Type = "integer" };
            schemaProperties["Title"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Description"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Rating"] = new OpenApiSchema { Type = "number" };
            schemaProperties["DirectorId"] = new OpenApiSchema { Type = "integer" };
            schemaProperties["IsHot"] = new OpenApiSchema { Type = "boolean" };
            schemaProperties["YearReleased"] = new OpenApiSchema { Type = "number" };
            schemaProperties["CategoryIds"] = new OpenApiSchema { Type = "number" };
            schemaProperties["ActorIds"] = new OpenApiSchema { Type = "number" };
            schemaProperties["LinkFilmUrl"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Nation"] = new OpenApiSchema { Type = "string" };

            requiredFields = new HashSet<string> { "MovieId", "Title" }; // posterFile and AvatarUrlFile are optional
        }

        //else if (methodName == "AddEpisode")
        //{
        //    schemaProperties["SeriesId"] = new OpenApiSchema { Type = "number" };
        //    schemaProperties["EpisodeNumber"] = new OpenApiSchema { Type = "number" };
        //    schemaProperties["Title"] = new OpenApiSchema { Type = "string" };
        //    schemaProperties["LinkFilmUrl"] = new OpenApiSchema { Type = "string" };

        //    requiredFields = new HashSet<string> { "SeriesId", "EpisodeNumber", "LinkFilmUrl" };
        //}

        else if (methodName == "AddEpisode")
        {
            schemaProperties["SeriesId"] = new OpenApiSchema { Type = "number" };
            schemaProperties["EpisodeNumber"] = new OpenApiSchema { Type = "number" };
            schemaProperties["Title"] = new OpenApiSchema { Type = "string" };
            schemaProperties["LinkFilmUrl"] = new OpenApiSchema { Type = "string" };

            requiredFields = new HashSet<string> { "SeriesId", "EpisodeNumber", "LinkFilmUrl" };
        }

        else if (methodName == "AddActor")
        {
            schemaProperties["NameAct"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Description"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Nationality"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Professional"] = new OpenApiSchema { Type = "string" };

            requiredFields = new HashSet<string> { "NameAct" };
        }

        else if (methodName == "UpdateActor")
        {
            schemaProperties["NameAct"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Description"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Nationality"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Professional"] = new OpenApiSchema { Type = "string" };

            requiredFields = new HashSet<string> { "NameAct" };
        }


        else if (methodName == "AddDirector")
        {
            schemaProperties["NameDir"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Description"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Nationality"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Professional"] = new OpenApiSchema { Type = "string" };

            requiredFields = new HashSet<string> { "NameDir" };
        }

        else if (methodName == "UpdateDirector")
        {
            schemaProperties["NameDir"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Description"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Nationality"] = new OpenApiSchema { Type = "string" };
            schemaProperties["Professional"] = new OpenApiSchema { Type = "string" };
            schemaProperties["AvatarUrlFile"] = new OpenApiSchema { Type = "string", Format = "binary" };

            requiredFields = new HashSet<string> { "NameDir" }; // chỉ bắt buộc tên
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
                        Required = requiredFields
                    }
                }
            }
        };
    }
}