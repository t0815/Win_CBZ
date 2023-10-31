using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Validation
{
    public class ValidationResult
    {
        protected String Key;

        protected String Value;

        protected String Message;

        public ValidationResult(String key, String value, String message) 
        { 
            Key = key;
            Value = value;  
            Message = message;
        }
    }
}
