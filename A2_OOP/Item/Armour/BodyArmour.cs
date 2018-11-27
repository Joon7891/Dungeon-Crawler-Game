//Author: Joon Song
//Project Name: A2_OOP
//File Name: BodyArmour.cs
//Creation Date: 10/18/2018
//Modified Date: 10/20/2018
//Description: Class to hold BodyArmour object; child to Armour object

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2_OOP
{
    public sealed class BodyArmour : Armour
    {
        //Array of possible body armour names
        private static string[] possibleNames = { "Chest Plate", "Bubble Wrap", "Cookie Sheet", "Caviar Vest", "Wood Planks" };

        /// <summary>
        /// Constructor for BodyArmour object
        /// </summary>
        public BodyArmour()
        {
            //Generating name, defense modifier, and durability
            name = possibleNames[SharedData.RNG.Next(0, possibleNames.Length)];
            armourTypeName = "Body";
            defenseModifier = (byte)SharedData.RNG.Next(15, 31);
            durability = (byte)SharedData.RNG.Next(10, 16);
            breakDefenseChange = 0.2f;

            //Updating item value
            UpdateValue();
        }
    }
}
