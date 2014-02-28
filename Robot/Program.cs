using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Imagine a robot sitting on the upper left corner of a X by Y grid.
// The robot can only move in two directions: right and down
// How many possible paths are there for the robot to go from
// (0, 0) to (X, Y) ?

namespace Robot
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 3, y = 2;

            Console.WriteLine("Test case 1: Num of possible paths for robot to go from 0,0 to {0},{1} is {2}", x, y, GetNumWays(x, y));

            Console.WriteLine("Test case 2: Num of possible paths for robot to go from 0,0 to {0}, {1} is {2}", y, x, GetNumWaysEasier(x, y));

            // Follow up
            Dictionary<Point, bool> cache = new Dictionary<Point, bool>();

            List<Point> result = new List<Point>();
            if (FindPath(x, y, result, cache))
            {
                Console.WriteLine("Path from (0, 0) to ({0},{1})", x, y);

                result.Reverse();

                foreach (Point p in result)
                {
                    Console.WriteLine("{0}, {1}", p.x, p.y);
                }
            }
        }

        // There are total X + Y steps
        // To get the answer to this question, we need to find
        // the number of ways X right moves can be selected 
        // out of (X + Y). This is a combinations problem of selecting
        // r items out of n since the ordering of X right steps does 
        // not matter.
        // Answer - n!/r!(n - r)! = (x + y)!/x!y!

        static int GetNumWays(int x, int y)
        {
            return Factorial(x + y)/(Factorial(x) * Factorial(y));
        }

        // Assumes num >= 0
        static int Factorial(int num)
        {
           if (num == 0)
           {
              return 1;
           }

           return num * Factorial(num - 1);
        }

        // Easier way of finding number of ways to get from top left 
        // of a matrix to bottom right
        // NOTE: x is column and y is row and they are 0 indexed

        // This solution is by tracking path backwards from 
        // the point (y , x) back to (0, 0)
        static int GetNumWaysEasier(int x, int y)
        {
            // If you have reached the first row or the 
            // first column then only one way to get back 
            // to origin
            if (y == 0 || x == 0)
            {
                return 1;
            }

            // Otherwise the result is the sum of the number of ways 
            // to get to one cell of the left and to one cell to the top
            // since the only allowed movements are right and down
            return GetNumWaysEasier(x, y - 1) + GetNumWaysEasier(x - 1, y);
        }


        // Follow up: If certain spots were off limits, design an algorithm to 
        // find a path from origin to destination
        static bool IsFree(int x, int y)
        {
            if (x == 2 && y == 2)
            {
                return false;
            }

            return true;
        }

        static bool FindPath(int x, int y, List<Point> path)
        {
           Point p = new Point(x, y);
           path.Add(p);

           if (x == 0 && y == 0)
           {
               return true;
           }

           bool success = false;
           if (x >= 1 && IsFree(x - 1, y))
           {
               success = FindPath(x - 1, y, path);
           }

           if (!success && y >= 1&& IsFree(x, y - 1))
           {
               success = FindPath(x, y - 1, path);
           }

           if (!success)
           {
              path.Remove(p);
           }

           return success;     
        }

        // Using Dynamic programming aka caching
        static bool FindPath(int x, int y, List<Point> path, Dictionary<Point, bool> cache)
        {
           Point p = new Point(x, y);

           if (cache.ContainsKey(p))
           {
              return cache[p];
           }

           path.Add(p);

           if (x == 0 && y == 0)
           {
               return true;
           }

           bool success = false;
           if (x >= 1 && IsFree(x - 1, y))
           {
               success = FindPath(x - 1, y, path, cache);
           }

           if (!success && y >= 1 && IsFree(x, y - 1))
           {
               success = FindPath(x, y - 1, path, cache);
           }

           if (!success)
           {
              path.Remove(p);
           }

           cache.Add(p, success);

           return success;
        }
    }

    class Point
    {
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x { private set; get; }

        public int y { private set; get; }
    }
}
