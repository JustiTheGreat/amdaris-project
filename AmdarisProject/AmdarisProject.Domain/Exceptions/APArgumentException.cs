﻿namespace AmdarisProject.Domain.Exceptions
{
    public class APArgumentException(string className, string methodName, string message)
        : AmdarisProjectException(className, methodName, message)
    { }
}
