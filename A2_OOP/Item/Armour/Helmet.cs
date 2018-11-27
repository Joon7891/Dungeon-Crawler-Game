//Author: Joon Song
//Project Name: A2_OOP
//File Name: Helmet.cs
//Creation Date: 10/18/2018
//Modified Date: 10/20/2018
//Description: Class to hold Helmet object; child to Armour object

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2_OOP
{
    public sealed class Helmet : Armour
    {
        //Array of possible helmet armour names
        private static string[] possibleNames = { "Bucket", "Military Cap", "Bike Helmet", "Sports Cap", "Hoodie" };

        /// <summary>
        /// Constructor for Helmet object
        /// </summary>
        public Helmet()
        {
            //Generating helmet information
            name = possibleNames[SharedData.RNG.Next(0, possibleNames.Length)];
            armourTypeName = "Helmet";
            defenseModifier = (byte)SharedData.RNG.Next(10, 21);
            durability = (byte)SharedData.RNG.Next(5, 11);
            breakDefenseChange = 0.25f;

            //Updating item value
            UpdateValue();
        }
    }
}
