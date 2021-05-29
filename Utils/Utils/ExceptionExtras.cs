using System;
using System.Text;

namespace Utils
{

    public class ExceptionExtras {
        private string message = null;
        private StringBuilder parameters = null;

        public ExceptionExtras(string message) {
            this.message = message;
            parameters = new StringBuilder();
        }

        public ExceptionExtras()
            : this("") {
        }

        public string Message {
            get {
                return message;
            }
            set {
                message = value;
            }
        }

        public override string ToString() {
            return message + Environment.NewLine + parameters.ToString();
        }

        public void AddParam(string name, string value) {
            parameters.Append(name).Append(": ");
            if(value == null) {
                parameters.Append("null");
            } else {
                parameters.Append(value);
            }
            parameters.Append(Environment.NewLine);
        }

        public void AddParam(string name, long value) {
            parameters.Append(name).Append(": ");
            parameters.Append(value);
            parameters.Append(Environment.NewLine);
        }

        public void AddParam(string name, int value) {
            parameters.Append(name).Append(": ");
            parameters.Append(value);
            parameters.Append(Environment.NewLine);
        }

        public void AddParam(string name, float value) {
            parameters.Append(name).Append(": ");
            parameters.Append(value);
            parameters.Append(Environment.NewLine);
        }

        public void AddParam(string name, double value) {
            parameters.Append(name).Append(": ");
            parameters.Append(value);
            parameters.Append(Environment.NewLine);
        }

        public void AddParam(string name, decimal value) {
            parameters.Append(name).Append(": ");
            parameters.Append(value);
            parameters.Append(Environment.NewLine);
        }

        public void AddParam(string name, bool value) {
            parameters.Append(name).Append(": ");
            parameters.Append(value);
            parameters.Append(Environment.NewLine);
        }

        public void AddParam(string name, DateTime value) {
            parameters.Append(name).Append(": ");
            if(value == null) {
                parameters.Append("null");
            } else {
                parameters.Append(String.Format("{0:dd/MM/yyyy HH:mm:ss.fff}", value));
            }
            parameters.Append(Environment.NewLine);
        }

        public void AddParam(string name, Object value) {
            parameters.Append(name).Append(": ");
            if(value == null) {
                parameters.Append("null");
            } else {
                parameters.Append("{").Append(Environment.NewLine).Append(value.ToString()).Append("}");
            }
            parameters.Append(Environment.NewLine);
        }
    }
}
