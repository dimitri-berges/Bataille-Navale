using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GameCore;

namespace InterfaceWPF
{
    /// <summary>
    /// Logique d'interaction pour InitGame.xaml
    /// </summary>
    /// 
    public partial class InitGame : Window
    {
        GameEngine gameEngine;

        private Button[,] grid;
        public InitGame()
        {
            InitializeComponent();

            try
            {
                this.gameEngine = GameEngine.FromJSON(APIManager.APICaller.GetJSON());
            }
            catch (Exception)
            {
                throw;
            }

            grid = new Button[gameEngine.nbColonnes, gameEngine.nbLignes];
            for (int i = 0; i < gameEngine.nbColonnes; i++)
            {
                for (int j = 0; j < gameEngine.nbLignes; j++)
                {
                    grid[i,j] = new Button()
                    {
                        CommandParameter = new int[2] { i, j },
                    };
                    grid[i, j].Click += Grid_Click;
                }
            }

        }


        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
