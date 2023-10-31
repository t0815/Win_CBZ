using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Validation
{
    public abstract class EntryValidation
    {
        protected String type;

        protected String min;

        protected String max;

        protected String value;

        protected String pattern;

        protected delegate ValidationResult validationFn();

        public abstract List<ValidationResult> Validate();
    }
}
