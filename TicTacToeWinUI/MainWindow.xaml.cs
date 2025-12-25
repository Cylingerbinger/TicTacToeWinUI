using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TicTacToeWinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Variables
        char turn = 'x';
        char[,] board = new char[3, 3];

        // When button is pressed
        private void ChangeSymbol(Object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Button Pressed");

            Button clickedButton = (Button)sender;
            // If already used, ignore click (prevents accidental turn switch)
            if (!clickedButton.IsEnabled)
                return;


            // Switch turn
            ChooseTurn(clickedButton);
        }

        // Change turn and apply changes to grid content
        private async Task ChooseTurn(Button clickedButton)
        {
            // Read the button's Tag ("row,col") and convert it to board indices 
            string tag = clickedButton.Tag.ToString();
            string[] parts = tag.Split(',');
            int row = int.Parse(parts[0]);
            int col = int.Parse(parts[1]);
            
            // Apply changes to grid and switch turns
            if (turn == 'x')
            {
                clickedButton.Content = "X";
                board[row, col] = turn;
            }
            else
            {
                clickedButton.Content = "O";
                board[row, col] = turn;
            }

            // Disable after a valid move so it can't be changed
            clickedButton.IsEnabled = false;

            // Check for win condition if met
            if (CheckWin(turn))
            {
                System.Diagnostics.Debug.WriteLine(turn + " won the game.");

                // Disable all remaining buttons on the board
                foreach (var child in TicTacToeGrid.Children)
                {
                    if (child is Button btn)
                        btn.IsEnabled = false;
                }

                ResultText.Text = turn + " won the game!";
                await RestartGame(); // Call restart game method

                return;
            }
            if (IsDraw())
            {
                System.Diagnostics.Debug.WriteLine("Game is a draw.");

                ResultText.Text = "It's a draw!"; // Set ContentDialog message
                await RestartGame(); // Call restart game method

                return;
            }

                // Switch turn
                turn = (turn == 'x') ? 'o' : 'x';
        }

        // Check if win condtions are met (3 in a row achieved)
        private bool CheckWin(char w)
        {
            // Check for 3 in a row (horizontally)
            for (int i = 0; i < board.GetLength(0); i++) // check each grid and get length of rows(0)
            {
                if (board[i,0] == w && board[i,1] == w && board[i,2] == w)
                {
                    return true;
                }
            }

            // Check for 3 in a column (vertically)
            for (int i = 0; i < board.GetLength(1); i++) // check each grid and get length of columns (1)
            {
                if (board[0,i] == w && board[1, i] == w && board[2,i] == w)
                {
                    return true; 
                }
            }

            // Check for 3 diagonally (from top left to bottom right)
            if (board[0, 0] == w && board[1, 1] == w && board[2, 2] == w) 
            {
                return true;
            }
            // Check for 3 diagonally (from top right to bottom left)
            if (board[0,2] == w && board[1,1] == w && board[2,0] == w)
            {
                return true;
            }

            // If no win condtions met return false
            return false;
        }

        // Check if game is a draw
        private bool IsDraw()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0;  j < board.GetLength(1); j++)
                {
                    if (board[i, j] == '\0')
                    {
                        return false; // Moves still remaining
                    } 
                }
            }
            
            return true; // board is filled, no clear winner
        }

        // Restart game method
        private async Task RestartGame()
        {
            
            
            var result = await RestartDialog.ShowAsync();

            if (result != ContentDialogResult.Primary)
            {
                // No pressed, then exit app
                Microsoft.UI.Xaml.Application.Current.Exit();
                return; 
            }


            // YES pressed, then reset state
            turn = 'x';

            // Clear board array
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    board[r, c] = '\0';

            // Clear + re-enable all buttons in the grid
            foreach (var child in TicTacToeGrid.Children)
            {
                if (child is Button btn)
                {
                    btn.Content = "";      // or null
                    btn.IsEnabled = true;
                }
            }

        }
    }
}
