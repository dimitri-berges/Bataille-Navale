using System;
using System.Linq;
using System.Text.Json;

namespace GameCore
{
    public class GameEngine
    {
        public int nbLignes { get; set; }
        public int nbColonnes { get; set; }
        public Bateau[] bateaux { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player[] Players { get; set; }

        public static GameEngine FromJSON(string json)
        {
            return JsonSerializer.Deserialize<GameEngine>(json).Initialize();
        }

        private GameEngine Initialize()
        {
            Player1 = new(new(nbLignes, nbColonnes), bateaux.ToArray());
            Player2 = new(new(nbLignes, nbColonnes), bateaux.ToArray());
            Players = new Player[] { Player1, Player2 };
            return this;
        }
    }
}
