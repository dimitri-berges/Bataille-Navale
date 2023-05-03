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
        internal BateauEntity? Bateau { get; set; }

        public Case(int x, int y) {
            Statut = CaseStatut.Water;
            this.x = x;
            this.y = y;
            Bateau = null;
        }

        internal HitResponse Hit()
        {
            switch (Statut)
            {
                case CaseStatut.Water:
                    Statut = CaseStatut.WaterHit;
                    return HitResponse.Missed;
                case CaseStatut.Boat:
                    Statut = CaseStatut.BoatHit;
                    return Bateau.BateauHitCheck() ? HitResponse.HitAndDrowned : HitResponse.Hit;
                default:
                    return HitResponse.Missed;
            }
        }
    }
}
