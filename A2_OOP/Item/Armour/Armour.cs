//Author: Joon Song
//Project Name: A2_OOP
//File Name: Armour.cs
//Creation Date: 10/18/2018
//Modified Date: 10/20/2018
//Description: Class to hold Armour object; child of Item object and parent to BodyArmour and Helmet objects

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace A2_OOP
{
    public abstract class Armour : Item
    {
        //Variables to hold armour data
        protected string name;
        protected string armourTypeName;
        protected byte defenseModifier;
        protected byte durability;
        protected float breakDefenseChange;
        
        //SoundEffect objects to hold various sound effects
        private static SoundEffect breakSoundEffect;
        
        /// <summary>
        /// Static constructor to set up Armour class
        /// </summary>
        static Armour()
        {
            //Loading in various sound effects
            breakSoundEffect = Main.Content.Load<SoundEffect>("Audio/SoundEffects/armourBreakSoundEffect");
        }

        /// <summary>
        /// Subprogram to update the value of the armour
        /// </summary>
        protected void UpdateValue()
        {
            //Updating value of armour item
            Value = (byte)(3 * defenseModifier + durability);
        }

        /// <summary>
        /// Subprogram to 'use' the armount
        /// </summary>
        /// <param name="damageAmount">The original damage amount</param>
        /// <returns>The adjusted damage amount</returns>
        public byte Use(byte damageAmount)
        {
            //Calculating new damage amoutn
            byte newDamageAmount = (byte)Math.Max(1, damageAmount - defenseModifier);

            //Adjusting durability and making approriate updates if weapon breaks
            if (durability > 0 && --durability == 0)
            {
                breakSoundEffect.CreateInstance().Play();
                defenseModifier = (byte)(defenseModifier * breakDefenseChange + 0.5);
                UpdateValue();
            }

            //Returning the new damage amount
            return newDamageAmount;
        }

        /// <summary>
        /// Subprogram to format armour information into a string
        /// </summary>
        /// <returns>Armour information in a string</returns>
        public override string ToString()
        {
            //Returning armour infomration as a string
            return $"{name} ({armourTypeName}) - {defenseModifier} Defense, {durability} Hits Left";
        }
    }
}
