using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Diagnostics;

namespace ProjetLabyrintheWPF
{
    public partial class MainWindow : Window
    {   
        private Maze maze = null;

        private int mazeSizeX = 0;
        private int mazeSizeY = 0;
        private int mazeCellLength = 0;

        private TextBox textBoxSizeX = null;
        private TextBox textBoxSizeY = null;
        private TextBox textBoxLength = null;

        private bool pathOfTheMazeDisplayed = false;

        private string assetsPath = AppDomain.CurrentDomain.BaseDirectory + "assets\\";

        private Stopwatch stopWatch = null;
        private Drawing draw = null;
        private Music music = null;

        public MainWindow()
        {   
            InitializeComponent();
            draw = new Drawing();
            music = new Music();
            stopWatch = new Stopwatch();
            music.PlayBGM(assetsPath + "music.wav");
        }

        private void WindowKeyDown(object sender, KeyEventArgs eKey)
        {   
            if (!pathOfTheMazeDisplayed && maze != null && (eKey.Key == Key.W || eKey.Key == Key.A || eKey.Key == Key.S || eKey.Key == Key.D)) //maze != null to prevent user from using key input before the maze was created
            {
                if (eKey.Key == Key.W)
                    maze.YouMove('N');

                if (eKey.Key == Key.D)
                    maze.YouMove('E');

                if (eKey.Key == Key.S)
                    maze.YouMove('S');

                if (eKey.Key == Key.A)
                    maze.YouMove('W');

                DisplayMaze2D(pathOfTheMazeDisplayed); //pathOfTheMazeDisplayed is false
                if (maze.AreYouOnTheExit())
                    YouWin();
            }      
        }

        private void TextBoxSizeX(object sender, TextChangedEventArgs e)
        {
            textBoxSizeX = sender as TextBox;       
        }
        private void TextBoxSizeY(object sender, TextChangedEventArgs e)
        {
            textBoxSizeY = sender as TextBox;       
        }
        private void TextBoxLength(object sender, TextChangedEventArgs e)
        {
            textBoxLength = sender as TextBox;
        }

        private void CreateTheMaze()
        {
            maze = new Maze(mazeSizeX, mazeSizeY);
        }

        private void ButtonClickDisplayAnswer(object sender, RoutedEventArgs e)
        {
            if (!pathOfTheMazeDisplayed)
            {   
                pathOfTheMazeDisplayed = true;
                DisplayMaze2D(true); //Only solution
            }
        }
        private void ButtonGenerateMaze(object sender, RoutedEventArgs e)
        {   
            if (TextBoxInputIsNumeric() && ManageToPutTextBoxInputIntoVariables())
            {
                IntegerInputSecurity();
                CreateTheMaze();
                pathOfTheMazeDisplayed = false;
                DisplayMaze2D(pathOfTheMazeDisplayed);
                maze.PrepareYourMove();
                StartChrono();
            }
        }

        private void ButtonClickToggleMusic(object sender, RoutedEventArgs e)
        {
            if (music.IsPlaying())
            {
                music.StopBGM();
                buttonToggleMusic.Content = "Enable music";
            }
            else
            {
                music.PlayBGM(assetsPath + "music.wav");
                buttonToggleMusic.Content = "Disable music";
            }   
        }

        private bool TextBoxInputIsNumeric()
        {
            if (IsNumeric(textBoxSizeX.Text) &&
                IsNumeric(textBoxSizeY.Text) &&
                IsNumeric(textBoxLength.Text))
                return true;
            else
                return false;
        }

        private void IntegerInputSecurity()
        {
            if (mazeSizeX < 2 || mazeSizeX > 300 ||
                mazeSizeY < 2 || mazeSizeY > 300 ||
                mazeCellLength < 2 || mazeCellLength > 50) {   
                //Arbitrary default values
                mazeSizeX = 32;
                mazeSizeY = 32;
                mazeCellLength = 16;
            }
        }

        private bool ManageToPutTextBoxInputIntoVariables()
        {
            if (!String.IsNullOrWhiteSpace(textBoxSizeX.Text) && textBoxSizeX.Text.Length <= 3 &&
                !String.IsNullOrWhiteSpace(textBoxSizeY.Text) && textBoxSizeY.Text.Length <= 3 &&
                !String.IsNullOrWhiteSpace(textBoxLength.Text) && textBoxLength.Text.Length <= 3)
            {
                mazeSizeX = Convert.ToInt32(textBoxSizeX.Text);
                mazeSizeY = Convert.ToInt32(textBoxSizeY.Text);
                mazeCellLength = Convert.ToInt32(textBoxLength.Text);
                return true;
            }
            else
                return false;
        }

        private void YouWin()
        {
            StopChrono();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}h {1:00}m {2:00}s", ts.Hours, ts.Minutes, ts.Seconds);

            draw.DrawText(mazeSizeX * mazeCellLength + 30, 0, "BRAVO!! ", 50, Colors.Red, LayoutRoot);
            draw.DrawText(mazeSizeX * mazeCellLength + 30, 60, "Time: " + elapsedTime + ". Your moves: " + maze.NbOfYourMoves + ". Minimum: " + maze.NbPathCell + ".", 15, Colors.Black, LayoutRoot);
            pathOfTheMazeDisplayed = true;
            DisplayMaze2D(pathOfTheMazeDisplayed);
        }

        private void DisplayMaze2D(bool displaySolution)
        {   
            if (!displaySolution) 
                LayoutRoot.Children.Clear(); //Clear the GUI

            for (int i = 0; i < mazeSizeX; i++) {
                for (int j = 0; j < mazeSizeY; j++) {   
                    if (displaySolution) {
                        DisplayMaze2DCellContent(i, j, mazeCellLength);
                        DisplayMaze2DCellStart(i, j, mazeCellLength);
                        DisplayMaze2DCellEnd(i, j, mazeCellLength);
                    }
                    else {
                        DisplayMaze2DCellStart(i, j, mazeCellLength);
                        DisplayMaze2DCellEnd(i, j, mazeCellLength);
                        DisplayMaze2DCellYouAreHere(i, j, mazeCellLength);
                        DisplayMaze2DWall(i, j, mazeCellLength);
                    }          
                }
            }
        }

        private void DisplayMaze2DCellYouAreHere(int posX, int posY, int length) {
            if (maze.Get2DCellYouAreHere(posX, posY))
                draw.DrawSquare(posX * length + 1, posY * length + 1, length - 2, Colors.BlueViolet, LayoutRoot);
        }

        private void DisplayMaze2DCellEnd(int posX, int posY, int length) {
            if (maze.Get2DCellEnd(posX, posY))
                draw.DrawSquare(posX * length + 1, posY * length + 1, length - 2, Colors.Orange, LayoutRoot);
        }

        private void DisplayMaze2DCellStart(int posX, int posY, int length) {
            if (maze.Get2DCellStart(posX, posY))
                draw.DrawSquare(posX * length + 1, posY * length + 1, length - 2, Colors.Red, LayoutRoot);
        }

        private void DisplayMaze2DCellContent(int posX, int posY, int length)
        {
            char direction = char.ToUpper(maze.Get2DCellDirection(posX, posY));
            if (direction == 'N' || direction == 'E' || direction == 'S' || direction == 'W')
                draw.DrawImage(new Uri(assetsPath + direction + ".png", UriKind.Absolute), posX * length + 1, posY * length + 1, length - 2, LayoutRoot);
        }

        private void DisplayMaze2DWall(int posX, int posY, int length)
        {
            if (maze.Get2DCellWall(posX, posY, 'N')) //North cell line
                draw.DrawLine(posX * length, posY * length, (posX * length) + length, posY * length, 2, Colors.Black, LayoutRoot); 
                      
            if (maze.Get2DCellWall(posX, posY, 'W')) //West cell line
                draw.DrawLine(posX * length, posY * length, posX * length, (posY * length) + length, 2, Colors.Black, LayoutRoot);

            if (posY == mazeSizeY - 1)  // Display only south line for last cells
                draw.DrawLine(posX * length, (posY * length) + length, (posX * length) + length, (posY * length) + length, 2, Colors.Black, LayoutRoot);
                  
            if (posX == mazeSizeX - 1) //Display only east line for last cells
                draw.DrawLine((posX * length) + length, posY * length, (posX * length) + length, (posY * length) + length, 2, Colors.Black, LayoutRoot);
        }    

        private bool IsNumeric(object objectToTest)
        {
            double retNum;
            return Double.TryParse(Convert.ToString(objectToTest), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        }

        private void StartChrono()
        {
            stopWatch.Start();
        }

        private void StopChrono()
        {
            stopWatch.Stop();
        }
    }
}
