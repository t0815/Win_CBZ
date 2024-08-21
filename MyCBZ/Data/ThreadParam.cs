using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class ThreadParam
    {
        public Dictionary<string, PipelineVariable> PipelineResult { get; set; } = new Dictionary<string, PipelineVariable>();

        public List<StackItem> Stack { get; set; }

        public CancellationToken CancelToken { get; set; }

        public PipelineVariable GetResult(string name)
        {
            PipelineVariable variable;

            if (!PipelineResult.TryGetValue(name, out variable))
            {
                return null;
            }

            return variable;
        }

        public void SetResult(string name, PipelineVariable value) 
        {
            if (PipelineResult.ContainsKey(name))
            {
                PipelineResult[name] = value;
            } else
            {
                PipelineResult.Add(name, value);
            }
        }
    }
}
