//Author: Joon Song
//Project Name: A2_OOP
//File Name: Dungeon.cs
//Creation Date: 10/11/2018
//Modified Date: 10/19/2018
//Description: Class to hold dungeon object

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace A2_OOP
{
    public sealed class Dungeon
    {
        /// <summary>
        /// The size of the dungeon in pixels
        /// </summary>
        public const int SIZE = 800;

        /// <summary>
        /// The ID number of the Dungeon
        /// </summary>
        public int ID { get; }  
        
        /// <summary>
        /// The name of the Dungeon
        /// </summary>
        public string Name { get; }

        //Objects to hold game related data
        private Player player;
        private Room currentRoom;
        private Room[,] gameRooms;

        //Objects to hold game result related data
        private static Texture2D winImage;
        private static Texture2D lossImage;
        private static Rectangle resultRectangle = new Rectangle(806, 403, 588, 491);

        /// <summary>
        /// Static constructor to set up Dungeon class
        /// </summary>
        static Dungeon()
        {
            //Importing result images
            winImage = Main.Content.Load<Texture2D>("Images/Sprites/winImage");
            lossImage = Main.Content.Load<Texture2D>("Images/Sprites/lossImage");
        }

        /// <summary>
        /// Constructor for Dungeon object
        /// </summary>
        /// <param name="id">The ID of the dungeon</param>
        /// <param name="name">The name of the dungeon</param>
        /// <param name="player">The player in the dungeon</param>
        /// <param name="gameRooms">The game rooms in the dungeon</param>
        public Dungeon(int id, string name, Player player, Room[,] gameRooms)
        {
            //Setting up various dungeon information
            ID = id;
            Name = name;

            //Setting up dungeon player and game rooms
            this.player = player;
            this.gameRooms = gameRooms;
        }

        /// <summary>
        /// Update subprogram for Dungeon object
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public void Update(GameTime gameTime)
        {
            //Updating appropraite variables/objects given game is still working
            if (player.CurrentGameState == GameState.NotStarted || player.CurrentGameState == GameState.Playing)
            {
                //Updating current room and player
                currentRoom = gameRooms[player.X, player.Y];
                currentRoom.Update(gameTime, player);
                player.Update(gameTime, currentRoom);
            }
        }

        /// <summary>
        /// Draw subprogram for Dungeon object
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Drawing rooms
            for (byte i = 0; i < gameRooms.GetLength(0); i++)
            {
                for (byte j = 0; j < gameRooms.GetLength(1); j++)
                {
                    gameRooms[i, j].Draw(spriteBatch);
                }
            }

            //Drawing player
            player.Draw(spriteBatch);

            //Informing user (visually) if they won or lost; if appropriate
            switch (player.CurrentGameState)
            {
                //Drawing win image if game has been won
                case GameState.Win:
                    spriteBatch.Draw(winImage, resultRectangle, Color.White);

                    break;

                //Drawing loss image if game has been loss
                case GameState.Loss:
                    spriteBatch.Draw(lossImage, resultRectangle, Color.White);

                    break;
            }
        }
    }
}
