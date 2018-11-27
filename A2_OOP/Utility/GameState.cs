//Author: Joon Song
//Project Name: A2_OOP
//File Name: GameState.cs
//Creation Date: 10/20/2018
//Modified Date: 10/20/2018
//Description: Class to hold GAmeState enum

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2_OOP
{
    /// <summary>
    /// Enum to hold different game states
    /// </summary>
    public enum GameState : byte
    {
        NotStarted,
        Playing,
        Loss,
        Win
    }
}
