using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAPI.Contracts;
using TestAPI.Controllers;
using TestAPI.Models;
using TestAPI.Repositories;

namespace TestAPI.Filters
{
    // check the object with ID in HTTP GET, PUT, DELETE exists?
    public class ValidateEntityExistsAttribute<T> : IAsyncActionFilter where T : class, IEntity
    {
        private readonly IRepositoryWrapper _repo;
        private readonly ILoggerManager _logger;
        public ValidateEntityExistsAttribute(IRepositoryWrapper repo, ILoggerManager logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int id=0;
            if (context.ActionArguments.ContainsKey("id"))
            {
                id = (int)context.ActionArguments["id"];
            }
            else
            {
                context.Result = new BadRequestObjectResult("Bad Id parameter");
                return;
            }
            IRepositoryBase<T> repo = _repo.GetRepo<T>();
            if (repo == null)
            {
                context.Result = new BadRequestObjectResult("Error");
                return;
            }
            var entity = await repo.FindByCondition(x => x.Id==id).FirstOrDefaultAsync();
            if (entity == null)
            {
                _logger.LogError(LogMessage.NotFound(typeof(T).Name, id));
                context.Result = new NotFoundResult();
                return;
            }
            else
            {
                context.HttpContext.Items.Add("entity", entity);
            }
            await next();
        }
    }
}
