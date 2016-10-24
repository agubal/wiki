using System;

namespace Wiki.Entities.Common
{
    public class ErrorUtils
    {
        public static string GetErrorMessage(Exception e, string defaultException)
        {
            if (e.InnerException != null) return GetErrorMessage(e.InnerException, defaultException);
            return !string.IsNullOrWhiteSpace(e.Message) ? e.Message : defaultException;
        }
    }
}
