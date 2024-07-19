namespace Tchat.Api.Exceptions.Utils
{
    [Serializable]
    public class RessourceAlreadyExists : Exception
    {
        public RessourceAlreadyExists()
        {
        }

        public RessourceAlreadyExists(string? message) : base(message)
        {
        }

        public RessourceAlreadyExists(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}