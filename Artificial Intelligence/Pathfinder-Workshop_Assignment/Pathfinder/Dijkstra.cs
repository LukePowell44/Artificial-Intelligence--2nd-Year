using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;


namespace Pathfinder
{
    class Dijkstra
    {
        public bool[,] closed; //whether or not location is closed
        public float[,] cost; //cost value for each location
        public Coord2[,] link; //link for each location = coords
                               //of a neighbouring location
        public bool[,] inPath; //whether or not a location
                               //is in the final path
        public Dijkstra()
        {
            closed = new bool[40, 40];
            cost = new float[40, 40];
            link = new Coord2[40, 40];
            inPath = new bool[40, 40];
        }

        public void Build(Level level, AiBotBase bot, Player plr)
        {
            for (int i = 0; i < 40; i++)
            {
                for (int x = 0; x < 40; x++)
                {
                    Coord2 C = new Coord2(-1, -1);
                    closed[i, x] = false;
                    cost[i, x] = 1000000;
                    link[i, x] = C;
                    inPath[i, x] = false;
                }
            }
            closed[bot.GridPosition.X, bot.GridPosition.Y] = false;
            cost[bot.GridPosition.X, bot.GridPosition.Y] = 0;

            while (true)
            {
                Coord2 Smallest_Position = new Coord2(0, 0);
                float smallest_Value = 1000;
                List<Coord2> Valid_Value = new List<Coord2>();

                for (int x = 0; x < 40; x++)
                {
                    for (int y = 0; y < 40; y++)
                    {
                        if (closed[x, y] == false)
                        {
                            Coord2 My_Coord2 = new Coord2(x, y);
                            bool Response = level.ValidPosition(My_Coord2);
                            if (Response == true)
                            {
                                Valid_Value.Add(My_Coord2);
                            }

                        }
                    }
                }

                for (int i = 0; i < Valid_Value.Count(); i++)
                {
                    float Cost_Value = cost[Valid_Value[i].X, Valid_Value[i].Y];
                    if (Cost_Value < smallest_Value)
                    {
                        smallest_Value = Cost_Value;
                        Smallest_Position.X = Valid_Value[i].X;
                        Smallest_Position.Y = Valid_Value[i].Y;
                    }
                }
                closed[Smallest_Position.X, Smallest_Position.Y] = true;
                for (int x = -1; x < 2; x++)
                {
                    for (int y = 1; y > -2; y--)
                    {
                        if (x != 0 || y != 0)
                        {
                            if (x == 0 || y == 0)
                            {
                                try
                                {
                                    if (cost[Smallest_Position.X, Smallest_Position.Y] + 1.0f < cost[Smallest_Position.X + x, Smallest_Position.Y + y])
                                    {
                                        Coord2 C = new Coord2(Smallest_Position.X + x, Smallest_Position.Y + y);
                                        bool Valid_Position = level.ValidPosition(C);
                                        if (Valid_Position == true)
                                        {
                                            cost[Smallest_Position.X + x, Smallest_Position.Y + y] = cost[Smallest_Position.X, Smallest_Position.Y] + 1.0f;
                                            link[Smallest_Position.X + x, Smallest_Position.Y + y].X = Smallest_Position.X;
                                            link[Smallest_Position.X + x, Smallest_Position.Y + y].Y = Smallest_Position.Y;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (cost[Smallest_Position.X, Smallest_Position.Y] + 1.4f < cost[Smallest_Position.X + x, Smallest_Position.Y + y])
                                    {
                                        Coord2 C = new Coord2(Smallest_Position.X + x, Smallest_Position.Y + y);
                                        bool Valid_Position = level.ValidPosition(C);
                                        if (Valid_Position == true)
                                        {
                                            cost[Smallest_Position.X + x, Smallest_Position.Y + y] = cost[Smallest_Position.X, Smallest_Position.Y] + 1.4f;
                                            link[Smallest_Position.X + x, Smallest_Position.Y + y].X = Smallest_Position.X;
                                            link[Smallest_Position.X + x, Smallest_Position.Y + y].Y = Smallest_Position.Y;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
                if (closed[plr.GridPosition.X, plr.GridPosition.Y] == true)
                {
                    break;
                }
            }
            bool done = false;
            Coord2 nextClosed = plr.GridPosition;

            while (!done)
            {
                inPath[nextClosed.X, nextClosed.Y] = true;
                nextClosed = link[nextClosed.X, nextClosed.Y];
                if (nextClosed == bot.GridPosition)
                {
                    done = true;
                }
            }
        }
    }
}

        