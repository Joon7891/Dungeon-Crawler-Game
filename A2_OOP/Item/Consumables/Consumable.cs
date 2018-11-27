//Author: Joon Song
//Project Name: A2_OOP
//File Name: Consumable.cs
//Creation Date: 10/14/2018
//Modified Date: 10/20/2018
//Description: Class to hold Consumable object; child of Item object and parent to HealthItem and TimeItem objects

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2_OOP
{
    public abstract class Consumable : Item
    {
        /// <summary>
        /// Whether the consumable was used or not
        /// </summary>
        public bool IsUsed { get; private set; }

        /// <summary>
        /// Subprogram to use the Consumable
        /// </summary>
        /// <param name="player">The player using the consumable item</param>
        public virtual void Use(Player player)
        {
            //Setting consumable as used
            IsUsed = true;
        }
    }
}