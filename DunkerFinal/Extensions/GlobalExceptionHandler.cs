namespace DunkerFinal.Extensions;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(RequestDelegate next)
    {
        _next = next;

    }


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception e)
        {
            var message = e.Message.ToString();

            context.Response.Redirect($"/Home/ErrorPage?error={message}");
        }
    }


    private string SanitizeErrorMessage(string errorMessage)
    {
        return new string(errorMessage.Where(c => !char.IsControl(c)).ToArray());
    }
}