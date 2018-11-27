//Author: Joon Song
//Project Name: A2_OOP
//File Name: MeleeWeapon.cs
//Creation Date: 10/20/2018
//Modified Date: 10/22/2018
//Description: Class to hold MeleeWeapon object; child of Weapon object

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2_OOP
{
    public sealed class MeleeWeapon : Weapon
    {
        //Array of possible melee weapon names
        private static string[] possibleNames = { "Bat", "Spiked Club", "Dead Fish", "Sword", "Hammer" };

        //Variables to hold melee weapon specific information
        private byte durability;
        private float breakOdds;

        /// <summary>
        /// Whether the melee weapon is broke or not
        /// </summary>
        public bool IsBroken { get; private set; }

        /// <summary>
        /// Constructor for MeleeWeapon object
        /// </summary>
        public MeleeWeapon()
        {
            //Randomly generating melee weapon data
            name = possibleNames[SharedData.RNG.Next(0, possibleNames.Length)];
            hitIntervalTime = SharedData.RNG.Next(1000, 3001) / 1000.0f;
            damage = SharedData.RNG.Next(20, 51);
            durability = (byte)SharedData.RNG.Next(5, 16);
            breakOdds = SharedData.RNG.Next(40, 91) / 100.0f;

            //Calling subprogram to calculate various melee weapon statistics
            CalculateStats();
        }

        /// <summary>
        /// Subprogarm to calculate melee weapon statistics
        /// </summary>
        private new void CalculateStats()
        {
            //Calling base subprogram
            base.CalculateStats();

            //Calculating value of melee wepaon
            Value = (byte)(dps * 3 + durability * 2);
        }

        /// <summary>
        /// Subprogram to attack a given enemy with a melee weapon
        /// </summary>
        /// <param name="enemy">The enemy to be attacked</param>
        public override void Attack(object enemy)
        {
            //Only attacking if enough time has passed
            if (timePassed >= hitIntervalTime)
            {
                //Resetting time passed, decreasing durability, and recalculating value
                timePassed = 0;
                durability = (byte)Math.Max(0, durability - 1);
                CalculateStats();

                //If durability is at zero determine if weapon will break
                if (durability == 0)
                {
                    //Setting item as broken
                    if (SharedData.RNG.NextDouble() <= breakOdds)
                    {
                        IsBroken = true;
                    }
                }
            }
        }

        /// <summary>
        /// Subprogram to format MeleeWeapon information into a string
        /// </summary>
        /// <returns>Melee weapon information as a string</returns>
        public override string ToString()
        {
            //Returning melee weapon information as a string
            return $"{name} (Melee Weapon) - {dps}DPS, {durability} Hits Left";
        }
    }
}
