using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;

namespace Api
{
    public class Envelope
    {
        public object Result { get; }
        public string ErrorCode { get; }
        public string ErrorMessage { get; }
        public string InvalidField { get; }
        public DateTime TimeGenerated { get; }

        private Envelope(object result, Error error, string invalidField)
        {
            Result = result;
            ErrorCode = error?.Code;
            ErrorMessage = error?.Message;
            InvalidField = invalidField;
            TimeGenerated = DateTime.UtcNow;
        }

        public static Envelope Ok(object result = null)
        {
            return new Envelope(result, null, null);
        }

        public static Envelope Error(Error error, string invalidField)
        {
            return new Envelope(null, error, invalidField);
        }
    }
}
