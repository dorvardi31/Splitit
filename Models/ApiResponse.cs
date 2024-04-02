using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ImdbScraperApi.Models
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; } //Instead implementing to individual service everytime Using Data 
        public object Errors { get; set; }

        public ApiResponse(int statusCode, string message, T data = default, object errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            Errors = errors;
        }
    }

}
