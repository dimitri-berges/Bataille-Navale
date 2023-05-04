using System.Linq;
using System.Text.Json;

namespace GameCore
{
    public class GameEngine
    {
        #region Variables d'initialisation
        public int nbLignes { get; set; }
        public int nbColonnes { get; set; }
        public Bateau[] bateaux { get; set; }
        public Joueur Joueur1 { get; set; }
        public Joueur Joueur2 { get; set; }
        #endregion

        #region Variables de jeu
        public Joueur[] Joueurs { get; set; }
        private int numeroJoueurActuel = 0;
        public int NumeroJoueurActuel
        {
            get { return numeroJoueurActuel; }
            private set
            {
                numeroJoueurActuel = value % Joueurs.Length;
            }
        }
        public Joueur JoueurActuel => Joueurs[NumeroJoueurActuel];
        public Joueur JoueurAdverse => Joueurs[((NumeroJoueurActuel + 1) % Joueurs.Length)];
        #endregion

        #region Initialisation
        public static GameEngine FromJSON(string json)
        {
            return JsonSerializer.Deserialize<GameEngine>(json).Initialize();
        }
        private GameEngine Initialize()
        {
            Joueur1 = new(new(nbLignes, nbColonnes), bateaux.ToList());
            Joueur2 = new(new(nbLignes, nbColonnes), bateaux.ToList());
            Joueurs = new Joueur[] { Joueur1, Joueur2 };
            return this;
        }
        #endregion

        #region Méthodes
        public HitResponse Jouer(int x, int y)
        {
            if (!(JoueurActuel.EstPretAJouer && JoueurAdverse.EstPretAJouer))
                throw new System.Exception("Au moins un joueur n'est pas prêt à jouer");
            return JoueurActuel.Tirer(x, y, JoueurAdverse);
        }
        public void FinDeTour()
        {
            NumeroJoueurActuel++;
        }
        #endregion
    }
}
