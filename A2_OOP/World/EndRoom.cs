//Author: Joon Song
//Project Name: A2_OOP
//File Name: EndRoom.cs
//Creation Date: 10/10/2018
//Modified Date: 10/10/2018
//Description: Class to hold EndRoom object; child of Room object

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
    public sealed class EndRoom : Room
    {
        //Variables relating to room image and rectangle
        private Rectangle rectangle;
        private static Texture2D image;
        
        /// <summary>
        /// Static constructor to set up EndRoom class
        /// </summary>
        static EndRoom()
        {
            //Importing end room image
            image = Main.Content.Load<Texture2D>("Images/Sprites/Rooms/endRoomImage"); 
        }

        /// <summary>
        /// Constructor for EndRoom object
        /// </summary>
        /// <param name="x">The x coordinate of the end room</param>
        /// <param name="y">The y coordinate of the end room</param>
        /// <param name="doorLayout">A string containing the layout of the doors</param>
        public EndRoom(byte x, byte y, string doorLayout) : base(x, y, doorLayout)
        {
            //Setting up end room rectangle
            rectangle = new Rectangle(HorizontalEdgeBuffer + x * RoomSize + InnerImageBuffer, VerticalEdgeBuffer + y * RoomSize + InnerImageBuffer,
                InnerImageSize, InnerImageSize);
        }

        /// <summary>
        /// Draw subprogram for EndRoom object
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Calling parent Room draw subprogram
            base.Draw(spriteBatch);

            //Drawing end room image
            spriteBatch.Draw(image, rectangle, Color.White);
        }
    }
}
