using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Player
    {
        public Board Board { get; private set; }
        public Bateau[] BateauxDisponibles { get; private set; }
        public BateauEntity[] Bateaux { get; private set; }

        public Player(Board board, Bateau[] bateauxDisponibles) {
            Board = board;
            BateauxDisponibles = bateauxDisponibles;
            Bateaux = new BateauEntity[bateauxDisponibles.Length];
        }
    }
}
