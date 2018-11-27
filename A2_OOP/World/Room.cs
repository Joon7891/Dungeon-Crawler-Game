//Author: Joon Song
//Project Name: A2_OOP
//File Name: Room.cs
//Creation Date: 10/10/2018
//Modified Date: 10/20/2018
//Description: Class to hold Room object; parent to PassageRoom, ShopRoom, and EndRoom objects

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
    public abstract class Room
    {
        /// <summary>
        /// Whether the top of the room is open
        /// </summary>
        public bool IsTopOpen { get; }

        /// <summary>
        /// Whether the right of the room is open
        /// </summary>
        public bool IsRightOpen { get; }

        /// <summary>
        /// Whether the bottom of the room is open
        /// </summary>
        public bool IsBottomOpen { get; }

        /// <summary>
        /// Whether the left of the room is open
        /// </summary>
        public bool IsLeftOpen { get; }

        /// <summary>
        /// The size of the room, in pixels
        /// </summary>
        public static byte RoomSize { get; private set; }

        /// <summary>
        /// The horizontal buffer between the room and the dungeon edge, in pixels
        /// </summary>
        public static byte HorizontalEdgeBuffer { get; private set; }

        /// <summary>
        /// The vertical buffer between the room and the dungeon edge, in pixels
        /// </summary>
        public static byte VerticalEdgeBuffer { get; private set; }

        /// <summary>
        /// The size of the inner image, in pixels
        /// </summary>
        public static byte InnerImageSize { get; private set; }

        /// <summary>
        /// The edge buffer between the room inner image and the room edge, in pixels
        /// </summary>
        public static byte InnerImageBuffer { get; private set; }

        //Whether the room has been visited yet
        protected bool isVisited;

        //Variables relating to room images and rectangle
        private Rectangle rectangle;
        private Texture2D image;
        private static Texture2D hiddenRoomImage;
        private static Dictionary<string, Texture2D> roomImageDictionary = new Dictionary<string, Texture2D>();

        /// <summary>
        /// Static constructor to setup Room class
        /// </summary>
        static Room()
        {
            //Importing room images
            string roomCode;
            for (byte i = 0; i < 16; i++)
            {
                roomCode = Convert.ToString(i, 2).PadLeft(4, '0');
                roomImageDictionary.Add(roomCode, Main.Content.Load<Texture2D>($"Images/Sprites/Rooms/room{roomCode}"));
            }
            hiddenRoomImage = Main.Content.Load<Texture2D>("Images/Sprites/Rooms/roomHiddenImage");
        }

        /// <summary>
        /// Constructor for Room object
        /// </summary>
        /// <param name="x">The x-coordinate of the room</param>
        /// <param name="y">The y-coordinate of the room</param>
        /// <param name="doorLayout">The string containing the door layout</param>
        protected Room(byte x, byte y, string doorLayout)
        {
            //Setting up room image and rectangle
            image = roomImageDictionary[doorLayout];
            rectangle = new Rectangle(HorizontalEdgeBuffer + x * RoomSize, VerticalEdgeBuffer + y * RoomSize, RoomSize, RoomSize);

            //Setting up room door properties
            IsTopOpen = doorLayout[0] == '1';
            IsRightOpen = doorLayout[1] == '1';
            IsBottomOpen = doorLayout[2] == '1';
            IsLeftOpen = doorLayout[3] == '1';
        }

        /// <summary>
        /// Subprogram to setup dimensions of the Room object
        /// </summary>
        /// <param name="columns">The number of columns in the dungeon</param>
        /// <param name="rows">The number of rows in the dungeon</param>
        public static void SetUpDimensions(byte columns, byte rows)
        {
            //Setting up data relating to room dimensions and location
            RoomSize = (byte)(Dungeon.SIZE / Math.Max(columns, rows));
            HorizontalEdgeBuffer = (byte)((Dungeon.SIZE - RoomSize * columns) / 2);
            VerticalEdgeBuffer = (byte)((Dungeon.SIZE - RoomSize * rows) / 2);
            InnerImageSize = (byte)(RoomSize * 0.75 + 0.5);
            InnerImageBuffer = (byte)((RoomSize - InnerImageSize) / 2);
        }

        /// <summary>
        /// Update subprogram for Room object
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="player">The player in the room</param>
        public virtual void Update(GameTime gameTime, Player player)
        {
            //Setting room as visited; if it hasn't been set as visited
            if (!isVisited)
            {
                isVisited = true;
            }
        }

        /// <summary>
        /// Draw subprogram for Room object
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Drawing room
            spriteBatch.Draw(isVisited ? image : hiddenRoomImage, rectangle, Color.White);            
        }
    }
}
