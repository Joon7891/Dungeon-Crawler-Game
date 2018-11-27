//Author: Joon Song
//Project Name: A2_OOP
//File Name: TimeItem.cs
//Creation Date: 10/14/2018
//Modified Date: 10/18/2018
//Descrition: Class to TimeItem object; child of Consumable object

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace A2_OOP
{
    public sealed class TimeItem : Consumable
    {
        //The amount of time recovered from using the item
        private readonly byte timeRecoveryAmount;

        /// <summary>
        /// Constructor for TimeItem object
        /// </summary>
        public TimeItem()
        {
            //Randomly generating time recovery amount and value
            timeRecoveryAmount = (byte)SharedData.RNG.Next(5, 11);
            Value = (byte)(3 * timeRecoveryAmount);
        }

        /// <summary>
        /// Subprogram to use the time item
        /// </summary>
        /// <param name="player">The player using the time item</param>
        public override void Use(Player player)
        {
            //Updating player time remaining
            player.TimeLeft += timeRecoveryAmount;
            
            //Calling base use subprogram
            base.Use(player);
        }

        /// <summary>
        /// Subprogram to return TimeItem information as a string
        /// </summary>
        /// <returns>TimeItem information as a string</returns>
        public override string ToString()
        {
            //Returning object information as a string
            return $"Time Item - {timeRecoveryAmount}s Recovery";
        }
    }
}