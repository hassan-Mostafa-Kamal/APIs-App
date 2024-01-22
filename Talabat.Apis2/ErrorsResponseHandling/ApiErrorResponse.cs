
namespace Talabat.Apis2.ErrorsResponseHandling
{
    public class ApiErrorResponse
    {

        public int StatusCode { get; set; }
        public string?  Message { get; set; }

        public ApiErrorResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode ;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode( int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad Request you have made",
                401=> "Authorized you are not",
                404=> "Resource was not found",
                500=> "Errors are the path to the dark side , Errors lead to anger ,Anger lead to hate , Hate lead to change cerrer",
                _ => null

            };
        }
    }
}
