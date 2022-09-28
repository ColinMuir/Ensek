using Microsoft.AspNetCore.Mvc;

namespace ENSEK.WebApi.Tests.Extensions;

public static class ActionResultExtensions
{
    public static T? GetActionResultValue<T>(this ActionResult<T> actionResult)
    {
        if (actionResult.Result is not null)
        {
            var objectResult = actionResult.Result as ObjectResult;

            return (T?)objectResult?.Value;
        }

        if (actionResult.Value is not null)
            return actionResult.Value;

        return default;
    }
}