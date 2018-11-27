//Author: Joon Song
//Project Name: A2_OOP
//File Name: Trap.cs
//Creation Date: 10/10/2018
//Modified Date: 10/10/2018
//Description: Class to hold Trap object; parent to ActiveTrap and PassiveTrap object

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
    public abstract class Trap
    {
        /// <summary>
        /// Whether the trap is active or not
        /// </summary>
        public bool IsActive { get; set; }

        //Variables to hold damage and time related data
        protected byte damageAmount;
        protected float timeRemaining;

        //Variables relating to Trap image and rectangle
        protected Texture2D image;
        private readonly Rectangle rectangle;

        //Variables relating to trap information
        protected string[] trapInfoText = { "", "", "", "" };
        protected Vector2[] trapInfoTextLocs = new Vector2[4];
        private static Color[] trapInfoTextColors = { Color.Red, Color.White, Color.White, Color.Green };
        protected bool drawTrapInfo = false;

        /// <summary>
        /// Constructor for Trap object
        /// </summary>
        /// <param name="x">The x coordinate of the trap</param>
        /// <param name="y">The y coordinate of the trap</param>
        /// <param name="damageAmount">The amount of damage the trap inflicts</param>
        protected Trap(byte x, byte y, byte damageAmount)
        {
            //Setting up trap rectangle
            rectangle = new Rectangle(Room.HorizontalEdgeBuffer + x * Room.RoomSize + Room.InnerImageBuffer, Room.VerticalEdgeBuffer + y * Room.RoomSize + Room.InnerImageBuffer,
                Room.InnerImageSize, Room.InnerImageSize);
            
            //Setting up trap damage and enabled status
            this.damageAmount = damageAmount;
            IsActive = true;

            //Setting trap information output
            trapInfoTextLocs[0] = new Vector2(988, 415);
            trapInfoTextLocs[2] = new Vector2(917, 535);
            trapInfoText[3] = "ESCAPE!!!";
            trapInfoTextLocs[3] = new Vector2(1013, 595);
        }

        /// <summary>
        /// Update subprogram for Trap object
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="player">The player on the trap</param>
        public virtual void Update(GameTime gameTime, Player player)
        {
            //If the trap is active, update time remaining and set trap info status as true;
            if (IsActive)
            {
                timeRemaining -= (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
                drawTrapInfo = true;
            }
        }

        /// <summary>
        /// Draw subprogram for Trap object
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Drawing trap and information; if trap is active
            if (IsActive)
            {
                spriteBatch.Draw(image, rectangle, Color.White);
            }

            //Calling subprogram to draw trap information, if it should
            if (drawTrapInfo)
            {
                DrawInformation(spriteBatch);
            }
        }

        /// <summary>
        /// Draw subprogram for Trap information
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw Sprites</param>
        private void DrawInformation(SpriteBatch spriteBatch)
        {
            //Drawing trap information
            for (byte i = 0; i < trapInfoText.Length; i++)
            {
                spriteBatch.DrawString(SharedData.InformationFonts[0], trapInfoText[i], trapInfoTextLocs[i], trapInfoTextColors[i]);
            }
        }
    }
}