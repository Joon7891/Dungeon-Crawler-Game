//Author: Joon Song
//Project Name: A2_OOP
//File Name: KeyboardHelper.cs
//Creation Date: 09/20/2018
//Modified Date: 09/20/2018
//Desription: Class to hold various subprograms to help with keyboard functionality

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
    public static class KeyboardHelper
    {
        /// <summary>
        /// Subprogram to check if keystroke was a new one
        /// </summary>
        /// <param name="key">The key to check for new keystroke</param>
        /// <returns>Whether or whether not a keystroke is a new one</returns>
        public static bool NewKeyStroke(Keys key)
        {
            //Returing if key stroke is a new one or not
            return Main.NewKeyboard.IsKeyDown(key) && !Main.OldKeyboard.IsKeyDown(key);
        }
    }
}
