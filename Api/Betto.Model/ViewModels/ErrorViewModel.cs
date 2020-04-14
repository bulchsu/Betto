using System;

namespace Betto.Model.Models
{
    public class ErrorViewModel
    {
        public static class Factory
        {
            public static ErrorViewModel NewErrorFromException(Exception e)
            {
                return new ErrorViewModel
                {
                    Message = e.InnerException != null
                        ? $"{e.Message} {e.InnerException.Message}"
                        : e.Message
                };
            }
        }

        public string Message { get; set; }
    }
}
