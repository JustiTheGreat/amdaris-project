using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.utils.exceptions
{
    public class GameNotPlayedByPlayerException : AmdarisProjectException
    {
        public GameNotPlayedByPlayerException(String message) : base(message)
        {
        }
    }
}
