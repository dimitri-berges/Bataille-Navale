using GameCore;
using System;
using Xunit;

namespace Tests_GameCore
{
    public class Tests_GameEngine
    {
        [Fact]
        public void Test_GameEngineConstructorFromJSON()
        {
            GameEngine gameEngine = GameEngine.FromJSON("{\r\n  \"nbLignes\": 10,\r\n  \"nbColonnes\": 10,\r\n  \"bateaux\": [\r\n    {\r\n      \"taille\": 5,\r\n      \"nom\": \"Porte-avions\"\r\n    },\r\n    {\r\n      \"taille\": 4,\r\n      \"nom\": \"Croiseur\"\r\n    },\r\n    {\r\n      \"taille\": 3,\r\n      \"nom\": \"Contre-torpilleur\"\r\n    },\r\n    {\r\n      \"taille\": 3,\r\n      \"nom\": \"Sous-marin\"\r\n    },\r\n    {\r\n      \"taille\": 2,\r\n      \"nom\": \"Torpilleur\"\r\n    }\r\n  ]\r\n}");
            Assert.NotNull(gameEngine);
            Assert.IsType<GameEngine>(gameEngine);
            Assert.Equal(10, gameEngine.nbLignes);
            Assert.Equal(10, gameEngine.nbColonnes);
            Assert.Equal(5, gameEngine.bateaux.Length);
            Assert.Equal(10, gameEngine.Board.Cases.GetLength(0));
            Assert.Equal(10, gameEngine.Board.Cases.GetLength(1));
        }
    }
}
