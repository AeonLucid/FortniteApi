using FortniteApi.Response.Persona;

namespace FortniteApi.Exceptions
{
    public class FortniteLookupException : FortniteException
    {
        public FortniteLookupException(string message, LookupErrorResponse error) : base(message)
        {
            Error = error;
        }

        public LookupErrorResponse Error { get; }
    }
}
