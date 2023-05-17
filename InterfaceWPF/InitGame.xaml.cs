using GameCore;
using System;
using System.Windows;
using System.Windows.Controls;

namespace InterfaceWPF
{
    /// <summary>
    /// Logique d'interaction pour InitGame.xaml
    /// </summary>
    /// 
    public partial class InitGame : Window
    {
        GameEngine gameEngine;

        private Button[,] gridOther, gridPlayer;
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

            gridOther = new Button[gameEngine.nbColonnes, gameEngine.nbLignes];
            gridPlayer = new Button[gameEngine.nbColonnes, gameEngine.nbLignes];
            for (int i = 0; i < gameEngine.nbColonnes; i++)
            {
                for (int j = 0; j < gameEngine.nbLignes; j++)
                {
                    gridOther[i, j] = new Button()
                    {
                        CommandParameter = new int[3] { i, j, 1 },
                    };
                    gridPlayer[i, j] = new Button()
                    {
                        CommandParameter = new int[3] { i, j, 0 },
                    };
                    gridOther[i, j].Click += Grid_Click;
                    gridPlayer[i, j].Click += Grid_Click;
                }
            }

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double height = this.Height / gameEngine.nbLignes - 1, width = (this.Width / 2) / gameEngine.nbColonnes - 1;
            double size = Math.Min(height, width);
            for (int i = 0; i < gameEngine.nbColonnes; i++)
                for (int j = 0; j < gameEngine.nbLignes; j++)
                {
                    gridOther[i, j].Width = gridOther[i, j].Height = size;
                    gridPlayer[i, j].Width = gridPlayer[i, j].Height = size;
                }


        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            double height = this.Height / gameEngine.nbLignes - 1, width = (this.Width / 2) / gameEngine.nbColonnes - 1;
            double size = Math.Min(height, width);
            InitGrid(size, GridOther, this.gridOther);
            InitGrid(size, GridPlayer, this.gridPlayer);
        }

        private void InitGrid(double size, Grid gridUI, Button[,] grid)
        {
            for (int i = 0; i < gameEngine.nbLignes; i++)
            {
                gridUI.RowDefinitions.Add(new());
            }

            for (int i = 0; i < gameEngine.nbColonnes; i++)
            {
                gridUI.ColumnDefinitions.Add(new());
                for (int j = 0; j < gameEngine.nbLignes; j++)
                {
                    gridUI.Children.Add(grid[i, j]);
                    Grid.SetColumn(grid[i, j], i);
                    Grid.SetRow(grid[i, j], j);
                    grid[i, j].Content = GameEngine.GetCoords(i, j);
                    grid[i, j].HorizontalContentAlignment = HorizontalAlignment.Center;
                    grid[i, j].VerticalContentAlignment = VerticalAlignment.Center;
                    grid[i, j].Padding = new Thickness(0);
                    grid[i, j].Width = grid[i, j].Height = size;
                }
            }
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            int[] tab = (int[])((Button)sender).CommandParameter;
            int i = tab[0], j = tab[1];
            string grille = tab[2] == 0 ? "Grille Joueur" : "Grille Adverse";
            MessageBox.Show($"Bouton {GameEngine.GetCoords(i, j)},{grille}");
        }
    }
}
