using System;

namespace Utils
{
    /// <summary>
    /// Exception custom, viene lanciata quando è necessario propagare un eccezione
    /// che è stata già catchata a fini di log oppure quando si vuole gestire come
    /// eccezione un errore di logica e non di runtime
    /// </summary>
    public class CustomException : ApplicationException {
        public CustomException(string message)
            : base(message) {
        }

        public CustomException(string message, Exception innerException)
            : base(message, innerException) {
        }

        public CustomException(ExceptionExtras eh)
            : base(eh.ToString()) {
        }

        public CustomException(ExceptionExtras eh, Exception innerException)
            : base(eh.ToString(), innerException) {
        }
    }
}