﻿using System;

namespace Utilities
{
    static class StaticRandom
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public static int RandomNumber(int min, int max)
        {
            lock (syncLock) // synchronize
            {
                return random.Next(min, max);
            }
        }
    }
}
