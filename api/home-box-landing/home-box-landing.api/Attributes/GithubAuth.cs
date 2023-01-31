using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace home_box_landing.api.Attributes
{
    public class GithubAuth : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            actionContext.HttpContext.Request.Headers.TryGetValue("X-Gitlab-Token", out var authorizationToken);

            if (authorizationToken != "ZKLjXEAYU8wX12y3JVc4x1BKZM5XZW8lerUxyqu2TKnV50awYE")
                actionContext.Result = new UnauthorizedResult();

            actionContext.HttpContext.Request.Headers.TryGetValue("X-Gitlab-Event", out var webhookType);

            if (webhookType != "Pipeline Hook")
                actionContext.Result = new UnauthorizedResult();
        }
    }
}