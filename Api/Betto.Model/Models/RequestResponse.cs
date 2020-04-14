using System.Collections.Generic;

namespace Betto.Model.Models
{
    public class RequestResponse<T> where T : class
    {
        public RequestResponse(int statusCode, 
            IEnumerable<ErrorViewModel> errors, 
            T result)
        {
            StatusCode = statusCode;
            Errors = errors;
            Result = result;
        }

        public int StatusCode { get; set; }
        public IEnumerable<ErrorViewModel> Errors { get; set; }
        public T Result { get; set; }
    }
}
