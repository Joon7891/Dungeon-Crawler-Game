//Author: Joon Song
//Project Name: A2_OOP
//File Name: Player.cs
//Creation Date: 10/09/2018
//Modified Date: 10/20/2018
//Description: Class to hold Player object

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace A2_OOP
{
    public sealed class Player
    {
        /// <summary>
        /// The x coodinate of the player
        /// </summary>
        public byte X { get; private set; }

        /// <summary>
        /// The y coordinate of the player
        /// </summary>
        public byte Y { get; private set; }

        /// <summary>
        /// The max health of the player
        /// </summary>
        public int MaxHealth { get; }

        /// <summary>
        /// The health of the player
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// The cash amount of the player
        /// </summary>
        public int Cash { get; set; }

        /// <summary>
        /// The amount of time that is left
        /// </summary>
        public double TimeLeft { get; set; }

        /// <summary>
        /// The current game state of the player, represented in an enum
        /// </summary>
        public GameState CurrentGameState { get; private set; }

        //Variables to hold the player's start location
        private readonly Vector2 startLoc;

        //Player buttons
        private static Texture2D useButtonImage;
        private static Texture2D sellButtonImage;
        private Button[] sellItemButtons = new Button[6];
        private Button[] useItemButtons = new Button[3];

        //Player inventory and related data
        private Item[] inventory = new Item[6];
        private string[] inventoryText = new string[6];
        private static Vector2[] inventoryLocs = new Vector2[6];
        private static Vector2[] inventoryPriceLocs = new Vector2[6];
        private static string[] inventoryReplaceText = { "NO WEAPON", "NO HELMET", "NO BODY ARMOUR", "EMPTY", "EMPTY", "EMPTY" };

        //Cache of current room
        private Type currentRoomType;
        private ShopRoom shopRoomCache;

        //Variables to hold imaages and rectangle of player
        private Rectangle rectangle;
        private Texture2D image;
        private static Texture2D[] directionalImages = new Texture2D[4];

        //Variables required to draw player information
        private static Vector2[] headerLocs = 
        {
            new Vector2(Dungeon.SIZE + 244, 15),
            new Vector2(Dungeon.SIZE + 24, 60),
            new Vector2(Dungeon.SIZE + 238, 60),
            new Vector2(Dungeon.SIZE + 380, 60),
            new Vector2(Dungeon.SIZE + 244, 105)
        };

        //Objects to hold various sound effects
        private static SoundEffect wallHitSoundEffect;

        /// <summary>
        /// Static constructor to set up Player class
        /// </summary>
        static Player()
        {
            //Setting up inventory locations
            for (byte i = 0; i < inventoryLocs.Length; i++)
            {
                inventoryLocs[i] = new Vector2(Dungeon.SIZE + 20, 145 + 40 * i);
                inventoryPriceLocs[i] = new Vector2(Dungeon.SIZE + 485, 145 + 40 * i);
            }

            //Importing various images
            for (Direction direction = Direction.Up; direction <= Direction.Left; direction++)
            {
                directionalImages[(byte)direction] = Main.Content.Load<Texture2D>($"Images/Sprites/Player/player{direction.ToString()}image");
            }
            useButtonImage = Main.Content.Load<Texture2D>("Images/Sprites/Buttons/useButtonImage");
            sellButtonImage = Main.Content.Load<Texture2D>("Images/Sprites/Buttons/sellButtonImage");

            //Importing sound effects
            wallHitSoundEffect = Main.Content.Load<SoundEffect>("Audio/SoundEffects/thudSoundEffect");
        }

        /// <summary>
        /// Constructor for Player object
        /// </summary>
        /// <param name="x">The x coodinate of the player</param>
        /// <param name="y">The y coordinate of the player</param>
        /// <param name="cash">The amount of cash the player has</param>
        /// <param name="health">The amount of health the player has</param>
        /// <param name="timeLeft">The amount of time left for the player</param>
        public Player(byte x, byte y, int cash, int health, double timeLeft)
        {
            //Setting up player properties
            startLoc = new Vector2(x, y);
            X = x;
            Y = y;
            Cash = cash;
            MaxHealth = health;
            Health = health;
            TimeLeft = timeLeft;

            //Setting up consumable item 'use' buttons
            for (byte i = 0; i < useItemButtons.Length; i++)
            {
                byte iCache = i;
                useItemButtons[i] = new Button(useButtonImage, new Rectangle(Dungeon.SIZE + 523, 257 + 40 * i, 65, 32), () => ((Consumable)inventory[iCache + 3])?.Use(this));
            }

            //Setting up sell item buttons
            for (byte i = 0; i < sellItemButtons.Length; i++)
            {
                byte iCache = i;
                sellItemButtons[i] = new Button(sellButtonImage, new Rectangle(Dungeon.SIZE + 523, 137 + 40 * i, 65, 32), () => shopRoomCache.PurchaseItem(inventory[iCache], this));
            }

            //Setting up player image and rectangle
            image = directionalImages[0];
            rectangle = new Rectangle(Room.HorizontalEdgeBuffer + x * Room.RoomSize + Room.InnerImageBuffer, Room.VerticalEdgeBuffer + y * Room.RoomSize + Room.InnerImageBuffer,
                Room.InnerImageSize, Room.InnerImageSize);
        }

        /// <summary>
        /// Update subprogram for Player object
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="currentRoom">The current room of the player</param>
        public void Update(GameTime gameTime, Room currentRoom)
        {
            //Caching current room type and game state
            currentRoomType = currentRoom.GetType();
            UpdateGameState();

            //Updating time remaining if current room is a passage room
            if (currentRoomType == typeof(PassageRoom) && CurrentGameState == GameState.Playing)
            {
                TimeLeft -= gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            }

            //Calling move subprogram with appropriate parameters if direction is pressed
            if (KeyboardHelper.NewKeyStroke(Keys.W))
            {
                Move(Direction.Up, currentRoom.IsTopOpen, currentRoom);
            }
            else if (KeyboardHelper.NewKeyStroke(Keys.A))
            {
                Move(Direction.Left, currentRoom.IsLeftOpen, currentRoom);
            }
            else if (KeyboardHelper.NewKeyStroke(Keys.S))
            {
                Move(Direction.Down, currentRoom.IsBottomOpen, currentRoom);
            }
            else if (KeyboardHelper.NewKeyStroke(Keys.D))
            {
                Move(Direction.Right, currentRoom.IsRightOpen, currentRoom);
            }

            //Calling subprogram to update inventory
            UpdateInventory(gameTime, currentRoom);
        }

        /// <summary>
        /// Draw subprogram for Player object
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Drawing player
            spriteBatch.Draw(image, rectangle, Color.White);

            //Calling subprogram to draw player information
            DrawInformation(spriteBatch);

            //Calling subprogram to draw shop information; if current room is a shop room
            if (currentRoomType == typeof(ShopRoom))
            {
                shopRoomCache.DrawInformation(spriteBatch);
            }
        }

        /// <summary>
        /// Subprogram to move the player around the map
        /// </summary>
        /// <param name="direction">The direction of the movement</param>
        /// <param name="canMove">The condition needing to be met for movement</param>
        /// <param name="currentRoom">The current room the player is in</param>
        private void Move(Direction direction, bool canMove, Room currentRoom)
        {
            //Storing direction in a numerical form
            byte directionNum = (byte)direction;

            //Making apporiate updates if player is able to move
            if (canMove)
            {
                if (currentRoom is PassageRoom)
                {
                    ((PassageRoom)currentRoom).LeaveRoom();
                }
                X += (byte)((2 - directionNum) % 2);
                Y += (byte)((directionNum - 1) % 2);
                rectangle.X += (2 - directionNum) % 2 * Room.RoomSize;
                rectangle.Y += (directionNum - 1) % 2 * Room.RoomSize;
            }
            else
            {
                //Playing wall hit sound effect
                wallHitSoundEffect.CreateInstance().Play();
            }

            //Updating image to appropriate direction
            image = directionalImages[directionNum];
        }

        /// <summary>
        /// Subprogram to update various inventory components
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="currentRoom">The current room of the player</param>
        private void UpdateInventory(GameTime gameTime, Room currentRoom)
        {
            //Updating weapon, if it exists
            if (inventory[0] != null)
            {
                ((Weapon)inventory[0]).Update(gameTime);
            }
            
            //If a consumable is used, remove it
            for (byte i = 3; i < inventory.Length; i++)
            {
                if (inventory[i] != null && ((Consumable)inventory[i]).IsUsed)
                {
                    inventory[i] = null;
                }
            }

            //If melee weapon is broken, remove it
            if (inventory[0] != null && inventory[0] is MeleeWeapon && ((MeleeWeapon)inventory[0]).IsBroken)
            {
                inventory[0] = null;
            }

            //Updating inventory text
            for (byte i = 0; i < inventoryText.Length; i++)
            {
                inventoryText[i] = inventory[i] == null ? inventoryReplaceText[i] : inventory[i].ToString();
            }

            //Calling appropriate update logic given type of room
            if (currentRoomType == typeof(PassageRoom))
            {
                //Updating consumable item buttons; if cooresponding item exists
                for (byte i = 0; i < useItemButtons.Length; i++)
                {
                    if (inventory[i + 3] != null)
                    {
                        useItemButtons[i].Update(gameTime);
                    }
                }
            }
            else if (currentRoomType == typeof(ShopRoom))
            {
                //Setting current room as shop room
                shopRoomCache = (ShopRoom)currentRoom;

                //Updating sell buttons; if corresponding item exists
                for (byte i = 0; i < sellItemButtons.Length; i++)
                {
                    if (inventory[i] != null)
                    {
                        sellItemButtons[i].Update(gameTime);
                    }
                }
            }
        }

        /// <summary>
        /// Subprogram to add an item to player inventory
        /// </summary>
        /// <param name="item">The item to be added</param>
        public void AddItem(Item item)
        {
            //Determing the type of item that is to be added; note switch statements cannot be used as "A constant value is expected"
            if (item is Weapon)
            {
                //Assigning weapon to appropriate inventory slot
                inventory[0] = item;
            }
            else if (item is Helmet)
            {
                //Assigning helmet to appropriate inventory slot
                inventory[1] = item;

            }
            else if (item is BodyArmour)
            {
                //Assigning body armour to appropriate inventory slot
                inventory[2] = item;
            }
            else
            {
                //If there is an open space in consumable slots
                for (byte i = 3; i < inventory.Length; i++)
                {
                    if (inventory[i] == null)
                    {
                        inventory[i] = item;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Subprogram to remove an item to player's inventory
        /// </summary>
        /// <param name="item">The item to be removed</param>
        public void RemoveItem(Item item)
        {
            //Removing item and exiting program if it is in inventory
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == item)
                {
                    inventory[i] = null;
                    return;
                }
            }

            //Otherwise throw a missing member exception
            throw new MissingMemberException();
        }

        /// <summary>
        /// Subprogram to determine if there is space in the inventory
        /// </summary>
        /// <param name="itemType">The item to check for space</param>
        /// <returns>Whether there is space for the new item</returns>
        public bool IsInventorySpace(Item item)
        {
            //If item is if weapon or armour type, return true
            if (item is Armour || item is Weapon)
            {
                return true;
            }

            //Returning true of there is space in consuamble slots
            for (byte i = 3; i < inventory.Length; i++)
            {
                if (inventory[i] == null)
                {
                    return true;
                }
            }

            //Otherwise return false
            return false;
        }

        /// <summary>
        /// Subprogram to inflict damage to the player
        /// </summary>
        /// <param name="damageAmount">The amount of damage to inflict</param>
        public void InflictDamage(byte damageAmount)
        {
            //Determing final damage and inflicting damage
            byte finalDamageAmount = damageAmount;
            finalDamageAmount = (inventory[1] == null) ? finalDamageAmount : ((Armour)inventory[1]).Use(finalDamageAmount);
            finalDamageAmount = (inventory[2] == null) ? finalDamageAmount : ((Armour)inventory[2]).Use(finalDamageAmount);
            Health = Math.Max(0, Health - finalDamageAmount);
        }

        /// <summary>
        /// Subprogram to draw various player information
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw Sprites</param>
        private void DrawInformation(SpriteBatch spriteBatch)
        {
            //Drawing text containing relevant player information
            spriteBatch.DrawString(SharedData.InformationFonts[0], "Player", headerLocs[0], Color.ForestGreen);
            spriteBatch.DrawString(SharedData.InformationFonts[1], $"Health: {Health}/{MaxHealth}", headerLocs[1], Color.Red);
            spriteBatch.DrawString(SharedData.InformationFonts[1], $"Cash: ${Cash}", headerLocs[2], Color.Goldenrod);
            spriteBatch.DrawString(SharedData.InformationFonts[1], $"Time Left: {Math.Round(TimeLeft, 1)}s", headerLocs[3], Color.White);

            //Drawing text containing inventory information
            spriteBatch.DrawString(SharedData.InformationFonts[1], "Inventory", headerLocs[4], Color.White);
            for (byte i = 0; i < inventory.Length; i++)
            {
                spriteBatch.DrawString(SharedData.InformationFonts[2], $"{i + 1}) " + inventoryText[i], inventoryLocs[i], Color.White);
            }

            //Drawing buttons
            DrawButtons(spriteBatch);
        }

        /// <summary>
        /// Subprogram to draw inventory related buttons
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw Sprites</param>
        private void DrawButtons(SpriteBatch spriteBatch)
        {
            //Calling appropriate draw logic given type of room
            if (currentRoomType == typeof(PassageRoom))
            {
                //Drawing consumable item buttons; if cooresponding item exists
                for (byte i = 0; i < useItemButtons.Length; i++)
                {
                    if (inventory[i + 3] != null)
                    {
                        useItemButtons[i].Draw(spriteBatch);
                    }
                }
            }
            else if (currentRoomType == typeof(ShopRoom))
            {
                //Drawing sell item buttons with prices; if corresponding item exists
                for (byte i = 0; i < sellItemButtons.Length; i++)
                {
                    if (inventory[i] != null)
                    {
                        spriteBatch.DrawString(SharedData.InformationFonts[2], $"${shopRoomCache.GetItemOffer(inventory[i])}", inventoryPriceLocs[i], Color.Goldenrod);
                        sellItemButtons[i].Draw(spriteBatch);
                    }
                }
            }
        }

        /// <summary>
        /// Subprogram to return game state to the dungeon
        /// </summary>
        private void UpdateGameState()
        {
            //Determining current game state and updating property
            if (currentRoomType == typeof(EndRoom))
            {
                CurrentGameState = GameState.Win;
            }
            else if (Health <= 0 || TimeLeft <= 0)
            {
                CurrentGameState = GameState.Loss;
            }
            else if (CurrentGameState == GameState.NotStarted && X == startLoc.X && Y == startLoc.Y)
            {
                CurrentGameState = GameState.NotStarted;
            }
            else
            {
                CurrentGameState = GameState.Playing;
            }
        }
    }
}
