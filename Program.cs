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
        

        programIntro();
        Console.ReadKey();
        stopwatch.Start();
        Console.Clear();

        int coinCount = 0;
        int origRow = Console.CursorTop + 1;
        int origCol = Console.CursorLeft + 1;
        foreach(string row in mapRows)
        {
            Console.WriteLine(row);
        }
        Console.SetCursorPosition(origCol, origRow);

        bool goalNotReached = false;
        long seconds = 0;
        int copyCol = 0;
        int copyRow = 0;
        do
        {
            //copies of the original row and column values in case moving is invalid
            copyCol = origCol;
            copyRow = origRow;
            switch(Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    origRow--;
                    break;
                case ConsoleKey.DownArrow:
                    origRow++;
                    break;
                case ConsoleKey.LeftArrow:
                    origCol--;
                    break;
                case ConsoleKey.RightArrow:
                    origCol++;
                    break;
            }
            if(tryMove(mapRows, origCol, origRow, coinCount))
            {
                Console.SetCursorPosition(origCol, origRow);
            }
            else
            {
                origCol = copyCol;
                origRow = copyRow;
                Console.SetCursorPosition(origCol, origRow);
            }
            if(collectCoins(mapRows, origCol, origRow))
            {
                mapRows[origRow] = mapRows[origRow].Replace('^', ' ');
                Console.SetCursorPosition(0, origRow);
                Console.Write(mapRows[origRow]);
                Console.SetCursorPosition(origCol, origRow);
                coinCount++;
            } 
            goalNotReached = reachedGoal(mapRows, origCol, origRow);
        }
        while(goalNotReached);

        seconds = stopwatch.ElapsedMilliseconds/1000;
        Console.WriteLine();
        stopwatch.Stop();
        Console.Clear();

        Console.WriteLine("Congratulations! You reached the end of the maze!!!");
        Console.WriteLine($"It took you {seconds} seconds to complete!");
    }
    static void programIntro()
    {
        Console.WriteLine("This program will present a maze for you to move through using directional arrows, your goal is to reach the *");
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
    static bool tryMove(string[] map, int col, int row, int coins)
    {
        if(map[row][col].Equals('*'))
        {
            return false;
        }
        if(map[row][col].Equals('|') && coins != 10)
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

    static bool collectCoins(string [] map, int col, int row)
    {
        if(map[row][col].Equals('^'))
        {
            return true;
        }
        return false;
    }
}