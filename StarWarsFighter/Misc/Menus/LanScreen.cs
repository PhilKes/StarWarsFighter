using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.IO;

namespace StarWarsFighter
{
    public class LanScreen : Menu
    {
        private MainMenu mainMenu;
        private StarWarsFighter myGame;

        public LanScreen(StarWarsFighter myGame, MainMenu mainMenu) : base(myGame)
        {
            this.myGame = myGame;
            this.mainMenu = mainMenu;

           
            //myGame.connect2Server();
            button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth/2, StarWarsFighter.windowHeight/2), "Join Server", 'b'));
            button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth /2, StarWarsFighter.windowHeight - StarWarsFighter.windowHeight/4), "back", 'b'));
            myGame.client = new System.Net.Sockets.TcpClient();
            Initialize();
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin();
            //spriteBatch.DrawString(GUI.font,"LAN Multiplayer Menu",new Vector2(StarWarsFighter.windowWidth/2-200,100),Color.Gold);
            spriteBatch.DrawString(font, "LAN Multiplayer Menu",
                       new Vector2(StarWarsFighter.windowWidth / 2 - font.MeasureString("LAN Multiplayer Menu").X / 2, StarWarsFighter.windowHeight / 8), Constants.fontColor);
            spriteBatch.End();
        }
        public override void Update(GameTime gameTime)
        {
             verticalSelect();
             base.Update(gameTime);
            //myGame.connect2Server();
        }
        public override void handleButton(string type)
        {
            switch (type)
            {
                case "Join Server":

                    StarWarsFighter.inLAN = true;
                    mainMenu.gameState = MainMenu.State.newSelection;
                    mainMenu.selScreen = new SelectionScreen(myGame, mainMenu);
                    mainMenu.selScreen.Initialize();
                    mainMenu.lanScreen = null;
                    break;
                case "back":
                    mainMenu.gameState = MainMenu.State.Main;
                    
                    mainMenu.lanScreen = null;
                    break;
                default:
                    break;
            }
            
        }
    }
}