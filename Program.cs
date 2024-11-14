/***********************
* Caleb Roskelley
* Lab 7 Maze
* Date Started: 10/23
* Date Finished:
***********************/

using System.Reflection.Metadata;
using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.CompilerServices;
using System.Data;
internal class Program
{

    private static void Main(string[] args)
    {
        bool playAgain = false;
        do
        {
            Stopwatch stopwatch = new Stopwatch();
            Random rand = new Random();
            int randMapNum = rand.Next(1,6);
            string[] mapRows = mapChoice(randMapNum);
            char[][] mapChar = mapRows.Select(item => item.ToArray()).ToArray();
            int[] enemy1XY = {5,14};
            int[] enemy2XY = {15,38};
            

            programIntro();
            Console.ReadKey();
            stopwatch.Start();
            Console.Clear();

            int coinCount = 0;
            long points = 0;
            int origRow = Console.CursorTop + 1;
            int origCol = Console.CursorLeft + 1;
            foreach(char[] character in mapChar)
            {
                Console.WriteLine(character);
            }
            Console.SetCursorPosition(origCol, origRow);

            bool goalNotReached = false;
            long seconds = 0;
            int copyCol = 0;
            int copyRow = 0;
            int enemy1X = 0;
            int enemy1Y = 0;
            int enemy2X = 0;
            int enemy2Y = 0;
            do
            {
                //copies of the original row and column values in case moving is invalid
                copyCol = origCol;
                copyRow = origRow;
                enemy1Y = enemy1XY[0];
                enemy1X = enemy1XY[1];
                enemy2Y = enemy2XY[0];
                enemy2X = enemy2XY[1];
                switch(Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        origRow--;
                        enemy1XY[0]--;
                        enemy2XY[0]--;
                        break;
                    case ConsoleKey.DownArrow:
                        origRow++;
                        enemy1XY[0]++;
                        enemy2XY[0]++;
                        break;
                    case ConsoleKey.LeftArrow:
                        origCol--;
                        enemy1XY[1]--;
                        enemy2XY[1]--;
                        break;
                    case ConsoleKey.RightArrow:
                        origCol++;
                        enemy1XY[1]++;
                        enemy2XY[1]++;
                        break;
                    default:
                        break;
                }
                if(tryMove(mapChar, enemy1XY[1], enemy1XY[0])&& !mapChar[enemy1XY[0]][enemy1XY[1]].Equals('^'))
                {
                    moveEnemy(mapChar, enemy1XY[1], enemy1XY[0], enemy1X, enemy1Y);
                }
                else
                {
                    enemy1XY[0] = enemy1Y;
                    enemy1XY[1] = enemy1X;
                }
                if(tryMove(mapChar, enemy2XY[1], enemy2XY[0]) && !mapChar[enemy2XY[0]][enemy2XY[1]].Equals('^'))
                {
                    moveEnemy(mapChar, enemy2XY[1], enemy2XY[0], enemy2X, enemy2Y);
                }
                else
                {
                    enemy2XY[0] = enemy2Y;
                    enemy2XY[1] = enemy2X;
                }
                if(tryMove(mapChar, origCol, origRow))
                {
                    Console.SetCursorPosition(origCol, origRow);
                }
                else
                {
                    origCol = copyCol;
                    origRow = copyRow;
                    Console.SetCursorPosition(origCol, origRow);
                }
                if(mapChar[origRow][origCol].Equals('^'))
                {
                    collectCoins(mapChar, origCol, origRow);
                    coinCount++;
                    points += 100;
                }
                if(openGate(coinCount))
                {
                    //the middle part of the gate is found in the 10th index of the array
                    mapChar[10][18] = ' ';
                    Console.SetCursorPosition(18,10);
                    Console.WriteLine(mapChar[10][18]);
                    Console.SetCursorPosition(origCol, origRow);
                }
                if(mapChar[origRow][origCol].Equals('$'))
                {
                    collectExtraPoints(mapChar, origCol, origRow);
                    points += 200;
                }
                goalNotReached = reachedGoal(mapChar, origCol, origRow);
                if(!runIntoEnemy(mapChar, origCol, origRow))
                {
                    goalNotReached = false;
                }
            }
            while(goalNotReached);

            seconds = stopwatch.ElapsedMilliseconds/1000;
            Console.WriteLine();
            stopwatch.Stop();
            Console.Clear();

            //change score based on how long the user takes to reach the end
            points = points - (seconds*10);
            
            if(!reachedGoal(mapChar, origCol, origRow))
            {
                Console.WriteLine($"Congratulations! You reached the end of the maze with a score of {points}!!!");
                Console.WriteLine($"It took you {seconds} seconds to complete!");
                Console.WriteLine("Press any button to continue");
            }
            else
            {
                Console.WriteLine($"Game over. Score: {points}");
            }
            Console.ReadKey();
            playAgain = keepPlaying();
        }
        while(playAgain);
    }
    static void programIntro()
    {
        Console.WriteLine("This program will present a maze for you to move through using directional arrows, your goal is to reach the #, ^ are coins to collect, and % are enemies. The faster you go the better score you get");
        Console.WriteLine("Goodluck! Press any button to continue");
    }

    static bool reachedGoal(char[][] map, int col, int row)
    {
        if(map[row][col].Equals('#'))
        {
            return false;
        }
        return true;
    }

    //tests to make sure that where the user wants to go is valid
    //can't go past the top or bottom of maze and can't go to the left or right of the maze
    static bool tryMove(char[][] map, int col, int row)
    {
        if(map[row][col].Equals('*') || map[row][col].Equals('|'))
        {
            return false;
        }

        if(col == -1 ||  row < 0)
        {
            return false;
        }
        return true;
    }
    
    //method to randomly select one of the map choicesS
    static string[] mapChoice(int randNum)
    {
        string[] map = new string[6];
        map = File.ReadAllLines($"maze.txt");
        return map;
    }

    static void collectCoins(char[][] map, int col, int row)
    {
        map[row][col] = ' ';
        Console.Write(map[row][col]);
        Console.SetCursorPosition(col, row);
    }

    static bool openGate(int coins)
    {
        if(coins == 10)
        {
            return true;
        }
        return false;
    }

    static void collectExtraPoints(char[][] map, int col, int row)
    {
        map[row][col] = ' ';
        Console.Write(map[row][col]);
        Console.SetCursorPosition(col, row);
    }

    static bool runIntoEnemy(char[][] map, int col, int row)
    {
        if(map[row][col].Equals('%'))
        {
            return false;
        }
        return true;
    }

    static void moveEnemy(char[][] map, int col, int row, int previousCol, int previousRow)
    {
        if(map[previousRow][previousCol].Equals('^'))
        {
            map[previousRow][previousCol] = '^';
        }
        else
        {
            map[previousRow][previousCol] = ' ';
        }
        Console.SetCursorPosition(previousCol, previousRow);
        Console.Write(map[previousRow][previousCol]);
        map[row][col] = '%';
        Console.SetCursorPosition(col, row);
        Console.Write(map[row][col]);
        
    }

    static bool keepPlaying()
    {
        Console.WriteLine("Would you like to play again? 1: Yes 2: No");
        string useranswer = Console.ReadLine();
        bool answer = int.TryParse(useranswer, out int num);
        do
        {
            if(answer)
            {
                if(num == 1 || useranswer.ToLower().Equals("yes"))
                {
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Please give a valid answer");
            }
        }
        while(!answer);
        return false;
    }
}