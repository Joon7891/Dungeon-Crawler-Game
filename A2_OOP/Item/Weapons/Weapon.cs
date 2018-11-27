//Author: Joon Song
//Project Name: A2_OOP
//File Name: Weapon.cs
//Creation Date: 10/18/2018
//Modified Date: 10/20/2018
//Description: Class to hold Weapon object; child of Item object, parent to MeleeWeapon and RangedWeapon objects

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2_OOP
{
    public abstract class Weapon : Item
    {
        //Variables to hold weapon data 
        protected string name;
        protected float hitIntervalTime;
        protected int damage;
        protected byte dps;
        protected float timePassed = 0.0f;

        /// <summary>
        /// Subprogarm to calculate various weapon statistis
        /// </summary>
        protected void CalculateStats()
        {
            //Calculating DPS
            dps = (byte)(damage / hitIntervalTime);
        }

        /// <summary>
        /// Update subprogram for Weapon object
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public void Update(GameTime gameTime)
        {
            //Updating time passed since last attack
            timePassed += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
        }

        /// <summary>
        /// Subprogram to 'attack' a given enemy
        /// </summary>
        /// <param name="enemy">The enemy to be attacked</param>
        public virtual void Attack(object enemy) { }
    }
}
