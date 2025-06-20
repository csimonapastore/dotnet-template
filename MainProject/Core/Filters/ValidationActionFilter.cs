using BasicDotnetTemplate.MainProject.Models.Api.Base;
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
                context.Result = new BadRequestObjectResult(new ValidationError
                {
                    Message = _requestNotWellFormedMessage,
                    Errors = context.ModelState.Where(m =>
                        m.Value != null && m.Value.Errors.Any())
                        .ToDictionary(
                            m => m.Key,
                            m => m.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                        )
                });
                return;
            }

            var requestBody = context.ActionArguments.Values.FirstOrDefault(arg => arg != null && !arg.GetType().IsPrimitive && arg is not string);

            if (requestBody == null)
            {
                context.Result = new BadRequestObjectResult(new ValidationError
                {
                    Message = _requestNotWellFormedMessage
                });
                return;
            }

            await next();
        }
    }
}
