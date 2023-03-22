using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    internal class BateauEntity : Bateau
    {
        public int x { get; internal set;}
        public int y { get; internal set;}

        internal List<Case> cases;

        internal bool orientation;

    }
}
