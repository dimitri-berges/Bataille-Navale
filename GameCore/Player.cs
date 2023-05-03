using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Player
    {
        public Plateau Plateau { get; private set; }
        public Plateau PlateauAdverse { get; private set; }
        public Bateau[] BateauxDisponibles { get; private set; }
        public BateauEntity[] Bateaux { get; private set; }

        public Player(Plateau plateau, Bateau[] bateauxDisponibles) {
            Plateau = plateau;
            PlateauAdverse = new(plateau);
            BateauxDisponibles = bateauxDisponibles;
            Bateaux = new BateauEntity[bateauxDisponibles.Length];
        }
    }
}
