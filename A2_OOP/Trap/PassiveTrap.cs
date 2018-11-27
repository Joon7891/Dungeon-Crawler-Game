//Author: Joon Song
//Project Name: A2_OOP
//File Name: PassiveTrap.cs
//Creation Date: 10/10/2018
//Modified Date: 10/10/2018
//Description: Class to hold PassiveTrap object; child of Trap object

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace A2_OOP
{
    public sealed class PassiveTrap : Trap 
    {
        //Variables to hold passive trap image and sound
        private static Texture2D passiveTrapImage;
        private static SoundEffect passiveTrapSoundEffect;

        /// <summary>
        /// Static constructor to set up PassiveTrap class
        /// </summary>
        static PassiveTrap()
        {
            //Importing passive trap image and sound effect
            passiveTrapImage = Main.Content.Load<Texture2D>("Images/Sprites/Traps/trapPassiveImage");
            passiveTrapSoundEffect = Main.Content.Load<SoundEffect>("Audio/SoundEffects/passiveTrapSoundEffect");
        }

        /// <summary>
        /// Constructor for PassiveTrap object
        /// </summary>
        /// <param name="x">The x coodinate of the passive trap</param>
        /// <param name="y">The y coordinate of the passive trap</param>
        /// <param name="damageAmount">The damage amount of the passive trap</param>
        /// <param name="escapeTime">The escape time of the active trap</param>
        public PassiveTrap(byte x, byte y, byte damageAmount, float escapeTime) : base(x, y, damageAmount)
        {
            //Setting up image and escape time
            image = passiveTrapImage;
            timeRemaining = escapeTime;

            //Setting trap information output related data
            trapInfoText[0] = "Passive Trap";
            trapInfoTextLocs[1] = new Vector2(920, 475);
        }

        /// <summary>
        /// Update subprogram for PassiveTrap object
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="player">The player on the passive trap</param>
        public override void Update(GameTime gameTime, Player player)
        {
            //Only calling update logic if trap is active
            if (IsActive)
            {
                //Inflicting damage, playing soundeffect, and disabling trap if player does not move in time
                if (timeRemaining <= 0)
                {
                    player.InflictDamage(damageAmount);
                    timeRemaining = 0;
                    passiveTrapSoundEffect.CreateInstance().Play();
                    IsActive = false;
                }

                //Updating information text
                trapInfoText[1] = $"Time to Escape: {Math.Round(timeRemaining, 1)}s";
                trapInfoText[2] = $"Damage Amount: {damageAmount}";

                //Calling parent update subprogram
                base.Update(gameTime, player);
            }
        }
    }
}
