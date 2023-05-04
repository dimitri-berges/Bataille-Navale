using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Joueur
    {
        public Plateau Plateau { get; private set; }
        public Plateau PlateauAdverse { get; private set; }
        public List<Bateau> BateauxDisponibles { get; private set; }
        public List<BateauEntity> Bateaux { get; private set; }

        public bool EstPretAJouer => BateauxDisponibles.Count == 0;
        public bool IsAlive { get
            {
                bool allBoatsDrowned = true;
                foreach (BateauEntity bateau in Bateaux)
                {
                    allBoatsDrowned &= bateau.IsDrowned;
                }
                return !allBoatsDrowned;
            }
        }

        public Joueur(Plateau plateau, List<Bateau> bateauxDisponibles) {
            Plateau = plateau;
            PlateauAdverse = new(plateau);
            BateauxDisponibles = bateauxDisponibles;
            Bateaux = new();
        }

        public void PlacerBateau(int x, int y, int indexBateauxDisponibles, bool vertical)
        {
            Bateau bateau = BateauxDisponibles[indexBateauxDisponibles];
            BateauEntity entity = new(x, y, vertical, bateau);
            for (int i = 0; i < bateau.taille; i++)
            {
                if (vertical)
                {
                    Plateau[x + i, y].Bateau = entity;
                    Plateau[x + i, y].Statut = CaseStatut.Boat;
                    entity.cases[i] = Plateau[x + i, y];
                } else
                {
                    Plateau[x, y + i].Bateau = entity;
                    Plateau[x, y + i].Statut = CaseStatut.Boat;
                    entity.cases[i] = Plateau[x, y + i];
                }
            }
            Bateaux.Add(entity);
            BateauxDisponibles.RemoveAt(indexBateauxDisponibles);
        }

        public HitResponse Hit(int x, int y)
        {
            return Plateau[x, y].Hit();
        }
        public HitResponse Tirer(int x, int y, Joueur joueur)
        {
            HitResponse hit = joueur.Hit(x, y);
            PlateauAdverse[x, y].Statut = hit switch
            {
                HitResponse.Missed => CaseStatut.WaterHit,
                HitResponse.Hit => CaseStatut.BoatHit,
                HitResponse.HitAndDrowned => CaseStatut.BoatDrowned,
                _ => CaseStatut.WaterHit,
            };
            return hit;
        }
    }
}
