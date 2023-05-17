using GameCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceConsole
{
    internal class Affichage
    {
        public static void AddFiglet(string text, Color color)
        {
            AnsiConsole.Write(new FigletText(text).Centered().Color(color));
        }
        public static void AfficherLettresColonne(int marginLeft, int nbColonnes)
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
        public static void AfficherLignePlateau(int marginLeft, Joueur joueur, int ligne)
        {
            StringBuilder stringBuilder = new();
            int numeroDeLigne = GameEngine.GetLigne(ligne);
            for (int i = 0; i < marginLeft - Utils.NbDigits(numeroDeLigne); i++) stringBuilder.Append(' ');
            stringBuilder.Append(numeroDeLigne);
            stringBuilder.Append(' ');
            for (int x = 0; x < joueur.Plateau.nbColonnes; x++)
            {
                stringBuilder.Append(Utils.GetMarkupForChar(Utils.GetCharOfCase(joueur.Plateau[x, ligne])));
                stringBuilder.Append(' ');
            }
            for (int i = 0; i < marginLeft; i++) stringBuilder.Append(' ');
            for (int i = 0; i <= marginLeft; i++) stringBuilder.Append(' ');

            for (int x = 0; x < joueur.PlateauAdverse.nbColonnes; x++)
            {
                stringBuilder.Append(Utils.GetMarkupForChar(Utils.GetCharOfCase(joueur.PlateauAdverse[x, ligne])));
                stringBuilder.Append(' ');
            }
            stringBuilder.Append(numeroDeLigne);
            for (int i = 0; i < marginLeft - Utils.NbDigits(numeroDeLigne); i++) stringBuilder.Append(' ');
            AnsiConsole.Write(new Markup(stringBuilder.ToString()).Centered());
        }
        public static void AfficherPlateau(Joueur joueur)
        {
            int marginLeft = Utils.NbDigits(joueur.Plateau.nbLignes);
            AfficherLettresColonne(marginLeft, joueur.Plateau.nbColonnes);
            for (int i = 0; i < joueur.Plateau.nbLignes; i++)
                AfficherLignePlateau(marginLeft, joueur, i);
            AfficherLettresColonne(marginLeft, joueur.Plateau.nbColonnes);
            AnsiConsole.WriteLine(" ");
        }
        public static void Exit()
        {
            AnsiConsole.Write("Merci d'avoir jouer !\nAppuyer sur n'importe quelle touche pour quitter..."); Console.ReadKey(true);
        }
        public static int SelectColonne(Joueur joueur, bool fire = true, int limit = 0)
        {
            int index_colonne;
            SelectionPrompt<int> prompt_colonne = new SelectionPrompt<int>()
                                            .Title(fire ? "Dans quelle colonne voulez-vous tirer ?" : "À partir de quelle colonne voulez-vous le placer ? (Sens de gauche à droite ou de haut en bas)")
                                            .AddChoices(Enumerable.Range(0, joueur.Plateau.nbColonnes - limit));
            prompt_colonne.Converter = i => GameEngine.GetColonne(i).ToString();
            index_colonne = AnsiConsole.Prompt(prompt_colonne);
            return index_colonne;
        }
        public static int SelectLigne(Joueur joueur, bool fire = true, int limit = 0)
        {
            int index_ligne;
            SelectionPrompt<int> prompt_ligne = new SelectionPrompt<int>()
                                            .Title(fire ? "Dans quelle ligne voulez-vous tirer ?" : "À partir de quelle ligne voulez-vous le placer ? (Sens de gauche à droite ou de haut en bas)")
                                            .AddChoices(Enumerable.Range(0, joueur.Plateau.nbLignes - limit));
            prompt_ligne.Converter = i => GameEngine.GetLigne(i).ToString();
            index_ligne = AnsiConsole.Prompt(prompt_ligne);
            return index_ligne;
        }
        public static void WriteLine(string text)
        {
            AnsiConsole.Write(new Text(text).Centered());
        }
    }
}
