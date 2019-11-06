using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class SelectionScreen : Menu
    {
        public MainMenu myMenu;
        public SpriteFont font;
        public SpriteBatch spriteBatch;
        public ShipSelectScreen shipScreen;
        public State menuState;
        public enum State
        {
            inMainScreen,
            inShipSelection,
        }
        public SelectionScreen(StarWarsFighter game,MainMenu menu) : base(game)
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            font = game.Content.Load<SpriteFont>("ScoreFont");
            myMenu = menu;
            menuState = State.inMainScreen;
           
            //button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 3, StarWarsFighter.windowHeight / 2), "rebels"));
            //button.Add(new MenuButton(this, new Vector2(2 * (StarWarsFighter.windowWidth / 3), (StarWarsFighter.windowHeight / 2)), "empire"));
           
        }
        public override void Update(GameTime gameTime)
        {
            if (menuState == State.inMainScreen)
            {
                base.Update(gameTime);
                horizontalSelect();
            }
            else if (menuState==State.inShipSelection)
            {
                shipScreen.Update(gameTime);
            }
        }
        public override void Initialize()
        {
            button.Add(new MenuButton(this, new Vector2((StarWarsFighter.windowWidth / 10), (5 * (StarWarsFighter.windowHeight / 6))), "back", 'b'));
            button.Add(new MenuLogo(this, new Vector2(StarWarsFighter.windowWidth / 3, StarWarsFighter.windowHeight / 3), "rebelsLogo", 'l'));
            button.Add(new MenuLogo(this, new Vector2(2 * (StarWarsFighter.windowWidth / 3), (StarWarsFighter.windowHeight / 3)), "empireLogo", 'l'));

            base.Initialize();
            selectIndex = 0;
            updateButtons();
        }
        public override void handleButton(string type)
        {
            switch (type)
            {
                case "rebelsLogo":
                case "rebels":
                    //Console.WriteLine("Rebelscum");
                    //startGame(0);
                    Enemy.side = "Empire";
                    shipScreen = new ShipSelectScreen(myGame,'r',this);
                    shipScreen.Initialize();
                    menuState = State.inShipSelection;
                    break;
                case "empireLogo":
                case "empire":
                    //Console.WriteLine("Empire");
                    //startGame(1);
                    Enemy.side = "Rebels";
                    shipScreen = new ShipSelectScreen(myGame,'e',this);
                    shipScreen.Initialize();
                    menuState = State.inShipSelection;
                    break;
                case"back":
                    myMenu.gameState = MainMenu.State.Main;
                    StarWarsFighter.inLAN = false;
                    myMenu.selScreen = null;
                    break;
            }
        }
        public override void Draw(GameTime gameTime)
        {
          
            if (menuState == State.inMainScreen)
            {
                base.Draw(gameTime);
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Choose your side", new Vector2(StarWarsFighter.windowWidth / 2 - 150, StarWarsFighter.windowHeight / 7), Constants.fontColor);
                spriteBatch.End();
            }
            else if (menuState == State.inShipSelection)
            {
                shipScreen.Draw(gameTime);
            }
        }
       

    }
}
