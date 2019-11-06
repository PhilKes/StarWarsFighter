using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace StarWarsFighter
{
    public class PauseMenu : Menu
    {
        //public StarBackground background;
        // public List<MenuButton> button;
        public enum State
        {
            Main,
            Options
        }
        public State pauseState = State.Main;
        public OptionsScreen optScreen;
        public PauseMenu(StarWarsFighter game) : base(game)
        {

            //button = new List<MenuButton>();
            //background = new StarBackground(myGame);
            Initialize();
        }
        public override void Initialize()
        {
            //myGame.gameAudio.AddSound("MainTheme");
           // myGame.gameAudio.PlaySound("MainTheme");
            button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight /2+button.Count*50), "resume", 'b'));
            //button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 2 + button.Count * 50), "options", 'b'));
            button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 2 + button.Count * 50), "mainmenu", 'b'));
            //button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 2 -60), "paused", 'l'));
            //background.Initialize();
            base.Initialize();

        }
        public override void Update(GameTime gameTime)
        {
            if (pauseState == State.Main)
            {
                base.Update(gameTime);
                verticalSelect();
            }
            else if (pauseState == State.Options)
            {

            }

            //background.Update(gameTime);
            /*MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                button.ForEach(b =>
                {
                    b.detectMouseClick(mouseState.X, mouseState.Y);
                });
            }*/
        }
        public override void Draw(GameTime gameTime)
        {
            if (pauseState == State.Main)
            {
                base.Draw(gameTime);
                spriteBatch.Begin();
                spriteBatch.DrawString(GUI.font,"Paused", new Vector2(StarWarsFighter.windowWidth / 2-45, StarWarsFighter.windowHeight / 2-60 ),Constants.fontColor);
                spriteBatch.End();
            }
            else if (pauseState == State.Options)
            {

            }
          
            //background.Draw(gameTime);
            /*button.ForEach(b =>
            {
                b.Draw(gameTime);
            });*/
        }
        public override void handleButton(string type)
        {
            switch (type)
            {
                case "resume":
                    //myGame.gameAudio.StopSound("MainTheme");
                    StarWarsFighter.gameState = StarWarsFighter.State.inGame;
                    myGame.showMouse = false;
                    myGame.gameAudio.playAudioList(StarWarsFighter.State.inGame);
                    //myGame.gameAudio.PlaySound("Music");
                    //myMenu.myGame.initializeGame();
                    // myGame.gameAudio.PlaySound("Music");
                    break;

                case "options":
                    /*optScreen = new OptionsScreen(myGame, this);
                    optScreen.Initialize();
                    pauseState = State.Options;*/
                    break;
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
                default:
                    break;
            }
        }
    }
}
