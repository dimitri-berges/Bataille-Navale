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
        public static readonly Dictionary<CaseStatut, char> statutsDeCase = new()
        {
            { CaseStatut.Water, '.' },
            { CaseStatut.Boat, '.' },
            { CaseStatut.WaterHit, 'X' },
            { CaseStatut.BoatHit, '+' },
            { CaseStatut.BoatDrowned, '#' },
        };
        public static readonly Dictionary<CaseBoatType, char> typesDeBateau = new()
        {
            { CaseBoatType.None, '.' },
            { CaseBoatType.EdgeUp, 'A' },
            { CaseBoatType.EdgeDown, 'V' },
            { CaseBoatType.EdgeLeft, '<' },
            { CaseBoatType.EdgeRight, '>' },
            { CaseBoatType.Vertical, '|' },
            { CaseBoatType.Horizontal, '-' },
        };

        static void Main(string[] args)
        {
            GameEngine gameEngine;
            try
            {
                gameEngine = Introduction();
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                Exit();
                return;
            }
            PhaseDePlacement(gameEngine);
            PhaseDeJeu(gameEngine);
            AnsiConsole.Clear();
            AddFiglet("Bataille navale", Color.Blue);
            WriteLine("Joueur 1");
            AfficherPlateau(gameEngine.Joueur1);
            WriteLine("Joueur 2");
            AfficherPlateau(gameEngine.Joueur2);
            AddFiglet("Victoire", Color.Gold1);
            AddFiglet(gameEngine.Joueur1.IsAlive ? "Joueur 1" : "Joueur 2", Color.Green);
            Exit();
        }

        private static void PhaseDeJeu(GameEngine gameEngine)
        {
            while (gameEngine.Joueur1.IsAlive && gameEngine.Joueur2.IsAlive)
            {
                JoueurJoue(gameEngine);
                if (gameEngine.JoueurAdverse.IsAlive)
                    ChangementDeJoueur(gameEngine);
            }
        }
        private static void JoueurJoue(GameEngine gameEngine)
        {
            Joueur joueur = gameEngine.JoueurActuel;
            AfficherPlateau(joueur);
            int index_colonne = SelectColonne(joueur);
            int index_ligne = SelectLigne(joueur);
            HitResponse hit = gameEngine.Jouer(index_colonne, index_ligne);
            ResetConsole(gameEngine);
            AfficherPlateau(joueur);
            AddFiglet(hit switch
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
            WriteLine("Appuyez sur une touche pour continuer."); Console.ReadKey(true);
        }
        private static void PhaseDePlacement(GameEngine gameEngine)
        {
            PhaseDePlacementJoueur(gameEngine);
            ChangementDeJoueur(gameEngine);
            PhaseDePlacementJoueur(gameEngine);
            ChangementDeJoueur(gameEngine);
        }
        private static void PhaseDePlacementJoueur(GameEngine gameEngine)
        {
            while (!gameEngine.JoueurActuel.EstPretAJouer)
            {
                AfficherPlateau(gameEngine.JoueurActuel);
                PlacerBateau(gameEngine.JoueurActuel);
                ResetConsole(gameEngine);
            }
        }
        private static void ChangementDeJoueur(GameEngine gameEngine)
        {
            gameEngine.FinDeTour();
            ResetConsole(gameEngine);
            WriteLine("Attention changement de joueur !");
            WriteLine("Appuyez sur une touche quand vous êtes prêt à jouer.");
            Console.ReadKey(true);
            ResetConsole(gameEngine);
        }
        private static void PlacerBateau(Joueur joueur)
        {
            int index_bateau, index_colonne, index_ligne;
            bool vertical, ok = false;
            do
            {
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

                index_colonne = SelectColonne(joueur);
                index_ligne = SelectLigne(joueur);
                AnsiConsole.WriteLine(GameEngine.GetCoords(index_colonne, index_ligne));

                ok = joueur.VerifierPlacementBateau(index_colonne, index_ligne, vertical, joueur.BateauxDisponibles[index_bateau].taille);
                if (ok)
                    ok = AnsiConsole.Confirm("Placement validé ?");
                else
                    AnsiConsole.WriteLine("Placement non valide, on recommence");
            } while (!ok);
            joueur.PlacerBateau(index_colonne, index_ligne, vertical, index_bateau);
        }
        private static int SelectLigne(Joueur joueur)
        {
            int index_ligne;
            SelectionPrompt<int> prompt_ligne = new SelectionPrompt<int>()
                                            .Title("À partir de quelle ligne voulez-vous le placer ? (Sens de gauche à droite ou de haut en bas)")
                                            .AddChoices(Enumerable.Range(0, joueur.Plateau.Cases.GetLength(1)));
            prompt_ligne.Converter = i => GameEngine.GetLigne(i).ToString();
            index_ligne = AnsiConsole.Prompt(prompt_ligne);
            return index_ligne;
        }
        private static int SelectColonne(Joueur joueur)
        {
            int index_colonne;
            SelectionPrompt<int> prompt_colonne = new SelectionPrompt<int>()
                                            .Title("À partir de quelle colonne voulez-vous le placer ? (Sens de gauche à droite ou de haut en bas)")
                                            .AddChoices(Enumerable.Range(0, joueur.Plateau.Cases.GetLength(0)));
            prompt_colonne.Converter = i => GameEngine.GetColonne(i).ToString();
            index_colonne = AnsiConsole.Prompt(prompt_colonne);
            return index_colonne;
        }
        private static void ResetConsole(GameEngine gameEngine)
        {
            AnsiConsole.Clear();
            AddFiglet("Bataille navale", Color.Blue);
            WriteLine(gameEngine.JoueurActuel.Equals(gameEngine.Joueur1) ? "Joueur 1" : "Joueur 2");
        }
        public static int NbDigits(int value) {
            return ((value == 0) ? 1 : ((int)Math.Floor(Math.Log10(Math.Abs(value))) + 1));
        }
        private static void AfficherPlateau(Joueur joueur)
        {
            int marginLeft = NbDigits(joueur.Plateau.Cases.GetLength(1));
            AfficherLettresColonne(marginLeft, joueur.Plateau.Cases.GetLength(0));
            for (int i = 0; i < joueur.Plateau.Cases.GetLength(1); i++)
                AfficherLignePlateau(marginLeft, joueur, i);
            AnsiConsole.WriteLine(" ");
        }
        private static void AfficherLettresColonne(int marginLeft, int nbColonnes)
        {
            StringBuilder stringBuilder = new();
            for (int i = 0; i <= marginLeft; i++) stringBuilder.Append(' ');
            for (int i = 0; i < nbColonnes; i++)
            {
                stringBuilder.Append(GameEngine.GetColonne(i));
                stringBuilder.Append(' ');
            }
            for (int i = 0; i < marginLeft; i++) stringBuilder.Append(' ');
            for (int i = 0; i <= marginLeft; i++) stringBuilder.Append(' ');
            for (int i = 0; i < nbColonnes; i++)
            {
                stringBuilder.Append(GameEngine.GetColonne(i));
                stringBuilder.Append(' ');
            }
            for (int i = 0; i < marginLeft; i++) stringBuilder.Append(' ');
            AnsiConsole.Write(new Text(stringBuilder.ToString()).Centered());
        }
        private static void AfficherLignePlateau(int marginLeft, Joueur joueur, int ligne)
        {
            StringBuilder stringBuilder = new();
            int numeroDeLigne = GameEngine.GetLigne(ligne);
            for (int i = 0; i < marginLeft - NbDigits(numeroDeLigne); i++) stringBuilder.Append(' ');
            stringBuilder.Append(numeroDeLigne);
            stringBuilder.Append(' ');
            for (int x = 0; x < joueur.Plateau.Cases.GetLength(0); x++)
            {
                stringBuilder.Append(GetCharOfCase(joueur.Plateau[x, ligne]));
                stringBuilder.Append(' ');
            }
            for (int i = 0; i < marginLeft; i++) stringBuilder.Append(' ');
            for (int i = 0; i <= marginLeft; i++) stringBuilder.Append(' ');

            for (int x = 0; x < joueur.PlateauAdverse.Cases.GetLength(0); x++)
            {
                stringBuilder.Append(GetCharOfCase(joueur.PlateauAdverse[x, ligne]));
                stringBuilder.Append(' ');
            }
            stringBuilder.Append(numeroDeLigne);
            for (int i = 0; i < marginLeft - NbDigits(numeroDeLigne); i++) stringBuilder.Append(' ');
            AnsiConsole.Write(new Text(stringBuilder.ToString()).Centered());
        }
        private static char GetCharOfCase(Case @case)
        {
            return @case.Statut == CaseStatut.Boat ? typesDeBateau[@case.BoatType] : statutsDeCase[@case.Statut];
        }
        private static GameEngine Introduction()
        {
            AddFiglet("Bataille navale", Color.Blue);
            GameEngine gameEngine = GameEngine.FromJSON(APIManager.APICaller.GetJSON());
            WriteLine("Bonjour et bienvenue dans cette bataille navale !");
            WriteLine("Vous serez 2 joueurs ici et jouerez chacun votre tour.");
            WriteLine("Voici un exemple de plateau de jeu :\n");
            AfficherPlateau(gameEngine.JoueurActuel);
            WriteLine("Sur la gauche vous trouverez votre plateau de jeu avec vos bateaux une fois placées.");
            WriteLine("Sur la droite vous trouverez votre vue du plateau adverses avec vos tirs réussis ou échoués.");
            WriteLine("\nQuand vous serez prêt à jouer, appuyer sur une touche de votre clavier.");
            Console.ReadKey(true);
            ResetConsole(gameEngine);
            return gameEngine;
        }
        private static void AddFiglet(string text, Color color)
        {
            AnsiConsole.Write(new FigletText(text).Centered().Color(color));
        }
        private static void WriteLine(string text)
        {
            AnsiConsole.Write(new Text(text).Centered());
        }
        private static void Exit()
        {
            AnsiConsole.Write("Merci d'avoir jouer !\nAppuyer sur n'importe quelle touche pour quitter..."); Console.ReadKey(true);
        }
    }
}
