using System;

namespace RestauranteTuristicoApp.Services
{
    public class ToastMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = "info"; // success, error, warning, info
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class ToastService
    {
        public event Action<ToastMessage>? OnShow;

        public void ShowSuccess(string message)
        {
            OnShow?.Invoke(new ToastMessage { Message = message, Type = "success" });
        }

        public void ShowError(string message)
        {
            OnShow?.Invoke(new ToastMessage { Message = message, Type = "error" });
        }

        public void ShowWarning(string message)
        {
            OnShow?.Invoke(new ToastMessage { Message = message, Type = "warning" });
        }

        public void ShowInfo(string message)
        {
            OnShow?.Invoke(new ToastMessage { Message = message, Type = "info" });
        }
    }
}
