using System;

namespace MineClearingEvaluator.Common
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}