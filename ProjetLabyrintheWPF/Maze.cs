using System;
using System.Collections;

namespace ProjetLabyrintheWPF
{
    class Maze
    {
        private Cell[,] maze2D = null;

        private char[] tableOfDirection = null;

        private bool pathExistsBetweenCells = false;

        private Stack cellStack = null;

        private Random random = null;

        private Cell currentCell = null;
        private Cell startCell = null;

        private int nbTotalCell = 0;  
        private int nbVisitedCell = 0;

        private int mazeSizeX = 0;
        private int mazeSizeY = 0;

        private int nbPathCell = 0;
        public int NbPathCell { get { return this.nbPathCell; } }

        private int nbOfYourMoves = 0;
        public int NbOfYourMoves { get { return this.nbOfYourMoves; } }

        public Maze(int sizeX, int sizeY)
        {
            CreateAndInitialize2DMaze(sizeX, sizeY);
            SetStartAndEndCellsPosition();
            RandomizeMaze();
            CalculateThePath();
        }
        private void CreateAndInitialize2DMaze(int SizeX, int SizeY)
        {
            random = new Random();
            cellStack = new Stack();
            mazeSizeX = SizeX;
            mazeSizeY = SizeY;
            nbTotalCell = SizeX * SizeY;
            nbVisitedCell = 0;
            nbPathCell = 0;
            nbOfYourMoves = 0;
            tableOfDirection = new char[4] { 'N', 'E', 'S', 'W' }; //Because it's a 2DMaze

            maze2D = new Cell[SizeX, SizeY];
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    maze2D[x, y] = new Cell(x, y);
                }
            }
        }

        private void SetStartAndEndCellsPosition()
        {
            int nbX, nbY;
            startCell = maze2D[random.Next(0, mazeSizeX), random.Next(0, mazeSizeY)];
            startCell.StartCell = true;
            do
            {
                nbX = random.Next(0, mazeSizeX);
                nbY = random.Next(0, mazeSizeY);
            }
            while (nbX == startCell.PosX && nbY == startCell.PosY);
            maze2D[nbX, nbY].EndCell = true;
        }

        private void RandomizeMaze()
        {
            currentCell = maze2D[random.Next(0, mazeSizeX), random.Next(0, mazeSizeY)];
            currentCell.Visited = true;
            nbVisitedCell = 1;
            while (nbVisitedCell < nbTotalCell)
            {
                pathExistsBetweenCells = false;
                RandomizationOfCharTable(tableOfDirection);
                for (int i = 0; i < 4 && !pathExistsBetweenCells; i++) //Loop to parse table of direction (N, E, S, W)
                {
                    if (CheckNeighbors(tableOfDirection[i], currentCell.PosX, currentCell.PosY, mazeSizeX, mazeSizeY))
                    {
                        currentCell.DestroyWall(tableOfDirection[i]);
                        cellStack.Push(currentCell);
                        currentCell = MoveToNewCell(tableOfDirection[i]);
                        currentCell.DestroyWall(GetOppositeDirection(tableOfDirection[i]));
                        currentCell.Visited = true;
                        nbVisitedCell++;
                        pathExistsBetweenCells = true;
                    }
                }
                if (!pathExistsBetweenCells) {
                    currentCell = (Cell)cellStack.Pop();
                }
            }
        }

        private void CalculateThePath()
        {
            cellStack.Clear();
            ReinitiateVistedCellToFalse();

            currentCell = startCell;
            currentCell.Visited = true;

            while (currentCell.EndCell == false)
            {
                pathExistsBetweenCells = false;
                for (int k = 0; k < 4 && !pathExistsBetweenCells; k++)
                {
                    if (CheckNeighbors(tableOfDirection[k], currentCell.PosX, currentCell.PosY, mazeSizeX, mazeSizeY) &&
                        !currentCell.GetWall(tableOfDirection[k])) //if neighbor exists, and wall destroyed
                    {
                        currentCell.Direction = tableOfDirection[k];
                        cellStack.Push(currentCell);
                        currentCell = MoveToNewCell(tableOfDirection[k]);
                        currentCell.Visited = true;
                        pathExistsBetweenCells = true;
                    }
                }
                if (!pathExistsBetweenCells)
                {
                    currentCell.Direction = '0';
                    currentCell = (Cell)cellStack.Pop();
                }
            }
            nbPathCell = cellStack.Count;
        }

        private bool CheckNeighbors(char c, int PosX, int PosY, int SizeX, int SizeY) //Check if neighbor exists, and never visited 
        {
            if (c == 'N' && PosY - 1 >= 0 && !maze2D[PosX, PosY - 1].Visited ||
                c == 'E' && PosX + 1 < SizeX && !maze2D[PosX + 1, PosY].Visited ||
                c == 'S' && PosY + 1 < SizeY && !maze2D[PosX, PosY + 1].Visited ||
                c == 'W' && PosX - 1 >= 0 && !maze2D[PosX - 1, PosY].Visited)
                return true;
            else
                return false;
        }

        private void RandomizationOfCharTable(char[] table)
        {
            int nbA, nbB;
            char temp;
            for (int i = 0; i < random.Next(1,table.Length+1); i++) {
                nbA = random.Next(0, table.Length);
                do
                    nbB = random.Next(0, table.Length);
                while (nbB == nbA);
                temp = table[nbA];
                table[nbA] = table[nbB];
                table[nbB] = temp;
            }
        }

        private Cell MoveToNewCell(char direction)
        {
            direction = char.ToUpper(direction);
            if (direction == 'N')
                return maze2D[currentCell.PosX, currentCell.PosY - 1];
            else if (direction == 'E')
                return maze2D[currentCell.PosX + 1, currentCell.PosY];
            else if (direction == 'S')
                return maze2D[currentCell.PosX, currentCell.PosY + 1];
            else if (direction == 'W')
                return maze2D[currentCell.PosX - 1, currentCell.PosY];
            else
                return currentCell;
        }

        private char GetOppositeDirection(char direction)
        {
            direction = char.ToUpper(direction);
            if (direction == 'N')
                return 'S';
            else if (direction == 'E')
                return 'W';
            else if (direction == 'S')
                return 'N';
            else if (direction == 'W')
                return 'E';
            else
                return direction;
        }

        private void ReinitiateVistedCellToFalse()
        {           
            foreach (Cell cell in maze2D)
                cell.Visited = false;
        }

        public bool Get2DCellWall(int x, int y, char c)
        {
            c = char.ToUpper(c);
            if (c == 'N')
                return maze2D[x, y].WallNorth;
            else if (c == 'E')
                return maze2D[x, y].WallEast;
            else if (c == 'S')
                return maze2D[x, y].WallSouth;
            else if (c == 'W')
                return maze2D[x, y].WallWest;
            else
                return false;
        }

        public bool Get2DCellStart(int x, int y) {
            return maze2D[x, y].StartCell;
        }
        public bool Get2DCellEnd(int x, int y) {
            return maze2D[x, y].EndCell;
        }
        public bool Get2DCellYouAreHere(int x, int y) {
            return maze2D[x, y].YouAreHere;
        }
        public char Get2DCellDirection(int x, int y) {
            return maze2D[x, y].Direction;
        }

        public void PrepareYourMove()
        {
            ReinitiateVistedCellToFalse(); //needed since CheckNeighbor() checks for non-visited cells
            currentCell = startCell;
            currentCell.YouAreHere = true;
        }
        public void YouMove(char direction)
        {
            if (CheckNeighbors(direction, currentCell.PosX, currentCell.PosY, mazeSizeX, mazeSizeY) 
                && !currentCell.GetWall(direction)) //If neighbor exists and wall destroyed
            {
                currentCell.YouAreHere = false;
                currentCell = MoveToNewCell(direction);
                currentCell.YouAreHere = true;
                nbOfYourMoves++;
            }
        }
        public bool AreYouOnTheExit() {
            return currentCell.EndCell;
        }
    }
}
