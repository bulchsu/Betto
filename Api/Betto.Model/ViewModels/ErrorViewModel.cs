using System;

namespace Betto.Model.Models
{
    public class ErrorViewModel
    {
        public static class Factory
        {
            public static ErrorViewModel NewErrorFromException(Exception e)
            {
                var message = e.InnerException != null
                    ? $"{e.Message} {e.InnerException.Message}"
                    : e.Message;

                return new ErrorViewModel(message);
            }

            public static ErrorViewModel NewErrorFromMessage(string message)
            {
                return new ErrorViewModel(message);
            }
        }

        private ErrorViewModel(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
