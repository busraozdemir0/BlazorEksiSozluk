using Microsoft.AspNetCore.Mvc.Filters;

namespace BlazorSozluk.Api.WebApi.Infrastructure.ActionFilters
{
    // Sistemimiz icerisinde bir validasyon hatasi olustugu zaman calisacak olan metodumuz bu
    public class ValidateModelStateFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var messages = context.ModelState.Values.SelectMany(x => x.Errors)
                                                        .Select(x => !string.IsNullOrEmpty(x.ErrorMessage) ?
                                                        x.ErrorMessage : x.Exception?.Message)
                                                        .Distinct().ToList();

                return;
            }

            await next();
        }
    }
}
