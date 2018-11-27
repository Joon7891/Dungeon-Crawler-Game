//Author: Joon Song
//Project Name: A2_OOP
//File Name: ShopRoom.cs
//Creation Date: 10/10/2018
//Modified Date: 10/18/2018
//Description: Class to hold ShopRoom object; child of Room object

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace A2_OOP
{
    public sealed class ShopRoom : Room
    {
        //Variables relating to room image and rectangle
        private Rectangle rectangle;
        private static Texture2D image;

        //Variables related to buying/selling function of shop
        private int cashAmount;
        private byte profitCut;
        private readonly byte inventorySize;
        private const byte ABS_MAX_SIZE = 8;
        private List<Item> inventory = new List<Item>();
        private static Vector2[] inventoryLocs = new Vector2[ABS_MAX_SIZE];
        private static Vector2[] inventoryPriceLocs = new Vector2[ABS_MAX_SIZE];
        private Player playerCache;
        private Button[] buyItemButtons;
        private static Texture2D buyButtonImage;

        //Soundeffects used by the shop
        private static SoundEffect transactionSoundEffect;
        private static SoundEffect errorSoundEffect;

        //Variables required to draw shop information
        private static string informText = string.Empty;
        private static Vector2[] headerLocs = { new Vector2(1054, 412), new Vector2(897, 457), new Vector2(1163, 457), new Vector2(1068, 522),  new Vector2(820, 495) };

        /// <summary>
        /// Static constructor to set up ShopRoom class
        /// </summary>
        static ShopRoom()
        {
            //Importing shop images
            image = Main.Content.Load<Texture2D>("Images/Sprites/Rooms/shopRoomImage");
            buyButtonImage = Main.Content.Load<Texture2D>("Images/Sprites/Buttons/buyButtonImage");

            //Setting up text location variables
            for (byte i = 0; i < ABS_MAX_SIZE; i++)
            {
                inventoryLocs[i] = new Vector2(820, 560 + 40 * i);
                inventoryPriceLocs[i] = new Vector2(1285, 560 + 40 * i);
            }

            //Impoting sound effects
            transactionSoundEffect = Main.Content.Load<SoundEffect>("Audio/SoundEffects/transactionSoundEffect");
            errorSoundEffect = Main.Content.Load<SoundEffect>("Audio/Soundeffects/errorSoundEffect");
        }

        /// <summary>
        /// Constructor for ShopRoom object
        /// </summary>
        /// <param name="x">The x coodinate of the shop room</param>
        /// <param name="y">The y coordinate of the shop room</param>
        /// <param name="doorLayout">The string containing the door layout</param>
        /// <param name="cashAmount">The starting cash amount of the shop</param>
        /// <param name="profitCut">The profit cut of the shop</param>
        /// <param name="inventorySize">The inventory size of the shop</param>
        public ShopRoom(byte x, byte y, string doorLayout, int cashAmount, byte profitCut, byte inventorySize) : base(x, y, doorLayout)
        {
            //Setting cash related data
            this.cashAmount = cashAmount;
            this.profitCut = profitCut;

            //Setting up shop inventory and buy buttons
            this.inventorySize = inventorySize;
            byte randomNumCache;
            buyItemButtons = new Button[inventorySize];
            for (byte i = 0; i < inventorySize; i++)
            {
                //Generating random number
                randomNumCache = (byte)SharedData.RNG.Next(0, 8);

                //Adding appropriate item given random number
                if (randomNumCache == 0 || randomNumCache == 1)
                {
                    inventory.Add(new HealthItem());
                }
                else if (randomNumCache == 2 || randomNumCache == 3)
                {
                    inventory.Add(new TimeItem());
                }
                else if (randomNumCache == 4)
                {
                    if (SharedData.RNG.Next(0, 2) == 0)
                    {
                        inventory.Add(new MeleeWeapon());
                    }
                    else
                    {
                        inventory.Add(new RangedWeapon());
                    }
                }
                else if (randomNumCache == 5)
                {
                    inventory.Add(new Helmet());
                }
                else
                {
                    inventory.Add(new BodyArmour());
                }

                //Constructing buy buttons
                byte iCache = i;
                buyItemButtons[i] = new Button(buyButtonImage, new Rectangle(1323, 542 + 40 * i, 65, 32), () => SellItem(inventory[iCache], playerCache));
            }

            //Setting up end room rectangle
            rectangle = new Rectangle(HorizontalEdgeBuffer + x * RoomSize + InnerImageBuffer, VerticalEdgeBuffer + y * RoomSize + InnerImageBuffer,
                InnerImageSize, InnerImageSize);
        }

        /// <summary>
        /// Update subprogram for ShopRoom object
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="player">The player in the shop room</param>
        public override void Update(GameTime gameTime, Player player)
        {
            //Calling base update subprogram
            base.Update(gameTime, player);

            //Caching player for future use
            playerCache = player;

            //Updating buy buttons
            for (byte i = 0; i < inventory.Count; i++)
            {
                buyItemButtons[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Draw subprogram for ShopRoom object
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Calling parent Room draw subprogram
            base.Draw(spriteBatch);

            //Drawing shop room image
            spriteBatch.Draw(image, rectangle, Color.White);
        }

        /// <summary>
        /// Subprogram to buy an item from the player to the shop
        /// </summary>
        /// <param name="item">The item to be bought</param>
        /// <param name="player"></param>
        public void PurchaseItem(Item item, Player player)
        {
            //Determining if shop is elidigble for purchase
            if (cashAmount < GetItemOffer(item))
            {
                //Informing user that shop does not have enough funds
                informText = "Transaction Error: Shop has Insufficient Funds";
                errorSoundEffect.CreateInstance().Play();
            }
            else if (inventory.Count == inventorySize)
            {
                //Informing user that shop does not have enough space
                informText = "Transaction Error: Shop has Insufficient Space";
                errorSoundEffect.CreateInstance().Play();
            }
            else
            {
                //Completing transaction and updating cash amounts
                inventory.Add(item);
                player.RemoveItem(item);
                player.Cash += GetItemOffer(item);
                cashAmount -= GetItemOffer(item);
                transactionSoundEffect.CreateInstance().Play();
                informText = "Transaction Completed Successfully";
            }
        }

        /// <summary>
        /// Subprogram to sell an item from the shop to a player
        /// </summary>
        /// <param name="item">The item to be sold</param>
        /// <param name="player">The player to buy the item</param>
        private void SellItem(Item item, Player player)
        {
            //Determing if player is eligible for purchase
            if (player.Cash < item.Value)
            {
                //Informing user they do not have enough funds
                informText = "Transaction Error: Player has Insufficient Funds";
                errorSoundEffect.CreateInstance().Play();
            }
            else if (!player.IsInventorySpace(item))
            {
                //Informing user they do not have enough space
                informText = "Transaction Error: Player has Insufficent Space";
                errorSoundEffect.CreateInstance().Play();
            }
            else
            {
                //Completing transaction and updating cash amounts
                player.AddItem(item);
                inventory.Remove(item);
                cashAmount += item.Value;
                player.Cash -= item.Value;
                transactionSoundEffect.CreateInstance().Play();
                informText = "Transaction Completed Successfully";
            }
        }

        /// <summary>
        /// Subprogram to output the shop's offer on a certain item
        /// </summary>
        /// <param name="item">The item to be offered</param>
        public byte GetItemOffer(Item item)
        {
            //Returning the offer the shop wil give
            return (byte)Math.Round(item.Value * profitCut / 100.0);
        }

        /// <summary>
        /// Subprogram to draw shop room information
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        public void DrawInformation(SpriteBatch spriteBatch)
        {
            //Drawing shop headers and basic information
            spriteBatch.DrawString(SharedData.InformationFonts[0], "Shop", headerLocs[0], Color.ForestGreen);
            spriteBatch.DrawString(SharedData.InformationFonts[1], $"Cash: ${cashAmount}", headerLocs[1], Color.Goldenrod);
            spriteBatch.DrawString(SharedData.InformationFonts[1], $"Max # Items: {inventorySize}", headerLocs[2], Color.White);
            spriteBatch.DrawString(SharedData.InformationFonts[1], "Items", headerLocs[3], Color.White);
            spriteBatch.DrawString(SharedData.InformationFonts[2], informText, headerLocs[4], Color.ForestGreen);

            //Drawing shop items with corresponding buttons
            for (byte i = 0; i < inventory.Count; i++)
            {
                spriteBatch.DrawString(SharedData.InformationFonts[2], $"{i + 1}) {inventory[i].ToString()}", inventoryLocs[i], Color.White);
                spriteBatch.DrawString(SharedData.InformationFonts[2], $"${inventory[i].Value}", inventoryPriceLocs[i], Color.Goldenrod);
                buyItemButtons[i].Draw(spriteBatch);
            }
        }
    }
}
