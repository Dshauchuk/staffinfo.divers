using System;

namespace Staffinfo.Divers.Shared.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a desired item not found
    /// </summary>
    public class NotFoundException : Exception
    {
        private const string DEFAULT_MESSAGE = "Объект не найден.";

        public NotFoundException() : base(DEFAULT_MESSAGE)
        {

        }

        public NotFoundException(string message) : base(message)
        {

        }

        public NotFoundException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
