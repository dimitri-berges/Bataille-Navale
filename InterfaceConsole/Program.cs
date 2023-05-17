using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCore;
using Spectre.Console;

namespace InterfaceConsole
{
    internal class Program
    {
        GameEngine gameEngine;

        static void Main()
        {
            Program program = new();
            try
            {
                program.Introduction();
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                Affichage.Exit();
                return;
            }
            //program.gameEngine.JoueurActuel.BateauxDisponibles.RemoveRange(0, 4);
            //program.gameEngine.JoueurAdverse.BateauxDisponibles.RemoveRange(0, 4);
            program.PhaseDePlacement();
            program.PhaseDeJeu();
            program.AffichageFinDeJeu();
            Affichage.Exit();
        }

        private void AffichageFinDeJeu()
        {
            AnsiConsole.Clear();
            Affichage.AddFiglet("Bataille navale", Color.Blue);
            Affichage.AddFiglet("Victoire", Color.Gold1);
            Affichage.AddFiglet(gameEngine.Joueur1.IsAlive ? "Joueur 1" : "Joueur 2", Color.Green);
            Affichage.WriteLine("Joueur 1");
            Affichage.AfficherPlateau(gameEngine.Joueur1);
            Affichage.WriteLine("Joueur 2");
            Affichage.AfficherPlateau(gameEngine.Joueur2);
        }
        private void PhaseDeJeu()
        {
            while (gameEngine.Joueur1.IsAlive && gameEngine.Joueur2.IsAlive)
            {
                JoueurJoue();
                if (gameEngine.JoueurAdverse.IsAlive)
                    ChangementDeJoueur();
            }
        }
        private void JoueurJoue()
        {
            Joueur joueur = gameEngine.JoueurActuel;
            int index_colonne, index_ligne;
            bool ok = false;
            do
            {
                ResetConsole();
                Affichage.AfficherPlateau(joueur);
                index_colonne = Affichage.SelectColonne(joueur);
                index_ligne = Affichage.SelectLigne(joueur);
                ok = AnsiConsole.Confirm($"Tir en {GameEngine.GetCoords(index_colonne, index_ligne)} ?");
            } while (!ok);
            HitResponse hit = gameEngine.Jouer(index_colonne, index_ligne);
            ResetConsole();
            Affichage.AfficherPlateau(joueur);
            Affichage.AddFiglet(hit switch
            {
                HitResponse.Missed => "Raté",
                HitResponse.Hit => "Touché",
                HitResponse.HitAndDrowned => "Touché coulé",
                _ => "Erreur"
            }, hit switch
            {
                HitResponse.Missed => Color.Aqua,
                HitResponse.Hit => Color.OrangeRed1,
                HitResponse.HitAndDrowned => Color.Red,
                _ => Color.Default
            });
            Affichage.WriteLine("Appuyez sur une touche pour continuer."); Console.ReadKey(true);
        }
        private void PhaseDePlacement()
        {
            PhaseDePlacementJoueur();
            ChangementDeJoueur();
            PhaseDePlacementJoueur();
            ChangementDeJoueur();
        }
        private void PhaseDePlacementJoueur()
        {
            while (!gameEngine.JoueurActuel.EstPretAJouer)
            {
                PlacerBateau(gameEngine.JoueurActuel);
            }
            ResetConsole();
            Affichage.AfficherPlateau(gameEngine.JoueurActuel);
            Affichage.WriteLine("Voici votre plateau de jeu, nous allons maintenance changer de joueur."); Console.ReadKey(true);
        }
        private void ChangementDeJoueur()
        {
            gameEngine.FinDeTour();
            ResetConsole();
            Affichage.WriteLine("Attention changement de joueur !");
            Affichage.WriteLine("Appuyez sur une touche quand vous êtes prêt à jouer.");
            Console.ReadKey(true);
            ResetConsole();
        }
        private void PlacerBateau(Joueur joueur)
        {
            int index_bateau, index_colonne, index_ligne;
            bool vertical, ok = false;
            do
            {
                ResetConsole();
                Affichage.AfficherPlateau(gameEngine.JoueurActuel);
                SelectionPrompt<int> prompt_bateau = new SelectionPrompt<int>()
                                .Title("Choissez le bateau que vous souhaitez placer")
                                .AddChoices(Enumerable.Range(0, joueur.BateauxDisponibles.Count));
                prompt_bateau.Converter = i => $"{joueur.BateauxDisponibles[i].nom} ({joueur.BateauxDisponibles[i].taille} cases)";
                index_bateau = AnsiConsole.Prompt(prompt_bateau);
                AnsiConsole.WriteLine(prompt_bateau.Converter(index_bateau));

                SelectionPrompt<bool> prompt_orientation = new SelectionPrompt<bool>()
                                .Title("Dans quelle orientation ?");
                prompt_orientation.AddChoice(true); prompt_orientation.AddChoice(false);
                prompt_orientation.Converter = (orientation) => orientation ? "Vertical" : "Horizontal";
                vertical = AnsiConsole.Prompt<bool>(prompt_orientation);
                AnsiConsole.WriteLine(prompt_orientation.Converter(vertical));

                index_colonne = Affichage.SelectColonne(joueur, false, vertical ? 0 : joueur.BateauxDisponibles[index_bateau].taille -1);
                index_ligne = Affichage.SelectLigne(joueur, false, vertical ? joueur.BateauxDisponibles[index_bateau].taille - 1 : 0);
                AnsiConsole.WriteLine(GameEngine.GetCoords(index_colonne, index_ligne));

                ok = joueur.VerifierPlacementBateau(index_colonne, index_ligne, vertical, joueur.BateauxDisponibles[index_bateau].taille);
                if (ok)
                {
                    Joueur dummy_joueur = new(new(gameEngine.nbLignes, gameEngine.nbColonnes), (new Bateau[] { joueur.BateauxDisponibles[index_bateau] }).ToList());
                    dummy_joueur.PlacerBateau(index_colonne, index_ligne, vertical, 0);
                    Affichage.AfficherPlateau(dummy_joueur);
                    ok = AnsiConsole.Confirm("Placement validé ?");
                }
                else
                {
                    AnsiConsole.WriteLine("Placement non valide, on recommence");
                    Console.ReadKey(true);
                }
            } while (!ok);
            joueur.PlacerBateau(index_colonne, index_ligne, vertical, index_bateau);
        }
        private void ResetConsole()
        {
            AnsiConsole.Clear();
            Affichage.AddFiglet("Bataille navale", Color.Blue);
            Affichage.WriteLine(gameEngine.JoueurActuel.Equals(gameEngine.Joueur1) ? "Joueur 1" : "Joueur 2");
        }
        private void Introduction()
        {
            Affichage.AddFiglet("Bataille navale", Color.Blue);
            this.gameEngine = GameEngine.FromJSON(APIManager.APICaller.GetJSON());
            Affichage.WriteLine("Bonjour et bienvenue dans cette bataille navale !");
            Affichage.WriteLine("Vous serez 2 joueurs ici et jouerez chacun votre tour.");
            Affichage.WriteLine("Voici un exemple de plateau de jeu :\n");
            Affichage.AfficherPlateau(gameEngine.JoueurActuel);
            Affichage.WriteLine("Sur la gauche vous trouverez votre plateau de jeu avec vos bateaux une fois placées.");
            Affichage.WriteLine("Sur la droite vous trouverez votre vue du plateau adverses avec vos tirs réussis ou échoués.");
            Affichage.WriteLine("\nQuand vous serez prêt à jouer, appuyer sur une touche de votre clavier.");
            Console.ReadKey(true);
            ResetConsole();
        }
    }
}
