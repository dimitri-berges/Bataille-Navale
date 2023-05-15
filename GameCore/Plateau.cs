using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Plateau
    {
        public Case[,] Cases { get; private set; }
        public List<Bateau> Bateaux { get; private set; }

        public Plateau(int nbLignes, int nbColonnes) {
            Cases = new Case[nbColonnes, nbLignes];
            for (int i = 0; i < nbLignes; i++)
                for (int j = 0; j < nbColonnes; j++)
                    Cases[j, i] = new(j, i);
        }

        public Plateau(Plateau plateau) : this(plateau.Cases.GetLength(1), plateau.Cases.GetLength(0)) {}

        public Case this[int x, int y] => Cases[x, y];

    }
}
