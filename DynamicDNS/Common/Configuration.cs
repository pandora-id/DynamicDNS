using DynamicDNS.Exceptions;
using System;

namespace DynamicDNS.Common
{
    public static class Configuration
    {
        private static string GetEnvironmentVariable(string variable)
        {
            var value = Environment.GetEnvironmentVariable(variable);
            if (string.IsNullOrEmpty(value))
            {
                throw new EnvironmentVariableException(variable);
            }

            return value;
        }

        public static string GetCloudflareApiToken()
        {
            return GetEnvironmentVariable("CLOUDFLARE_API_TOKEN");
        }

        public static string GetCloudflareRecordName()
        {
            return GetEnvironmentVariable("CLOUDFLARE_RECORD_NAME");
        }
    }
}
