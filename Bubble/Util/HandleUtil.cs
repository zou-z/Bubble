using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubble.Util
{
    internal class HandleUtil
    {
        public static string Format(IntPtr handle)
        {
            string hex = Convert.ToString(handle.ToInt64(), 16);
            return "0x" + hex.ToUpper().PadLeft(8, '0');
        }
    }
}
