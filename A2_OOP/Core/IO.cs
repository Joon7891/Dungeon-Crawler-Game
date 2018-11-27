//Author: Joon Song
//Project Name: A2_OOP
//File Name: IO.cs
//Creation Date: 10/13/2018
//Modified Date: 10/13/2018
//Descrition: Class to hold file input and output

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace A2_OOP
{
    public static class IO
    {
        //The base path of each file
        private static string baseFilePath;

        //Instances of stream reader and writer for file IO
        private static StreamReader inFile;
        private static StreamWriter outFile;

        /// <summary>
        /// Static class constructor to set up IO class
        /// </summary>
        static IO()
        {
            //Setting up base file path
            baseFilePath = Assembly.GetExecutingAssembly().CodeBase;
            baseFilePath = Path.GetDirectoryName(baseFilePath).Substring(6) + @"\IO\";
        }

        /// <summary>
        /// Subprogam to load a dungeon from file
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns>The loaded dungeon</returns>
        public static Dungeon LoadDungeon(string fileName)
        {
            //Variables to hold temporary data
            string[] newLineCache;
            Vector2 coordinateCache;
            int dungeonId;
            string dungeonName;
            double timeRemaining;
            byte dungeonColumns, dungeonRows;
            Vector2 startLoc, endLoc;
            int playerCash, playerHealth;
            Dictionary<Vector2, Trap> traps = new Dictionary<Vector2, Trap>();
            Player player;
            Room[,] gameRooms;
            
            //Try-Catch block to handle file reading exceptions
            try
            {
                //Opening file
                inFile = File.OpenText(baseFilePath + fileName);

                //Reading in dungeon information
                dungeonId = Convert.ToInt16(inFile.ReadLine());
                dungeonName = inFile.ReadLine();
                timeRemaining = Convert.ToDouble(inFile.ReadLine());
                newLineCache = inFile.ReadLine().Split(',');
                dungeonRows = Convert.ToByte(newLineCache[0]);
                dungeonColumns = Convert.ToByte(newLineCache[1]);
                gameRooms = new Room[dungeonColumns, dungeonRows];
                newLineCache = inFile.ReadLine().Split(',');
                startLoc = new Vector2(Convert.ToInt16(newLineCache[1]), Convert.ToInt16(newLineCache[0]));
                newLineCache = inFile.ReadLine().Split(',');
                endLoc = new Vector2(Convert.ToInt16(newLineCache[1]), Convert.ToInt16(newLineCache[0]));

                //Calling subprogram to set up various dimensions
                Room.SetUpDimensions(dungeonColumns, dungeonRows);

                //Reading in and setting up player
                newLineCache = inFile.ReadLine().Split(',');
                playerCash = SharedData.RNG.Next(Convert.ToInt16(newLineCache[0]), Convert.ToInt16(newLineCache[1]));
                newLineCache = inFile.ReadLine().Split(',');
                playerHealth = SharedData.RNG.Next(Convert.ToInt16(newLineCache[0]), Convert.ToInt16(newLineCache[1]));
                player = new Player((byte)startLoc.X, (byte)startLoc.Y, playerCash, playerHealth, timeRemaining);

                //Reading in and setting up traps
                inFile.ReadLine();
                newLineCache = inFile.ReadLine().Split(',');
                while (newLineCache[0] != "TrapEnd")
                {
                    //Constructing passive or active trap
                    coordinateCache = new Vector2(Convert.ToInt16(newLineCache[2]), Convert.ToInt16(newLineCache[1]));
                    if (newLineCache[0] == "PassiveTrap")
                    {
                        traps.Add(coordinateCache, new PassiveTrap((byte)coordinateCache.X, (byte)coordinateCache.Y, (byte)SharedData.RNG.Next(Convert.ToByte(newLineCache[3]), Convert.ToByte(newLineCache[4]) + 1),
                            SharedData.RNG.Next(Convert.ToInt16(newLineCache[5]), Convert.ToInt16(newLineCache[6]) + 1) / 1000.0f));
                    }
                    else
                    {
                        traps.Add(coordinateCache, new ActiveTrap((byte)coordinateCache.X, (byte)coordinateCache.Y, (byte)SharedData.RNG.Next(Convert.ToByte(newLineCache[3]), Convert.ToByte(newLineCache[4]) + 1),
                            SharedData.RNG.Next(Convert.ToInt16(newLineCache[5]), Convert.ToInt16(newLineCache[6]) + 1) / 1000.0f));
                    }

                    //Reading new line of input
                    newLineCache = inFile.ReadLine().Split(',');
                }

                //Reading in and setting up rooms
                for (byte i = 0; i < dungeonRows; i++)
                {
                    for (byte j = 0; j < dungeonColumns; j++)
                    {
                        //Reading new line of input and getting coordaintes
                        newLineCache = inFile.ReadLine().Split(',');
                        coordinateCache = new Vector2(Convert.ToInt16(newLineCache[2]), Convert.ToInt16(newLineCache[1]));

                        //Creating approriate room based on line data
                        if (coordinateCache == endLoc)
                        {
                            gameRooms[(byte)coordinateCache.X, (byte)coordinateCache.Y] = new EndRoom((byte)coordinateCache.X, (byte)coordinateCache.Y, newLineCache[3]);
                        }
                        else if (newLineCache[0] == "PassageRoom")
                        {
                            if (traps.ContainsKey(coordinateCache))
                            {
                                gameRooms[(byte)coordinateCache.X, (byte)coordinateCache.Y] = new PassageRoom((byte)coordinateCache.X, (byte)coordinateCache.Y, newLineCache[3], traps[coordinateCache]);
                            }
                            else
                            {
                                gameRooms[(byte)coordinateCache.X, (byte)coordinateCache.Y] = new PassageRoom((byte)coordinateCache.X, (byte)coordinateCache.Y, newLineCache[3]);
                            }
                        }
                        else
                        {
                            gameRooms[(byte)coordinateCache.X, (byte)coordinateCache.Y] = new ShopRoom((byte)coordinateCache.X, (byte)coordinateCache.Y, newLineCache[3],
                                SharedData.RNG.Next(Convert.ToInt32(newLineCache[4]), Convert.ToInt32(newLineCache[5]) + 1),
                                (byte)SharedData.RNG.Next(Convert.ToInt32(newLineCache[6]), Convert.ToInt32(newLineCache[7]) + 1),
                                (byte)SharedData.RNG.Next(Convert.ToInt32(newLineCache[8]), Convert.ToInt32(newLineCache[9]) + 1));
                        }
                    }
                }

                //Closing file
                inFile.Close();

                //Returning constructed dungeon
                return new Dungeon(dungeonId, dungeonName, player, gameRooms);
            }
            catch (FileNotFoundException)
            {
                //Informing user that file was not found
                Console.WriteLine("Exception: File was not found");
            }
            catch (IndexOutOfRangeException)
            {
                //Informing user that index was out of range
                Console.WriteLine("Exception: Index was out of range");
            }
            catch (FormatException)
            {
                //Informing user that their data was read in via wrong format
                Console.WriteLine("Exception: Data was read in wrong format");
            }
            catch (EndOfStreamException)
            {
                //Informing user that file was attempted to be read past end of stream
                Console.WriteLine("Exception: Attempted to read past end of file");
            }

            //If exception occurs, return null
            return null;
        }
    }
}
