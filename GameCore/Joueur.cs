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

        public bool PlacerBateau(int x, int y, bool vertical, int indexBateauxDisponibles)
        {
            Bateau bateau = BateauxDisponibles[indexBateauxDisponibles];
            bool placementOK = VerifierPlacementBateau(x, y, vertical, bateau.taille);
            if (!placementOK)
                return false;
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
            return true;
        }

        public bool VerifierPlacementBateau(int x, int y, bool vertical, int taille)
        {
            bool placementValide = true;
            for (int i = 0; i < taille; i++)
            {
                Case @case = vertical ? Plateau[x + i, y] : Plateau[x, y + i];
                for (int offX = -1; offX <= 1; offX++)
                {
                    for (int offY = -1; offY <= 1; offY++)
                    {
                        if (@case.x + offX < 0 || @case.x + offX >= Plateau.Cases.GetLength(0)
                         || @case.y + offY < 0 || @case.y + offY >= Plateau.Cases.GetLength(1))
                            continue;
                        placementValide &= Plateau[@case.x + offX, @case.y + offY].Bateau == null;
                        if (!placementValide) break;
                    }
                    if (!placementValide) break;
                }
                if (!placementValide) break;
            }
            return placementValide;
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
