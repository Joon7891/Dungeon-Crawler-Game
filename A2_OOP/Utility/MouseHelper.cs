//Author: Joon Song
//Project Name: A2_OOP
//File Name: MosueHelper.cs
//Creation Date: 09/10/2018
//Modified Date: 09/10/2018
//Desription: Class to hold various subprograms to help with mouse functionality

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A2_OOP
{
    public static class MouseHelper
    {
        /// <summary>
        /// Subprogram to check if a mouse click was a new one
        /// </summary>
        /// <returns>Whether or whether not a mouse click was a new one</returns>
        public static bool NewClick()
        {
            //Returing true of mouse click is detected as a new one
            if (Main.NewMouse.LeftButton == ButtonState.Pressed && Main.OldMouse.LeftButton != ButtonState.Pressed)
            {
                return true;
            }

            //Otherwise returning false
            return false;
        }
    }
}