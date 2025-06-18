using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace BasicDotnetTemplate.MainProject.Core.Filters
{
    public class ValidationActionFilter : IAsyncActionFilter
    {
        private readonly string _requestNotWellFormedMessage = "Request is not well formed";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new { message = _requestNotWellFormedMessage, errors = context.ModelState });
                return; 
            }

            var requestBody = context.ActionArguments.Values.FirstOrDefault(arg => arg != null && !arg.GetType().IsPrimitive && !(arg is string));

            if (requestBody == null)
            {
                context.Result = new BadRequestObjectResult(new { message = _requestNotWellFormedMessage });
                return;
            }

            await next();
        }
    }
}