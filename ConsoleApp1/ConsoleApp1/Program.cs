using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] str = Console.ReadLine().Split(",");

            Class1 c = new Class1();
            int[][] map = c.GetMap();
            GetPath1(map, int.Parse(str[0]), int.Parse(str[1]), int.Parse(str[2]), int.Parse(str[3]));
            map[int.Parse(str[0])][int.Parse(str[1])] = -1;
            map[int.Parse(str[2])][int.Parse(str[3])] = -2;
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == 1)
                        Console.Write(".");
                    else if (map[i][j] == 0)
                        Console.Write("W");
                    else if (map[i][j] == -1)
                        Console.Write("S");
                    else if (map[i][j] == -2)
                        Console.Write("E");
                    else
                        Console.Write("#");
                }
                Console.Write("\n");
            }
        }

        static void GetPath1(int[][] map,int curX,int curY,int tarX,int tarY)
        {
            if (curX >= map.Length || curY >= map[0].Length || curX < 0 || curY < 0)
            {
                return;
            }
            else
            {
                if ((curX == tarX && curY == tarY))
                {
                    map[curX][curY] = 2;
                    return;
                }
                else if (map[curX][curY] == 2 || map[curX][curY] == 0 || map[tarX][tarY] == 2)
                {
                    return;
                }
                else
                {
                    map[curX][curY] = 2;
                    GetPath1(map, curX + 1, curY, tarX, tarY);
                    GetPath1(map, curX, curY + 1, tarX, tarY);
                    GetPath1(map, curX - 1, curY, tarX, tarY);
                    GetPath1(map, curX, curY - 1, tarX, tarY);
                }
            }
        }

        static void GetPath2(int[][] map)
        {
        }
    }
}
