//Author: Joon Song
//Project Name: A2_OOP
//File Name: Button.cs
//Creation Date: 09/10/2018
//Modified Date: 09/18/2018
//Desription: Class to hold Button, an object that represents an on-screen button

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
    public sealed class Button
    {
        //Delegate and instance of delegate to hold button behavior
        public delegate void OnClick();
        private OnClick onClick;

        //Variables to hold the image and rectangular dimensions of button
        private Texture2D image;
        private Rectangle rect;

        //Variable to hold information on if mouse is hovering over button
        private bool isMouseHovering;

        /// <summary>
        /// Constructor for button object
        /// </summary>
        /// <param name="image">The image of the button</param>
        /// <param name="rect">The rectangle representing the rectangular dimensions of the button</param>
        /// <param name="onClick">The behavior of the button when clicked</param>
        public Button(Texture2D image, Rectangle rect, OnClick onClick)
        {
            //Setting up button variables from parameters
            this.onClick = onClick;
            this.image = image;
            this.rect = rect;
        }

        /// <summary>
        /// Update various button components
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public void Update(GameTime gameTime)
        {
            //Updating variable on if mouse is hovering over button
            isMouseHovering = CollisionDetection.PointToRect(Main.NewMouse.Position.ToVector2(), rect);

            //Invoking button behvior if button is clicked
            if (MouseHelper.NewClick() && isMouseHovering)
            {
                onClick();
            }
        }

        /// <summary>
        /// Draws the button
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Drawing button; transparency is higher if mouse is not hovering over button
            spriteBatch.Draw(image, rect, Color.White * (0.6f + 0.4f * Convert.ToByte(isMouseHovering)));
        }
    }
}
