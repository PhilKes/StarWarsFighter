using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class OptionsScreen : Menu
    {
        private MainMenu myMenu;
        

        public OptionsScreen(StarWarsFighter game, MainMenu main) : base(game)
        {
            myMenu = main;

        }
        public override void Initialize()
        {
            MenuButton tB = new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 3 + (button.Count - 2) * 50), "Debug",'c');
          
            button.Add(tB);
            MenuButton tB2 = new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, StarWarsFighter.windowHeight / 3 + (button.Count - 2) * 50), "Fullscreen", 'c');

            button.Add(tB2);
            button.Add(new MenuButton(this, new Vector2(StarWarsFighter.windowWidth / 2, 2*StarWarsFighter.windowHeight/3 + (button.Count - 2) * 50), "back",'b'));
            spriteBatch = new SpriteBatch(myGame.GraphicsDevice);
          
            if (StarWarsFighter.debug)
            {
                tB.renderTexture = myGame.Content.Load<Texture2D>("Misc/Menu/checkBoxChecked");
            }
            else
            {
                tB.renderTexture = myGame.Content.Load<Texture2D>("Misc/Menu/checkBox");
            }
            if (myGame.graphics.IsFullScreen)
            {
                tB2.renderTexture = myGame.Content.Load<Texture2D>("Misc/Menu/checkBoxChecked");
            }
            else
            {
                tB2.renderTexture = myGame.Content.Load<Texture2D>("Misc/Menu/checkBox");
            }
            base.Initialize();
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin();
            spriteBatch.DrawString(GUI.font, "Options", new Vector2(StarWarsFighter.windowWidth/2-50, StarWarsFighter.windowHeight/10), Constants.fontColor);
            spriteBatch.End();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            verticalSelect();
        }
        public override void handleButton(string type)
        {
            switch (type)
            {
                case "Debug":
                    MenuButton b = button.Find(btn => btn.name == type);
                    
                    if (!StarWarsFighter.debug)
                    {
                        b.renderTexture = myGame.Content.Load<Texture2D>("Misc/Menu/checkBoxChecked");
                        StarWarsFighter.debug = true;
                    }
                    else
                    {
                        b.renderTexture = myGame.Content.Load<Texture2D>("Misc/Menu/checkBox");
                        StarWarsFighter.debug = false;
                    }
                    break;
                case "Fullscreen":
                    MenuButton B = button.Find(btn => btn.name == type);

                    if (!myGame.graphics.IsFullScreen)
                    {
                        B.renderTexture = myGame.Content.Load<Texture2D>("Misc/Menu/checkBoxChecked");
                        myGame.graphics.IsFullScreen= true;
                    }
                    else
                    {
                        B.renderTexture = myGame.Content.Load<Texture2D>("Misc/Menu/checkBox");
                        myGame.graphics.IsFullScreen = false;
                    }
                    myGame.graphics.ApplyChanges();
                    StarWarsFighter.isFullscreen = myGame.graphics.IsFullScreen;
                    break;
                case "back":
                    myMenu.gameState = MainMenu.State.Main;
                    myMenu.optScreen = null;
                    break;
                default:
                    break;
            }
        }
    }
}
