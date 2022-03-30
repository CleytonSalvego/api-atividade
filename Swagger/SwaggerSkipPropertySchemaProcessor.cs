using NJsonSchema.Generation;
using System;
using System.Linq;
using System.Reflection;

namespace api.Swagger
{
    public class SwaggerSkipPropertySchemaProcessor
    {
        public void Process(SchemaProcessorContext context)
        {
            if (context.Schema?.Properties == null)
            {
                return;
            }

            var skipProperties = context.Type.GetProperties().Where(t => t.GetCustomAttribute<SwaggerIgnoreAttribute>() != null);

            foreach (var skipProperty in skipProperties)
            {
                var propertyToSkip = context.Schema.Properties.Keys.SingleOrDefault(x => string.Equals(x, skipProperty.Name, StringComparison.OrdinalIgnoreCase));

                if (propertyToSkip != null)
                {
                    context.Schema.Properties.Remove(propertyToSkip);
                }
            }
        }
    }
}
