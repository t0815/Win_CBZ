using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;

namespace Win_CBZ.Models
{
    public class GlobalAction
    {

        public GlobalAction() { }

        public String Key { get; set; } 

        public GlobalAction(string name) { }

        public string Name { get; set; }    

        public Task<TaskResult> Action { get; set; }

        public String Message { get; set; }
    }
}
