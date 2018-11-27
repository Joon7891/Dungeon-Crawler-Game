//Author: Joon Song
//Project Name: A2_OOP
//File Name: RangedWeapon.cs
//Creation Date: 10/20/2018
//Modified Date: 10/22/2018
//Description: Class to hold RangedWeapon object; child of Weapon object

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2_OOP
{
    public sealed class RangedWeapon : Weapon
    {
        //Array of possible ranged weapon names
        private static string[] possibleNames = { "Crossbow", "Slingshot", "Spears", "Hand Cannon", "Fire Balls" };

        //Variables to hold ranged weapon specific data
        private float successOdds;

        /// <summary>
        /// Constructor for RangedWeapon object
        /// </summary>
        public RangedWeapon()
        {
            //Randonly generating ranged weapon data
            name = possibleNames[SharedData.RNG.Next(0, possibleNames.Length)];
            hitIntervalTime = SharedData.RNG.Next(250, 1001) / 1000.0f;
            damage = SharedData.RNG.Next(5, 21);
            successOdds = SharedData.RNG.Next(30, 91) / 100.0f;

            //Calling subprogram to calculate various melee weapon statistics
            CalculateStats();
        }

        /// <summary>
        /// Subprogram to calculate ranged weapon statistics
        /// </summary>
        public new void CalculateStats()
        {
            //Calling base subprogram
            base.CalculateStats();

            //Calculating value of ranged weapon
            Value = (byte)(3 * dps * successOdds + 0.5);
        }

        /// <summary>
        /// Subprogram to attack a given enemy with a ranged weapon
        /// </summary>
        /// <param name="enemy">The enemy to be attacked</param>
        public override void Attack(object enemy)
        {
            //Only attacking enemy if it's hit is successsful and enough time passed
            if (SharedData.RNG.NextDouble() <= successOdds && timePassed >= hitIntervalTime)
            {
                //Resetting time passed
                timePassed = 0.0f;

                //TO DO: ADD ENEMY HIT LOGIC
            }
        }

        /// <summary>
        /// Subprogram to format RangedWeapon information into a string
        /// </summary>
        /// <returns>Ranged weapon information as a string</returns>
        public override string ToString()
        {
            //Returning ranged weapon information as a string
            return $"{name} (Ranged Weapon) - {dps}DPS";
        }
    }
}