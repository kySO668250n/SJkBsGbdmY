// 代码生成时间: 2025-10-29 01:05:06
 * commenting, and maintainability.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiDocumentationGenerator
{
    // Configuration class to configure the Swagger generator
    public class SwaggerConfig
    {
        public static void ConfigureSwaggerGen(SwaggerGenOptions options)
        {
            // Set the comments path for the Swagger JSON and UI.
            var xmlPath = System.Web.Hosting.HostingEnvironment.MapPath($"~/{typeof(ApiDocumentationGenerator).Assembly.GetName().Name}.xml");
            options.IncludeXmlComments(xmlPath);

            // Define the Swagger documentation for each API version
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        }
    }

    // Custom schema filter to document API models
    public class CustomSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // Add model descriptions from Data Annotations or directly from the model properties
            var modelProperties = context.Type.GetProperties().ToList();
            foreach (var property in modelProperties)
            {
                var descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute != null)
                {
                    schema.Properties[property.Name].Description = descriptionAttribute.Description;
                }
            }
        }
    }

    // Main class for the API documentation generator
    public class ApiDocumentationGenerator
    {
        private readonly DbContext _context;

        public ApiDocumentationGenerator(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Method to generate API documentation
        public string GenerateDocumentation()
        {
            try
            {
                var schemaRepository = new SchemaRepository(_context);
                var document = new OpenApiDocumentGenerator(schemaRepository).GenerateDocument();
                return document.ToJson();
            }
            catch (Exception ex)
            {
                // Log the exception and return an error message
                Console.WriteLine($"Error generating API documentation: {ex.Message}");
                return "Error: Unable to generate API documentation.";
            }
        }
    }
}
