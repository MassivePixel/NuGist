using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace NuGist.Web.Filters
{
    public class NullArgumentsFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.Any(x => x.Value == null))
            {
                context.Result = new BadRequestObjectResult(new
                {
                    Model = new[]
                    {
                        "Model shouldn't be null"
                    }
                });
            }
        }
    }
}
