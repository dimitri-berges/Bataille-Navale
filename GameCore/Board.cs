using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Board
    {
        public Case[,] Cases { get; private set; }
        public List<Bateau> Bateaux { get; private set; }

        public Board(int nbLignes, int nbColonnes) {
            Cases = new Case[nbLignes, nbColonnes];
            for (int i = 0; i < nbLignes; i++)
                for (int j = 0; j < nbColonnes; j++)
                    Cases[i, j] = new(i, j);
        }

    }
}
