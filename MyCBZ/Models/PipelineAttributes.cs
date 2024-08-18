using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class PipelineAttributes
    {

        public Dictionary<string, object> Payload;


        public PipelineAttributes()
        {
            Payload = new Dictionary<String, object>();
        }

        public PipelineAttributes SetAttribute(string key, object payload)
        {
            Payload.Add(key, payload);
            
            return this;
        }
    }
}
