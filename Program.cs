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
internal class Program
{

    private static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();
        Random rand = new Random();
        int randMapNum = rand.Next(1,6);
        string[] mapRows = mapChoice(randMapNum);
        char[][] mapChar = mapRows.Select(item => item.ToArray()).ToArray();
        

        programIntro();
        Console.ReadKey();
        stopwatch.Start();
        Console.Clear();

        int coinCount = 0;
        long points = 0;
        int origRow = Console.CursorTop + 1;
        int origCol = Console.CursorLeft + 1;
        int enemyRow = 5;
        int enemyCol = 14;
        foreach(char[] character in mapChar)
        {
            Console.WriteLine(character);
        }
        Console.SetCursorPosition(origCol, origRow);

        bool goalNotReached = false;
        long seconds = 0;
        int copyCol = 0;
        int copyRow = 0;
        int copyEnemyRow = 0;
        int copyEnemyCol = 0;
        do
        {
            //copies of the original row and column values in case moving is invalid
            copyCol = origCol;
            copyRow = origRow;
            copyEnemyRow = enemyRow;
            copyEnemyCol = enemyCol;
            switch(Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    origRow--;
                    enemyRow--;
                    break;
                case ConsoleKey.DownArrow:
                    origRow++;
                    enemyRow++;
                    break;
                case ConsoleKey.LeftArrow:
                    origCol--;
                    enemyCol--;
                    break;
                case ConsoleKey.RightArrow:
                    origCol++;
                    enemyCol++;
                    break;
            }
            if(tryMove(mapRows, enemyCol, enemyRow))
            {
                moveEnemy(mapChar, enemyCol, enemyRow, copyEnemyCol, copyEnemyRow);
            }
            else
            {
                enemyRow = copyEnemyRow;
                enemyCol = copyEnemyCol;
            }
            if(tryMove(mapRows, origCol, origRow))
            {
                Console.SetCursorPosition(origCol, origRow);
            }
            else
            {
                origCol = copyCol;
                origRow = copyRow;
                Console.SetCursorPosition(origCol, origRow);
            }
            if(mapRows[origRow][origCol].Equals('^'))
            {
                collectCoins(mapChar, origCol, origRow);
                coinCount++;
                points += 100;
            }
            if(openGate(coinCount))
            {
                //the middle part of the gate is found in the 10th index of the array
                mapRows[10] = mapRows[10].Replace('|', ' ');
                Console.SetCursorPosition(0, 10);
                Console.Write(mapRows[10]);
                Console.SetCursorPosition(origCol, origRow);
            }
            if(mapRows[origRow][origCol].Equals('$'))
            {
                collectExtraPoints(mapChar, origCol, origRow);
                points += 200;
            }
            goalNotReached = reachedGoal(mapRows, origCol, origRow);
            if(!runIntoEnemy(mapRows, origCol, origRow))
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
        
        if(!reachedGoal(mapRows, origCol, origRow))
        {
            Console.WriteLine($"Congratulations! You reached the end of the maze with a score of {points}!!!");
            Console.WriteLine($"It took you {seconds} seconds to complete!");
        }
        else
        {
            Console.WriteLine($"Game over. Score: {points}");
        }
    }
    static void programIntro()
    {
        Console.WriteLine("This program will present a maze for you to move through using directional arrows, your goal is to reach the #, ^ are coins to collect, and % are enemies. The faster you go the better score you get");
        Console.WriteLine("Goodluck! Press any button to continue");
    }

    static bool reachedGoal(string[] map, int col, int row)
    {
        if(map[row][col].Equals('#'))
        {
            return false;
        }
        return true;
    }

    //tests to make sure that where the user wants to go is valid
    //can't go past the top or bottom of maze and can't go to the left or right of the maze
    static bool tryMove(string[] map, int col, int row)
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

    static bool runIntoEnemy(string [] map, int col, int row)
    {
        if(map[row][col].Equals('%'))
        {
            return false;
        }
        return true;
    }

    static void moveEnemy(char[][] map, int col, int row, int previousCol, int previousRow)
    {
        map[previousRow][previousCol] = ' ';
        Console.SetCursorPosition(previousCol, previousRow);
        Console.Write(map[previousRow][previousCol]);
        map[row][col] = '%';
        Console.SetCursorPosition(col, row);
        Console.Write(map[row][col]);
        
    }
}