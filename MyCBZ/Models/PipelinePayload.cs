using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class PipelinePayload
    {
        public const String PAYLOAD_EXECUTE_RENAME_SCRIPT = "RenameAllPages()";
       
        public Dictionary<String, String> Attributes;

      

        public PipelinePayload()
        {
            Attributes = new Dictionary<String, String>();
        }

        public PipelinePayload SetAttribute(String name, String value)
        {
            Attributes.Add(name, value);

            return this;
        }

        public String GetAttribute(String key)
        {
            if (Attributes.ContainsKey(key))
            {
                return Attributes[key];
            } else
            {
                return null;
            }
        }
    }
}
