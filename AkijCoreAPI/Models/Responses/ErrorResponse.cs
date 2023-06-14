namespace AkijCoreAPI.Models.Responses
{
    public class ErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ErrorResponse(string error) : this(new List<string>() { error }) { }
        
        public ErrorResponse(IEnumerable<string> errors)
        {
            Errors = errors;
        }
    }
}
