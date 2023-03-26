using System.Net;
using Microsoft.AspNetCore.Components;

namespace Digitime.Shared.UI.Components;

public class ErrorNotificationBase : ComponentBase
{
    [Parameter]
    public HttpStatusCode StatusCode { get; set; }

    [Parameter]
    public string ErrorMessage { get; set; }

    protected string Title { get; set; }
    protected string Message { get; set; }
    protected bool ShowNotification { get; set; }
    protected string AriaLiveCssClass => ShowNotification ? "pointer-events-auto fixed inset-0 flex items-end px-4 py-6 sm:items-start sm:p-6" : "pointer-events-none fixed inset-0 flex items-end px-4 py-6 sm:items-start sm:p-6";
    protected string NotificationCssClass => $"pointer-events-auto w-full max-w-sm overflow-hidden rounded-lg bg-white shadow-lg ring-1 ring-black ring-opacity-5 {EnterTransitionCssClass} {LeaveTransitionCssClass}";
    protected string EnterTransitionCssClass => "transform ease-out duration-300 transition";
    protected string LeaveTransitionCssClass => "transition ease-in duration-100";

    protected RenderFragment NotificationIcon
    {
        get
        {
            if (StatusCode >= HttpStatusCode.BadRequest && StatusCode < HttpStatusCode.InternalServerError)
            {
                return builder =>
                {
                    builder.OpenComponent(0, typeof(RenderFragment));
                    builder.AddMarkupContent(1, "<svg class=\"h-6 w-6 text-red-400\" fill=\"none\" viewBox=\"0 0 24 24\" stroke-width=\"1.5\" stroke=\"currentColor\" aria-hidden=\"true\"><path stroke-linecap=\"round\" stroke-linejoin=\"round\" d=\"M9 12.75L11.25 15 15 9.75M21 12a9 9 0 11-18 0 9 9 0 0118 0z\" /></svg>");
                    builder.CloseComponent();
                };
            }
            else
            {
                return builder =>
                {
                    builder.OpenComponent(0, typeof(RenderFragment));
                    builder.AddMarkupContent(1, "<svg class=\"h-6 w-6 text-yellow-400\" fill=\"none\" viewBox=\"0 0 24 24\" stroke-width=\"1.5\" stroke=\"currentColor\" aria-hidden=\"true\"><path stroke-linecap=\"round\" stroke-linejoin=\"round\" d=\"M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L4.34 16c-.77 1.333.192 3 1.732 3z\"/></svg>");
                    builder.CloseComponent();
                };
            }
        }
    }

    protected override void OnParametersSet()
    {
        ShowNotification = !string.IsNullOrEmpty(ErrorMessage);

        if (StatusCode >= HttpStatusCode.BadRequest && StatusCode < HttpStatusCode.InternalServerError)
        {
            Title = "Erreur fonctionnelle";
            Message = ErrorMessage;
        }
        else if (StatusCode >= HttpStatusCode.InternalServerError)
        {
            Title = "Erreur technique";
            Message = "Une erreur technique s'est produite. Veuillez réessayer plus tard.";
        }
    }

    protected void HideNotification()
    {
        ShowNotification = false;
        StateHasChanged();
    }
}

