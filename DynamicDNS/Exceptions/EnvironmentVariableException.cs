using System;

namespace DynamicDNS.Exceptions
{
    public class EnvironmentVariableException : Exception
    {
        public EnvironmentVariableException(string variableName) 
            : base($"Required environment variable {variableName} was not configured.") { }
    }
}
