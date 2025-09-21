﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Helper
{
    internal class RandomId
    {

        private static RandomId Instance;

        private Random RandomProvider;


        /// <summary>
        /// Generate a random id
        /// </summary>
        /// <returns>Hexadecimal random string</returns>
        public String Make()
        {
            if (RandomProvider == null)
            {
                throw new ApplicationException("Error! RandomID- Provider not initialized. Call 'GetInstance()' first!", false);
            }

            return RandomProvider.Next().ToString("X");
        }

        public static RandomId GetInstance()
        {
            
            RandomId.Instance ??= new RandomId();

            return RandomId.Instance;
        }

        private RandomId() 
        {
            RandomProvider = new Random();
        }
    }
}
