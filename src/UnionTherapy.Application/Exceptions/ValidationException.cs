using System;
using System.Collections.Generic;

namespace UnionTherapy.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public ValidationException(string message) : base(message)
        {
            Errors = new[] { message };
        }

        public ValidationException(IEnumerable<string> errors) : base("Validation failed")
        {
            Errors = errors;
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
            Errors = new[] { message };
        }
    }
} 