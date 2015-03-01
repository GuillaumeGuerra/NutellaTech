using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Extensions
{
    public static class MathExtensions
    {
        public static int Remainder(int up, int down)
        {
            return up - down * (up / down);
        }

        public static int Quotient(int up, int down)
        {
            return down * (up / down);
        }
    }
}
