using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Helper
{
    internal class RandomId
    {

        private static RandomId Instance;

        private static Random RandomProvider;

        public String make()
        {
            return RandomProvider.Next().ToString("X");
        }

        public static RandomId getInstance()
        {
            if (RandomId.Instance == null)
            {
                RandomId.Instance = new RandomId();
            }

            return RandomId.Instance;
        }

        private RandomId() 
        {
            RandomProvider = new Random();
        }
    }
}
