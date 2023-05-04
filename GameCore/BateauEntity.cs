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

        internal Case[] cases;

        internal bool vertical;
        public bool Vertical => vertical;
        public bool Horizontal => !vertical;

        public bool IsDrowned { get; private set; }



        public BateauEntity(int x, int y, bool oriantation, Bateau bateau)
        {
            this.taille = bateau.taille;
            this.nom = bateau.nom;
            this.X = x;
            this.Y = y;
            this.vertical = oriantation;
            this.IsDrowned = false;
            cases = new Case[bateau.taille];
        }

        public bool BateauHitCheck()
        {
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
                IsDrowned = true;

                foreach (Case caseCourante in cases)
                {
                    caseCourante.Statut = CaseStatut.BoatDrowned;
                }
            }

            return IsDrowned;
        }

    }
}
