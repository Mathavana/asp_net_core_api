﻿using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Supermarket.Swagger
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        /// <summary>  
        /// Applies the filter to the specified operation using the given context.  
        /// </summary>  
        /// <param name="operation">The operation to apply the filter to.</param>  
        /// <param name="context">The current operation filter context.</param>  
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                return;
            }
            foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>())
            {
                var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                var routeInfo = description.RouteInfo;

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (routeInfo == null)
                {
                    continue;
                }

                if (parameter.Default == null)
                {
                    parameter.Default = routeInfo.DefaultValue;
                }

                parameter.Required |= !routeInfo.IsOptional;
            }
        }
    }
}