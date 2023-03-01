﻿using System;
using System.Text.Json;

namespace GameCore
{
    public class GameEngine
    {
        public int nbLignes { get; set; }
        public int nbColonnes { get; set; }
        public Bateau[] bateaux { get; set; }

        public static GameEngine FromJSON(string json)
        {
            return JsonSerializer.Deserialize<GameEngine>(json);
        }
    }
}
