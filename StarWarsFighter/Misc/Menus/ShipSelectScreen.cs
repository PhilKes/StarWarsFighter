using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class ShipSelectScreen : Menu
    {
        public List<string> shipsSide;
        public SelectionScreen selScreen;
        public ShipSelectScreen(StarWarsFighter game,char side,SelectionScreen sel) : base(game)
        {
            selScreen = sel;
            if (side == 'r') shipsSide = Constants.rebelsShip;
            else if (side == 'e') shipsSide = Constants.empireShip;
        }
        public override void Initialize()
        {
            button.Add(new MenuButton(this, new Vector2((StarWarsFighter.windowWidth / 10), (5 * (StarWarsFighter.windowHeight / 6))), "back",'b'));
            for (int i = 0; i < shipsSide.Count; i++)
            {
                button.Add(new MenuButton(this, new Vector2((i+ 1) * StarWarsFighter.windowWidth / 5, (int)StarWarsFighter.windowHeight / 2), shipsSide[i],'l'));
            }
            base.Initialize();
            selectIndex = 0;
            updateButtons();
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            selScreen.spriteBatch.Begin();
            selScreen.spriteBatch.DrawString(font,"Choose your ship",
                        new Vector2(StarWarsFighter.windowWidth/2-font.MeasureString("Choose your ship").X/2,StarWarsFighter.windowHeight/8), Constants.fontColor);
            for (int i = 0; i < button.Count; i++)
            {
                if (button[i].name != "back")
                {
                    selScreen.spriteBatch.DrawString(font,button[i].name,
                        new Vector2(button[i].texPosition.X, button[i].texPosition.Y - font.MeasureString(button[i].name).Y-10),Constants.fontColor);
                }
            }
            selScreen.spriteBatch.End();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            horizontalSelect();
        }
        public override void handleButton(string type)
        {
            if (type == "back")
            {
                selScreen.menuState = SelectionScreen.State.inMainScreen;
                selScreen.shipScreen = null;
            }
            else
            {
                startGame(type);
            } 
        }
        private void startGame(string type)
        {
            myGame.showMouse = false;
            myGame.inGameTime = 0;
            
            if (StarWarsFighter.inLAN)
            {
                StarWarsFighter.gameState = StarWarsFighter.State.inLANGame;
                if (!myGame.client.Connected)
                {

                    if (!myGame.connect2Server())
                    {
                        string path = Directory.GetParent(myGame.Content.RootDirectory).FullName + "/Server/RelayServer.exe";

                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = path;
                        startInfo.WindowStyle = ProcessWindowStyle.Minimized;
                        Process.Start(startInfo);
                        //myGame.initializeLANGame("Xwing",true);
                        myGame.connect2Server();
                        // mainMenu = null;
                    }
                    else
                    {
                        //myGame.initializeLANGame("Xwing",false);
                    }
                    myGame.initializeLANGame(type, true);
                }
                else
                {
                    myGame.player=new Player(myGame, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight), type, myGame.Content);
                    //myGame.player.Initialize();
                    myGame.character.Add(myGame.player);
                    myGame.Components.Add(myGame.player);
                    myGame.gameAudio.StopAll();
                    myGame.gameAudio.playAudioList(StarWarsFighter.State.inGame);
                }
                
                myGame.sendRespawn();
                myGame.player.type = type;
            }
            else
            {
                StarWarsFighter.gameState = StarWarsFighter.State.inGame;
                myGame.initializeGame(type);
            }
           

            myGame.myMainMenu = null;

        }
    }
}
