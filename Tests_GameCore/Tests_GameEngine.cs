using GameCore;
using System;
using Xunit;

namespace Tests_GameCore
{
    public class Tests_GameEngine
    {
        [Fact]
        public void TestConstructorFromJSON()
        {
            GameEngine gameEngine = GameEngine.FromJSON("{\r\n  \"nbLignes\": 10,\r\n  \"nbColonnes\": 10,\r\n  \"bateaux\": [\r\n    {\r\n      \"taille\": 5,\r\n      \"nom\": \"Porte-avions\"\r\n    },\r\n    {\r\n      \"taille\": 4,\r\n      \"nom\": \"Croiseur\"\r\n    },\r\n    {\r\n      \"taille\": 3,\r\n      \"nom\": \"Contre-torpilleur\"\r\n    },\r\n    {\r\n      \"taille\": 3,\r\n      \"nom\": \"Sous-marin\"\r\n    },\r\n    {\r\n      \"taille\": 2,\r\n      \"nom\": \"Torpilleur\"\r\n    }\r\n  ]\r\n}");
            Assert.NotNull(gameEngine);
            Assert.IsType<GameEngine>(gameEngine);
            Assert.Equal(10, gameEngine.nbLignes);
            Assert.Equal(10, gameEngine.nbColonnes);
            Assert.Equal(5, gameEngine.bateaux.Length);
        }

        [Fact]
        public void TestJouer()
        {
            GameEngine gameEngine = GameEngine.FromJSON("{\r\n  \"nbLignes\": 10,\r\n  \"nbColonnes\": 10,\r\n  \"bateaux\": [\r\n    {\r\n      \"taille\": 5,\r\n      \"nom\": \"Porte-avions\"\r\n    },\r\n    {\r\n      \"taille\": 4,\r\n      \"nom\": \"Croiseur\"\r\n    },\r\n    {\r\n      \"taille\": 3,\r\n      \"nom\": \"Contre-torpilleur\"\r\n    },\r\n    {\r\n      \"taille\": 3,\r\n      \"nom\": \"Sous-marin\"\r\n    },\r\n    {\r\n      \"taille\": 2,\r\n      \"nom\": \"Torpilleur\"\r\n    }\r\n  ]\r\n}");

            Assert.Throws<Exception>(() => { gameEngine.Jouer(5, 5); });

            gameEngine.JoueurActuel.PlacerBateau(0, 0, true, 0);
            gameEngine.JoueurActuel.PlacerBateau(2, 0, true, 0);
            gameEngine.JoueurActuel.PlacerBateau(4, 0, true, 0);
            gameEngine.JoueurActuel.PlacerBateau(6, 0, true, 0);
            gameEngine.JoueurActuel.PlacerBateau(8, 0, true, 0);
            gameEngine.FinDeTour();
            gameEngine.JoueurActuel.PlacerBateau(0, 0, true, 0);
            gameEngine.JoueurActuel.PlacerBateau(2, 0, true, 0);
            gameEngine.JoueurActuel.PlacerBateau(4, 0, true, 0);
            gameEngine.JoueurActuel.PlacerBateau(6, 0, true, 0);
            gameEngine.JoueurActuel.PlacerBateau(8, 0, true, 0);
            gameEngine.FinDeTour();

            Assert.Equal(HitResponse.Hit, gameEngine.Jouer(8, 0));
            Assert.Equal(CaseStatut.BoatHit, gameEngine.JoueurAdverse.Plateau[8, 0].Statut);
            Assert.Equal(CaseStatut.BoatHit, gameEngine.JoueurActuel.PlateauAdverse[8, 0].Statut);

            Assert.Equal(HitResponse.Missed, gameEngine.Jouer(9, 0));
            Assert.Equal(CaseStatut.WaterHit, gameEngine.JoueurAdverse.Plateau[9, 0].Statut);
            Assert.Equal(CaseStatut.WaterHit, gameEngine.JoueurActuel.PlateauAdverse[9, 0].Statut);

            Assert.Equal(HitResponse.HitAndDrowned, gameEngine.Jouer(8, 1));
            Assert.Equal(CaseStatut.BoatDrowned, gameEngine.JoueurAdverse.Plateau[8, 1].Statut);
            Assert.Equal(CaseStatut.BoatDrowned, gameEngine.JoueurAdverse.Plateau[8, 0].Statut);
            Assert.Equal(CaseStatut.BoatDrowned, gameEngine.JoueurActuel.PlateauAdverse[8, 1].Statut);
            Assert.Equal(CaseStatut.BoatDrowned, gameEngine.JoueurActuel.PlateauAdverse[8, 0].Statut);

        }

        [Fact]
        public void TestFinDeTour()
        {
            GameEngine gameEngine = GameEngine.FromJSON("{\r\n  \"nbLignes\": 10,\r\n  \"nbColonnes\": 10,\r\n  \"bateaux\": [\r\n    {\r\n      \"taille\": 5,\r\n      \"nom\": \"Porte-avions\"\r\n    },\r\n    {\r\n      \"taille\": 4,\r\n      \"nom\": \"Croiseur\"\r\n    },\r\n    {\r\n      \"taille\": 3,\r\n      \"nom\": \"Contre-torpilleur\"\r\n    },\r\n    {\r\n      \"taille\": 3,\r\n      \"nom\": \"Sous-marin\"\r\n    },\r\n    {\r\n      \"taille\": 2,\r\n      \"nom\": \"Torpilleur\"\r\n    }\r\n  ]\r\n}");
            Joueur j1 = gameEngine.JoueurActuel;
            gameEngine.FinDeTour();
            Joueur j2 = gameEngine.JoueurActuel;
            Assert.NotNull(j1);
            Assert.NotNull(j2);
            Assert.NotEqual(j1, j2);
        }

        [Fact]
        public void TestStatic() {
            Assert.Equal('A', GameEngine.GetColonne(0));
            Assert.Equal('N', GameEngine.GetColonne(13));
            Assert.Equal(1, GameEngine.GetLigne(0));
            Assert.Equal(14, GameEngine.GetLigne(13));

            Assert.Equal(0, GameEngine.GetIndexOfColonne('A'));
            Assert.Equal(13, GameEngine.GetIndexOfColonne('N'));
            Assert.Equal(0, GameEngine.GetIndexOfLigne(1));
            Assert.Equal(13, GameEngine.GetIndexOfLigne(14));

            Assert.Equal("A1", GameEngine.GetCoords(0, 0));
            Assert.Equal("I5", GameEngine.GetCoords(8, 4));
            Assert.Equal("E7", GameEngine.GetCoords(4, 6));

        }
    }
}
