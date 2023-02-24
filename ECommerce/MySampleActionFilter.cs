using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce
{
    public class MySampleActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("OnActionExecuted");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            Console.WriteLine("OnActionExecuting");
        }
    }
}
