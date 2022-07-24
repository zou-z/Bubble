using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubble.ViewModel
{
    internal class VmLocator
    {
        public static VmMain Main => main ??= new VmMain();

        private static VmMain? main = null;
    }
}
