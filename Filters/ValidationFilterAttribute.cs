using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAPI.Contracts;
using TestAPI.Extensions;

namespace TestAPI.Filters
{
    // check if object in HTTP PUT and POST is and check the mapping modelview object to model object is valid
    public class ValidationFilterAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;

        public ValidationFilterAttribute(ILoggerManager logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {            
            var param = context.ActionArguments.SingleOrDefault(pr => pr.Value is IEntity);
            if (param.Value == null)
            {
                context.Result = new BadRequestObjectResult("Object is null");
                return;
            }

            if (!context.ModelState.IsValid)
            {
                _logger.LogError("Model State Invalid\n" + string.Join('\n',context.ModelState.GetErrorMessages()));
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }
            var result = await next();
        }
    }
}
