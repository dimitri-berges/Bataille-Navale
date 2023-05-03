using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    internal class BateauEntity : Bateau
    {
        public int X { get; internal set;}
        public int Y { get; internal set;}

        internal List<Case> cases;

        internal bool orientation;

    }
}
