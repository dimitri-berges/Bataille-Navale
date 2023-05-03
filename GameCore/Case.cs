using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Case
    {
        public CaseStatut Statut { get; internal set; }
        public int x, y;
        internal Bateau? Bateau { get; set; }

        public Case(int x, int y) {
            Statut = CaseStatut.Water;
            this.x = x;
            this.y = y;
            Bateau = null;
        }
    }
}
