using System.Collections.Generic;

namespace GestaoProdutos.API.Middlewares
{

    public class ErrorRequest
    {
        public bool Success { get; set; }
        public List<string> Message { get; set; }

        public ErrorRequest() { }

        public ErrorRequest(bool success, List<string> message)
        {
            Success = success;
            Message = message;
        }
        public ErrorRequest(bool success, string message)
        {
            Success = success;
            Message = new List<string> { message };
        }
    }
}