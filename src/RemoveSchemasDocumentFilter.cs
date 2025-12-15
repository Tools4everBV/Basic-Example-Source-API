using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EXAMPLE.SOURCE.API
{
    public class RemoveSchemasDocumentFilter : IDocumentFilter
    {
        private readonly HashSet<string> _namesToRemove = new()
        {
            "Employer"
        };

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var name in _namesToRemove.ToList())
            {
                if (swaggerDoc.Components?.Schemas?.ContainsKey(name) == true)
                {
                    swaggerDoc.Components.Schemas.Remove(name);
                }
            }
        }
    }
}
