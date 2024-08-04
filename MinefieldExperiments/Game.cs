namespace MinefieldExperiments
{
    public class GameState
    {
        public GameState(int lives, int mines)
        {
            this.lives = lives;
            this.mines = mines;
            moves = 0;
            position = 0;
            gameOver = false;
        }

        public int lives { get; set; }

        public int mines { get; set; }

        public int moves { get; set; }

        public int position { get; set; }

        public bool gameOver { get; set; }
    }

    public class Game
    {
        private readonly IRandomIntegerGenerator _random;

        // for tracking what's on the board
        public const int CELL_EMPTY_NOT_VISITED = 0;
        public const int CELL_EMPTY_VISITED = 1;
        public const int CELL_PLAYER = 2;
        public const int CELL_MINE = 4;
        public const int CELL_GOAL = 8;


        int[] board;
        int boardTotalCells;
        int boardSize;

        private GameState gameState;

        public Game(IRandomIntegerGenerator random)
        {
            _random = random;
        }

        public void Help()
        {
            Console.WriteLine("You are on a {0}x{1} board. The goal is to move your player from position A1 (bottom left) to the top right position but be careful, mines are lurking everywhere", boardSize, boardSize);
            Console.WriteLine("\nThe following commands are available:");
            Console.WriteLine(" h = show instructions");
            Console.WriteLine(" l = move left");
            Console.WriteLine(" r = move right");
            Console.WriteLine(" u = move up");
            Console.WriteLine(" d = move down");
            Console.WriteLine(" p = print map");
            Console.WriteLine(" q = quit game\n");
        }

        public void ReadyGame(int boardSize = 8, int lives = 4, int mines = 8)
        {
            // check parameters make sense
            if (boardSize < 4 || boardSize > 26)
                throw new ArgumentException("Board size has to be between 4 and 26");

            this.boardSize = boardSize;
            boardTotalCells = boardSize * boardSize;

            if (lives < 0 || mines > (boardTotalCells - 2))
                throw new ArgumentException("Invalid values for lives or mines");

            board = new int[boardTotalCells];

            gameState = new GameState(lives, mines);

            InitBoard();
        }

        private void InitBoard()
        {
            // init board, initially everything is EMPTY_NOT_VISITED
            for (int i = 1; i < boardTotalCells; i++)
            {
                board[i] = CELL_EMPTY_NOT_VISITED;
            }

            // put player at A1
            board[gameState.position] = CELL_PLAYER;
            // put goal at H8
            board[boardTotalCells - 1] = CELL_GOAL;

            // now place mines randomly
            int minesPlaced = 0;
            while (minesPlaced < gameState.mines)
            {
                // mine can not be on A1 or H8 so only 62 posible values
                int minePosition = _random.NextValue(1, boardTotalCells - 1);
                if (board[minePosition] == CELL_EMPTY_NOT_VISITED)
                {
                    board[minePosition] = CELL_MINE;
                    minesPlaced++;
                }
            }
        }

        public GameState Play()
        {
            while (!gameState.gameOver)
            {
                Console.Write("> ");
                char command = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (command == 'q')
                    return gameState;  // quit game, ideally we would ask to confirm

                ProcessCommand(command);

                Console.WriteLine();
                Console.WriteLine(GetPrintableStatus());
                Console.WriteLine();
            }

            return gameState;
        }

        public void ProcessCommand(char command)
        {
            int previousPlayerPosition = gameState.position;
            switch (command)
            {
                case 'h':
                    Help();
                    break;
                case 'p':
                    PrintBoard();
                    break;
                case 'l':
                    MoveLeft();
                    break;
                case 'r':
                    MoveRight();
                    break;
                case 'u':
                    MoveUp();
                    break;
                case 'd':
                    MoveDown();
                    break;
                default:
                    Console.WriteLine("Unknown command. Use h to see supported commands.");
                    break;
            }

            // mark visited cells and check for mines
            if (gameState.position != previousPlayerPosition)
            {
                gameState.moves++;
                board[previousPlayerPosition] = CELL_EMPTY_VISITED;
                if (board[gameState.position] == CELL_MINE)
                {
                    gameState.lives--;
                    Console.WriteLine("BOOM! Unfortunately you hit a mine :(");
                }
                board[gameState.position] = CELL_PLAYER;
            }

            gameState.gameOver = (gameState.position == (boardTotalCells - 1)) || (gameState.lives == 0);
        }


        private void MoveLeft()
        {
            if (gameState.position % boardSize > 0)
            {
                gameState.position = gameState.position - 1;
            }
            else
            {
                Console.WriteLine("Can't move further left!");
            }
        }

        private void MoveRight()
        {
            if (gameState.position % boardSize < (boardSize - 1))
            {
                gameState.position = gameState.position + 1;
            }
            else
            {
                Console.WriteLine("Can't move further right!");
            }
        }

        private void MoveUp()
        {
            if (gameState.position / boardSize < (boardSize - 1))
            {
                gameState.position = gameState.position + boardSize;
            }
            else
            {
                Console.WriteLine("Can't move further up!");
            }
        }

        private void MoveDown()
        {
            if (gameState.position / boardSize > 0)
            {
                gameState.position = gameState.position - boardSize;
            }
            else
            {
                Console.WriteLine("Can't move further down!");
            }
        }

        public GameState GetGameState()
        {
            return gameState;
        }

        public int GetBoardContent(int row, int col)
        {
            int position = row * boardSize + col;
            if (position >= 0 && position < boardTotalCells)
            {
                return board[position];
            }

            throw new ArgumentOutOfRangeException();
        }

        private string GetPrintablePlayerPosition()
        {
            char column = (char)((byte)'A' + (byte)(gameState.position % boardSize));
            int row = gameState.position / boardSize + 1;

            return column + row.ToString();
        }

        private void PrintBoard()
        {
            Console.Write(" ");
            for (int i = 0; i < boardSize; i++)
            {
                Console.Write('-');
            }
            Console.WriteLine();
            for (int row = (boardSize - 1); row >= 0; row--)
            {
                Console.Write("|");
                for (int col = 0; col < boardSize; col++)
                {
                    int index = row * boardSize + col;
                    Console.Write(BoardContentToSymbol(board[index]));
                }
                Console.Write("|");
                Console.WriteLine(row + 1);
            }
            Console.Write(" ");
            for (int i = 0; i < boardSize; i++)
            {
                Console.Write('-');
            }
            Console.WriteLine();

            Console.Write(" ");
            for (int i = 0; i < boardSize; i++)
            {
                Console.Write((char)('A' + i));
            }

            Console.WriteLine();

        }

        private char BoardContentToSymbol(int content)
        {
            switch (content)
            {
                case CELL_PLAYER: return '*';
                case CELL_EMPTY_VISITED: return ' ';
                case CELL_GOAL: return 'G';
                default: return '?';
            }
        }

        private string GetPrintableStatus()
        {
            return "CURRENT POSITION: " + GetPrintablePlayerPosition() + "\t MOVES: " + gameState.moves + "\t LIVES LEFT: " + gameState.lives;
        }
    }
}

