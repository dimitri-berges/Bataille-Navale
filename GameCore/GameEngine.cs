using System;
using System.Text.Json;

namespace GameCore
{
    public class GameEngine
    {
        public int nbLignes { get; set; }
        public int nbColonnes { get; set; }
        public Bateau[] bateaux { get; set; }
        public Board Board { get; set; }

        public static GameEngine FromJSON(string json)
        {
            GameEngine gameEngine = JsonSerializer.Deserialize<GameEngine>(json);
            gameEngine.Board = new(gameEngine.nbLignes, gameEngine.nbColonnes);
            return gameEngine;
        }
    }
}
