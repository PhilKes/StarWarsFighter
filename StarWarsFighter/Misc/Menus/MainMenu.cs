using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class MainMenu : Menu
    {
        public StarBackground background;
        public SelectionScreen selScreen;
        public OptionsScreen optScreen;
        public State gameState;
       
        public LanScreen lanScreen;
        public enum State
        {
            Main,
            newSelection,
            Options,
            LANMenu
        }
        //public List<MenuButton> button;
        public MainMenu(StarWarsFighter game) : base(game)
        {
            
            gameState = State.Main;
            
            //button = new List<MenuButton>();
            background = new StarBackground(myGame);
            Initialize();
        }
        
        public override void Initialize()
        {
            button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 5), "logo",'l'));
           // button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 2+304/2-20), "mainmenu_background"));
            button.Add(new MenuButton(this,new Vector2(StarWarsFighter.windowWidth/2,StarWarsFighter.windowHeight/2+(button.Count-2)*50),"newGame", 'b'));
            button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 2 + (button.Count - 2) * 50), "Multiplayer", 'b'));
            button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 2+ (button.Count - 2) * 50), "options", 'b'));
            button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 2 + (button.Count - 2) * 50), "exit", 'b'));
            base.Initialize();
            background.Initialize();
            myGame.gameAudio.StopAll();
            myGame.gameAudio.AddSound("MainTheme");
            myGame.gameAudio.PlaySound("MainTheme");
            myGame.gameAudio.setAudioList(StarWarsFighter.State.inMainMenu);
        }
        public override void Update(GameTime gameTime)
        {
            verticalSelect();
            background.Update(gameTime);
            if(gameState==State.Main)base.Update(gameTime);
            else if (gameState == State.newSelection) selScreen.Update(gameTime);
            else if (gameState == State.Options) optScreen.Update(gameTime);
            else if (gameState == State.LANMenu) lanScreen.Update(gameTime);
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
          
            background.Draw(gameTime);
            if (gameState == State.Main) base.Draw(gameTime);
            else if (gameState == State.newSelection) selScreen.Draw(gameTime);
            else if (gameState == State.Options) optScreen.Draw(gameTime);
            else if (gameState == State.LANMenu) lanScreen.Draw(gameTime);
            /*button.ForEach(b =>
            {
                b.Draw(gameTime);
            });*/

        }
        public override void handleButton(string type)
        {
            switch (type)
            {
                case "newGame":
                    selScreen = new SelectionScreen(myGame,this);
                    selScreen.Initialize();
                    gameState = State.newSelection;
                    break;
                case "Multiplayer":
                    lanScreen = new LanScreen(myGame, this);
                    gameState = State.LANMenu;
                    break;
                case "options":
                    optScreen = new OptionsScreen(myGame,this);
                    optScreen.Initialize();
                    gameState = State.Options;
                    break;
                case "exit":
                    myGame.gameAudio.RemoveAll();
                    myGame.Dispose();
                    myGame.Exit();
                    break;
                default:
                    break;
            }
        }
    }
}
