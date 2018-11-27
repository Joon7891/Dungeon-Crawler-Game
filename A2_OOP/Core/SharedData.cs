//Author: Joon Song
//Project Name: A2_OOP
//File Nae: SharedData.cs
//Creation Date: 10/10/2018
//Modified Date: 10/15/2018
//Description: Class to hold various shared data

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace A2_OOP
{
    public static class SharedData
    {
        /// <summary>
        /// Random number generator
        /// </summary>
        public static Random RNG { get; }
        
        /// <summary>
        /// The width of the screen
        /// </summary>
        public const int SCREEN_WIDTH = 1400;

        /// <summary>
        /// The height of the screen
        /// </summary>
        public const int SCREEN_HEIGHT = 900;

        /// <summary>
        /// Array of information fonts; can be indexed from 0 to 2, inclusive
        /// </summary>
        public static SpriteFont[] InformationFonts { get; }
        
        /// <summary>
        /// Static constructor to set up SharedData class
        /// </summary>
        static SharedData()
        {
            //Setting up random number generator
            RNG = new Random();

            //Setting up infomration fonts
            InformationFonts = new SpriteFont[3];
            for (byte i = 0; i < InformationFonts.Length; i++)
            {
                InformationFonts[i] = Main.Content.Load<SpriteFont>($"Fonts/InformationFonts/InformationFont{i}");
            }
        }
    }
}
