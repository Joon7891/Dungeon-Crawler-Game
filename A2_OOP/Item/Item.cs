//Author: Joon Song
//Project Name: A2_OOP
//File Name: Item.cs
//Creation Date: 10/16/2018
//Modified Date: 10/16/2018
//Description: Class to hold Item object; parent to Weapon, Armour, and Consumable objects

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2_OOP
{
    public abstract class Item
    {
        /// <summary>
        /// The value of the item
        /// </summary>
        public byte Value { get; protected set; }
    }
}