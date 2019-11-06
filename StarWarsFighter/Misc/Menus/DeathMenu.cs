using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class DeathMenu : Menu
    {
        //public StarBackground background;
        // public List<MenuButton> button;
        public enum State
        {
            Main,
            Options
        }
        public State deadMenuState = State.Main;
        public OptionsScreen optScreen;
        public DeathMenu(StarWarsFighter game) : base(game)
        {

            //button = new List<MenuButton>();
            //background = new StarBackground(myGame);
            Initialize();
        }
        public override void Initialize()
        {
            //myGame.gameAudio.AddSound("MainTheme");
            // myGame.gameAudio.PlaySound("MainTheme");
            button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 2 - 100), "dead", 'l'));
            button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 2 + button.Count * 50), "respawn", 'b'));
            button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 2 + button.Count * 50), "Change Ship", 'b'));
            button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 2 + button.Count * 50), "mainmenu", 'b'));
            
            
            //background.Initialize();
            base.Initialize();

        }
        public override void Update(GameTime gameTime)
        {
            if (deadMenuState == State.Main)
            {
                base.Update(gameTime);
                verticalSelect();
            }
            else if (deadMenuState == State.Options)
            {

            }

        }
        public override void Draw(GameTime gameTime)
        {

            if (deadMenuState == State.Main)
            {
                base.Draw(gameTime);
                SpriteBatch sp = new SpriteBatch(myGame.GraphicsDevice);
                if (!StarWarsFighter.inLAN)
                {
                    sp.Begin();
                    sp.DrawString(GUI.font, "Your Score: " + myGame.score.score, new Vector2(StarWarsFighter.windowWidth / 2 - GUI.font.MeasureString("Your Score: " + myGame.score.score).Length() / 2, StarWarsFighter.windowHeight / 2 - 60), new Color(236, 199, 96));
                    sp.End();
                }
                
            }
            else if (deadMenuState == State.Options)
            {

            }

        }
        public override void handleButton(string type)
        {
            switch (type)
            {
               
                /*case "options":
                    optScreen = new OptionsScreen(myGame, this);
                    optScreen.Initialize();
                    deadMenuState = State.Options;
                    break;*/
                case "mainmenu":
                    myGame.gameAudio.StopAll();
                    if (StarWarsFighter.inLAN)
                    {
                        StarWarsFighter.inLAN = false;
                        myGame.client.GetStream().Close();
                        myGame.client.Close();
                        //myGame.client = null;
                    }
                    StarWarsFighter.gameState = StarWarsFighter.State.inMainMenu;
                    myGame.gameAudio.playAudioList(StarWarsFighter.State.inMainMenu);
                    myGame.myMainMenu = new MainMenu(myGame);
                    break;
                case "respawn":
                    if (StarWarsFighter.inLAN)
                    {
                       StarWarsFighter.gameState = StarWarsFighter.State.inGame;
                       //myGame.initializeLANGame(myGame.player.ship.type);
                       myGame.player= new Player(myGame, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight), myGame.player.type, myGame.Content);
                       myGame.character.Add(myGame.player);
                       myGame.Components.Add(myGame.player);
                       myGame.sendRespawn();
                    }
                    else
                    {
                        StarWarsFighter.gameState = StarWarsFighter.State.inGame;
                        myGame.initializeGame(myGame.player.type);
                    }
                   
                    myGame.showMouse = false;
                    myGame.inGameTime = 0;
                    
                   
                    break;
                case "Change Ship":
                    StarWarsFighter.gameState = StarWarsFighter.State.inMainMenu;
                    myGame.myMainMenu = new MainMenu(myGame);
                    myGame.myMainMenu.gameState = MainMenu.State.newSelection;
                    myGame.myMainMenu.selScreen = new SelectionScreen(myGame, myGame.myMainMenu);
                    myGame.myMainMenu.selScreen.Initialize();
                    break;
                default:
                    break;
            }
            myGame.myDeathMenu = null;
        }
    }
}

