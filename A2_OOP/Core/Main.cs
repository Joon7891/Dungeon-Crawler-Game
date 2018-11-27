//Author: Joon Song
//Project Name: A2_OOP
//File Name: Main.cs
//Creation Date: 10/09/2018
//Modified Date: 10/18/2018
//Description: Program to emulate a Dungeon crawler game

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace A2_OOP
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        //Instances of graphics device manager and sprite batch for in-game graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// Instance of ContentManeger, used for loading content
        /// </summary>
        public new static ContentManager Content { get; private set; }

        /// <summary>
        /// The mouse state of the mouse 1 frame back
        /// </summary>
        public static MouseState OldMouse { get; private set; }

        /// <summary>
        /// The mouse state of the mouse currently
        /// </summary>
        public static MouseState NewMouse { get; private set; }

        /// <summary>
        /// The keyboard state of the keyboard 1 frame back
        /// </summary>
        public static KeyboardState OldKeyboard { get; private set; }

        /// <summary>
        /// The keyboard state of the keyboard currently
        /// </summary>
        public static KeyboardState NewKeyboard { get; private set; }

        //Variables required to draw background and GUI layout
        private Texture2D backgroundImg;
        private Rectangle backgroundRect;
        private Texture2D whiteImg;
        private Rectangle[] borderRects = new Rectangle[6];

        //Variables required to draw keys and their functions
        private char[] keys = { 'N', 'W', 'A', 'S', 'D' };
        private Texture2D[] keyImages = new Texture2D[5];
        private string[] keyMessages = { "New Game", "Up", "Left", "Down", "Right" };
        private Rectangle[] keyRectangles = new Rectangle[5];
        private Vector2[] keyMessageLocs = new Vector2[5];

        //Instance of game dungeon
        private Dungeon dungeon;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content = base.Content;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Setting game dimensions and mouse as visible
            graphics.PreferredBackBufferWidth = SharedData.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SharedData.SCREEN_HEIGHT;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            //Initializing game
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Loading and setting up background and GUI layout
            backgroundImg = Content.Load<Texture2D>("Images/Backgrounds/woodBackgroundImg");
            backgroundRect = new Rectangle(0, 0, SharedData.SCREEN_WIDTH, SharedData.SCREEN_HEIGHT);
            whiteImg = Content.Load<Texture2D>("Images/Sprites/whiteImg");
            borderRects[0] = new Rectangle(Dungeon.SIZE, 0, 6, SharedData.SCREEN_HEIGHT);
            borderRects[1] = new Rectangle(SharedData.SCREEN_WIDTH - 6, 0, 6, SharedData.SCREEN_HEIGHT);
            for (byte i = 0; i < 3; i++)
            {
                borderRects[i + 2] = new Rectangle(Dungeon.SIZE, i * (SharedData.SCREEN_HEIGHT / 2 - 3) - 50 * (i == 1 ? 1 : 0), SharedData.SCREEN_WIDTH - Dungeon.SIZE, 6);
                Console.WriteLine(borderRects[3].ToString());
            }
            borderRects[5] = new Rectangle(0, Dungeon.SIZE, Dungeon.SIZE, 6);

            //Loading and setting up key related data
            for (byte i = 0; i < keyImages.Length; i++)
            {
                keyImages[i] = Content.Load<Texture2D>($"Images/Sprites/Keys/KeyImage{keys[i]}");
                keyMessageLocs[i] = new Vector2(65 + (i == 0 ? 0 : 65) + 140 * i, Dungeon.SIZE + (SharedData.SCREEN_HEIGHT - Dungeon.SIZE) / 2 - 10);
                keyRectangles[i] = new Rectangle(10 + (i == 0 ? 0 : 65) + 140 * i, Dungeon.SIZE + (SharedData.SCREEN_HEIGHT - Dungeon.SIZE) / 2 - 25, 50, 50);
            }

            //Loading dungeon from file
            dungeon = IO.LoadDungeon("dungeon1.txt");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Updating old and new keyboard and mouse states
            OldKeyboard = NewKeyboard;
            OldMouse = NewMouse;
            NewKeyboard = Keyboard.GetState();
            NewMouse = Mouse.GetState();

            //If 'N' is pressed, reset the dungeon
            if (KeyboardHelper.NewKeyStroke(Keys.N))
            {
                dungeon = IO.LoadDungeon("dungeon1.txt");
            }

            //Updating the dungeon
            dungeon.Update(gameTime);

            //Updating game
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Begin of spriteBatch; allowing sprites to be drawn
            spriteBatch.Begin();

            //Drawing background and GUI set up
            spriteBatch.Draw(backgroundImg, backgroundRect, Color.White);
            for (byte i = 0; i < borderRects.Length; i++)
            {
                spriteBatch.Draw(whiteImg, borderRects[i], Color.White);
            }

            //Drawing keys to inform user of its functions
            for (byte i = 0; i < keyMessages.Length; i++)
            {
                spriteBatch.Draw(keyImages[i], keyRectangles[i], Color.White);
                spriteBatch.DrawString(SharedData.InformationFonts[1], keyMessages[i], keyMessageLocs[i], Color.White);
            }

            //Drawing dungeon
            dungeon.Draw(spriteBatch);

            //End of spriteBatch; no longer allow sprites to be draw
            spriteBatch.End();

            //Drawing game
            base.Draw(gameTime);
        }
    }
}
