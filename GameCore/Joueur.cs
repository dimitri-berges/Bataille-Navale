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
                    Plateau[x, y + i].Bateau = entity;
                    Plateau[x, y + i].Statut = CaseStatut.Boat;
                    Plateau[x, y + i].BoatType = i < bateau.taille - 1 ? CaseBoatType.Vertical : CaseBoatType.EdgeDown;
                    entity.cases[i] = Plateau[x, y + i];
                } else
                {
                    Plateau[x + i, y].Bateau = entity;
                    Plateau[x + i, y].Statut = CaseStatut.Boat;
                    Plateau[x + i, y].BoatType = i < bateau.taille - 1 ? CaseBoatType.Horizontal : CaseBoatType.EdgeRight;
                    entity.cases[i] = Plateau[x + i, y];
                }
                if (i == 0)
                    Plateau[x, y].BoatType = vertical ? CaseBoatType.EdgeUp : CaseBoatType.EdgeLeft;
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
                if (vertical ? y + i >= Plateau.nbLignes : x + i >= Plateau.nbColonnes)
                    return false;
                Case @case = vertical ? Plateau[x, y + i] : Plateau[x + i, y];
                for (int offX = -1; offX <= 1; offX++)
                {
                    for (int offY = -1; offY <= 1; offY++)
                    {
                        if (@case.x + offX < 0 || @case.x + offX >= Plateau.nbColonnes
                         || @case.y + offY < 0 || @case.y + offY >= Plateau.nbLignes)
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
            if (hit == HitResponse.HitAndDrowned)
            {
                HitAndDrowned(x, y);
            }
            return hit;
        }

        public void HitAndDrowned(int x, int y)
        {
            for (int offX = -1; offX <= 1; offX++)
            {
                for (int offY = -1; offY <= 1; offY++)
                {
                    if (x + offX < 0 || x + offX >= PlateauAdverse.nbColonnes
                         || y + offY < 0 || y + offY >= PlateauAdverse.nbLignes)
                        continue;
                    if (PlateauAdverse[x + offX, y + offY].Statut == CaseStatut.BoatHit)
                    {
                        PlateauAdverse[x + offX, y + offY].Statut = CaseStatut.BoatDrowned;
                        HitAndDrowned(x + offX, y + offY);
                    }
                }
            }
        }
    }
}
