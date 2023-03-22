using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    internal class Case
    {
        public CaseStatut Statut{ get; internal set; }
        public int x, y;
        internal Bateau? Bateau { get; set; }

    }
}
