//Author: Joon Song
//Project Name: A2_OOP
//File Name: ActiveTrap.cs
//Creation Date: 10/10/2018
//Modified Date: 10/10/2018
//Description: Class to hold ActiveTrap object; child of Trap object

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
    public sealed class ActiveTrap : Trap
    {
        //Variables to hold active trap image and sound effect
        private static Texture2D activeTrapImage;
        private static SoundEffect activeTrapSoundEffect;

        //Variable to hold the time interval between hits
        private readonly float hitTimeInterval;

        /// <summary>
        /// Static constructor to setup ActiveTrap class
        /// </summary>
        static ActiveTrap()
        {
            //Importing active trap image and sound effect
            activeTrapImage = Main.Content.Load<Texture2D>("Images/Sprites/Traps/trapActiveImage");
            activeTrapSoundEffect = Main.Content.Load<SoundEffect>("Audio/SoundEffects/activeTrapSoundEffect");
        }

        /// <summary>
        /// Constructor for ActiveTrap object
        /// </summary>
        /// <param name="x">The x coorindate of the active trap</param>
        /// <param name="y">The y coordainte of the active trap</param>
        /// <param name="damageAmount">The damage amount of the active trap</param>
        /// <param name="hitTimeInterval">The time interval between hits</param>
        public ActiveTrap(byte x, byte y, byte damageAmount, float hitTimeInterval) : base(x, y, damageAmount)
        {
            //Setting up image and hit time interval
            image = activeTrapImage;
            timeRemaining = -0.1f;
            this.hitTimeInterval = hitTimeInterval;

            //Setting trap information output related data
            trapInfoText[0] = "Active Trap";
            trapInfoTextLocs[1] = new Vector2(886, 475);
        }

        /// <summary>
        /// Update subprogram for ActiveTrap object
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="player">The player on the trap</param>
        public override void Update(GameTime gameTime, Player player)
        {
            //Only calling update logic if trap is active
            if (IsActive)
            {
                //Inflicting damage on player, playing soundeffect, resetting time passed every hit interval
                if (timeRemaining <= 0)
                {
                    player.InflictDamage(damageAmount);
                    timeRemaining = hitTimeInterval;
                    activeTrapSoundEffect.CreateInstance().Play();
                }

                //Updating information text
                trapInfoText[1] = $"Time Until Next Hit: {Math.Round(timeRemaining, 1)}s";
                trapInfoText[2] = $"Damage Amount: {damageAmount}";

                //Calling parent update subprogram
                base.Update(gameTime, player);
            }
        }
    }
}
