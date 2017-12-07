using FortniteApi.Response.Account;

namespace FortniteApi.Exceptions
{
    public class FortniteAuthException : FortniteException
    {
        public FortniteAuthException(string message, OAuthTokenError error) : base(message)
        {
            Error = error;
        }

        public OAuthTokenError Error { get; }
    }
}
