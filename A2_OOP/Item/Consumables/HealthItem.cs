//Author: Joon Song
//Project Name: A2_OOP
//File Name: HealthItem.cs
//Creation Date: 10/14/2018
//Modified Date: 10/18/2018
//Descrition: Class to HealthItem object; child of Consumable object

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
    public sealed class HealthItem : Consumable
    {
        //The amount of health recovered from using the item
        private readonly byte recoveryAmountPercent;

        /// <summary>
        /// Constructor for HealthItem object
        /// </summary>
        public HealthItem()
        {
            //Randomly generating health recovery amount and value
            recoveryAmountPercent = (byte)SharedData.RNG.Next(10, 31);
            Value = recoveryAmountPercent;
        }

        /// <summary>
        /// Subprogram to use the health item
        /// </summary>
        /// <param name="player"></param>
        public override void Use(Player player)
        {
            //Updating player health
            player.Health = Math.Min(player.MaxHealth, player.Health + (byte)(player.MaxHealth * recoveryAmountPercent / 100.0));

            //Calling base use subprogram
            base.Use(player);
        }

        /// <summary>
        /// Subprogram to return HealthItem information as a string
        /// </summary>
        /// <returns>HealthItem information as a string</returns>
        public override string ToString()
        {
            //Returning object information as a string
            return $"Health Item - {recoveryAmountPercent}% Recovery";
        }
    }
}
