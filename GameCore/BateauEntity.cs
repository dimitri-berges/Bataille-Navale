using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class BateauEntity : Bateau
    {
        public int X { get; internal set;}
        public int Y { get; internal set;}

        internal List<Case> cases;

        internal bool vertical;
        public bool Vertical => vertical;
        public bool Horizontal => !vertical;

        public Boolean BateauHitCheck()
        {
            bool Couller = false;
            int NombreTouches = 0;

            foreach (Case caseCourante in cases)
            {
                if (caseCourante.Statut.Equals(CaseStatut.BoatHit))
                {
                    NombreTouches++;
                }
            }

            if (NombreTouches == this.taille)
            {
                Couller = true;

                foreach (Case caseCourante in cases)
                {
                    caseCourante.Statut = CaseStatut.BoatDrowned;
                }
            }

            return Couller;
        }

    }
}
