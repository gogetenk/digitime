namespace Digitime.Shared.UI.Services;

public class ErrorNotificationService
{
    public event Action<Exception> OnError;

    public void ShowError(Exception exception)
    {
        OnError?.Invoke(exception);
    }
}