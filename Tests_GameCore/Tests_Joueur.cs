using GameCore;
using System;
using Xunit;

namespace Tests_GameCore
{
    public class Tests_Joueur
    {
        [Fact]
        public void TestPlacerBateau()
        {
            GameEngine gameEngine = GameEngine.FromJSON("{\r\n  \"nbLignes\": 10,\r\n  \"nbColonnes\": 10,\r\n  \"bateaux\": [\r\n    {\r\n      \"taille\": 5,\r\n      \"nom\": \"Porte-avions\"\r\n    },\r\n    {\r\n      \"taille\": 4,\r\n      \"nom\": \"Croiseur\"\r\n    },\r\n    {\r\n      \"taille\": 3,\r\n      \"nom\": \"Contre-torpilleur\"\r\n    },\r\n    {\r\n      \"taille\": 3,\r\n      \"nom\": \"Sous-marin\"\r\n    },\r\n    {\r\n      \"taille\": 2,\r\n      \"nom\": \"Torpilleur\"\r\n    }\r\n  ]\r\n}");

            Joueur joueur = gameEngine.JoueurActuel;

            Assert.True(joueur.PlacerBateau(0, 0, true, 0));

            Assert.False(joueur.PlacerBateau(1,0,false,0));

            Assert.Equal(CaseStatut.Boat, joueur.Plateau[0,0].Statut);
            Assert.Equal(CaseStatut.Boat, joueur.Plateau[0,4].Statut);
            Assert.Equal(CaseStatut.Water, joueur.Plateau[0,5].Statut);
            Assert.Equal(CaseStatut.Water, joueur.Plateau[1,0].Statut);

        }
    }
}
