﻿using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.utils.exceptions
{
    public class GameNotPlayedByPlayerException(string message) : AmdarisProjectException(message)
    {
    }
}
