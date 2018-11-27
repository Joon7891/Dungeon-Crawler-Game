//Author: Joon Song
//Project Name: A2_OOP
//File Name: PassageRoom.cs
//Creation Date: 10/10/2018
//Modified Date: 10/20/2018
//Description: Class to hold PassageRoom object; child of Room object

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A2_OOP
{
    public sealed class PassageRoom : Room
    {
        //The trap in the passage room
        private Trap trap;
        
        /// <summary>
        /// Constructor for PassageRoom object
        /// </summary>
        /// <param name="x">The x coordinate of the passage room</param>
        /// <param name="y">The y coordinate of the passage room</param>
        /// <param name="doorLayout">A string containing the layout of the doors</param>
        /// <param name="trap">The trap in the passage room</param>
        public PassageRoom(byte x, byte y, string doorLayout, Trap trap = null) : base(x, y, doorLayout)
        {
            //Setting up passage room trap
            this.trap = trap;
        }

        /// <summary>
        /// Update subprogram for PassageRoom object
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="player">The player in the room</param>
        public override void Update(GameTime gameTime, Player player)
        {
            //Updating trap; if not null
            trap?.Update(gameTime, player);
            
            //Calling parent Room update subprogram
            base.Update(gameTime, player);
        }

        /// <summary>
        /// Draw subprogram for PassageRoom object
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Calling parent Room draw subprogram
            base.Draw(spriteBatch);

            //Drawing trap; if one exist and it is active and revealed
            if (trap != null && trap.IsActive && isVisited)
            {
                trap?.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Subprogram to make appropriate updates when the room is left
        /// </summary>
        public void LeaveRoom()
        {
            //Disabling trap; if one exists
            if (trap != null)
            {
                trap.IsActive = false;
            }
        }
    }
}
