namespace ProjetLabyrintheWPF
{
    class Cellule
    {
        public int PosX { get; }
        public int PosY { get; }

        private char direction;
        public char Direction {
            get { return this.direction;}
            set {
                char temp = char.ToUpper(value);
                if (temp == 'N' || temp == 'E' || temp == 'S' || temp == 'W' || temp == '0') { //0 = null direction
                    this.direction = temp;
                }    
            }
        }

        private bool wallNorth;
        private bool wallEast;
        private bool wallSouth;
        private bool wallWest;

        public bool WallNorth { get {return this.wallNorth;} }
        public bool WallEast { get {return this.wallEast;} }
        public bool WallSouth { get {return this.wallSouth;} }
        public bool WallWest { get {return this.wallWest;} }
        public bool StartCell { get; set; }
        public bool EndCell { get; set; }
        public bool Visited { get; set; }
        public bool YouAreHere { get; set; }

        public Cellule(int posX, int posY)
        {
            this.PosX = posX;
            this.PosY = posY;
            wallNorth = true;
            wallEast = true;
            wallSouth = true;
            wallWest = true;
            StartCell = false;
            EndCell = false;
            Visited = false;
            YouAreHere = false;
            Direction = '0';
        }

        public bool GetWall(char direction)
        {
            direction = char.ToUpper(direction);
            if (direction == 'N')
                return this.wallNorth;
            else if (direction == 'E')
                return this.wallEast;
            else if (direction == 'S')
                return this.wallSouth;
            else if (direction == 'W')
                return this.wallWest;
            else return false;
        }

        public void DestroyWall(char direction)
        {
            direction = char.ToUpper(direction);
            if (direction == 'N' && wallNorth)
                wallNorth = false;
            else if (direction == 'E' && wallEast)
                wallEast = false;
            else if (direction == 'S' && wallSouth)
                wallSouth = false;
            else if (direction == 'W' && wallWest)
                wallWest = false;
        }
    }
}
