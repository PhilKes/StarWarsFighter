using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
   
    public abstract class Menu
    {
        public StarWarsFighter myGame;
        public List<MenuButton> button;
        public SpriteFont font;
        public int menuIndex;
        public int selectIndex;
        public SpriteBatch spriteBatch;
        public Menu(StarWarsFighter game)
        {
            myGame = game;
            button = new List<MenuButton>();
            font = game.Content.Load<SpriteFont>("ScoreFont");
            menuIndex = -1;
            selectIndex = 0;
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
           
        }
        public abstract void handleButton(string type);
        public virtual void Draw(GameTime gameTime)
        {
            button.ForEach(b =>
            {
                b.Draw(gameTime);
            });
        }
        public virtual void Update(GameTime gameTime)
        {
           // MouseState mouseState = Mouse.GetState();
            //GamePadState padState = GamePad.GetState(PlayerIndex.One);
            button.ForEach(b => b.Update(gameTime));
            //verticalSelect();
            if (myGame.mouseState.LeftButton == ButtonState.Pressed&&!(myGame.lastmouseState.LeftButton==ButtonState.Pressed))
            {
                button.ForEach(b =>
                {
                    b.detectMouseClick(myGame.mouseState.X, myGame.mouseState.Y);
                });
            }
            if ((myGame.newKeyStatePad.IsButtonDown(Buttons.A) && !myGame.lastKeyStatePad.IsButtonDown(Buttons.A))|| (myGame.newKeyStateKey.IsKeyDown(Keys.Enter) && !myGame.lastKeyStateKey.IsKeyDown(Keys.Enter)))
            {
                handleButton(button[selectIndex].name);
            }
        }
        public void verticalSelect()
        {
            if (myGame.newKeyStatePad.ThumbSticks.Left.Y != 0 && myGame.lastKeyStatePad.ThumbSticks.Left.Y == 0)
            {
                if (myGame.newKeyStatePad.ThumbSticks.Left.Y < 0)
                {
                    selectIndex++;
                }
                else if (myGame.newKeyStatePad.ThumbSticks.Left.Y > 0)
                {
                    selectIndex--;
                }
            }
            if (myGame.newKeyStateKey.IsKeyDown(Keys.S) && !myGame.lastKeyStateKey.IsKeyDown(Keys.S))
            {
                selectIndex++;
            }
            else if (myGame.newKeyStateKey.IsKeyDown(Keys.W) && !myGame.lastKeyStateKey.IsKeyDown(Keys.W))
            {
                selectIndex--;
            }
            if (selectIndex < 0) selectIndex = menuIndex;
            if (selectIndex > menuIndex) selectIndex = 0;
            if (button[selectIndex].name == "logo")
            {
                selectIndex = menuIndex;
            }
            if (selectIndex < 0) selectIndex = menuIndex;
            if (selectIndex > menuIndex) selectIndex = 0;
            updateButtons();
            
        }
        public void horizontalSelect()
        {
            if (myGame.newKeyStatePad.ThumbSticks.Left.X != 0 && myGame.lastKeyStatePad.ThumbSticks.Left.X==0)
            {
                if (myGame.newKeyStatePad.ThumbSticks.Left.X < 0)
                {
                    selectIndex--;
                }
                else if (myGame.newKeyStatePad.ThumbSticks.Left.X > 0)
                {
                    selectIndex++;
                }
            }
            if (myGame.newKeyStateKey.IsKeyDown(Keys.D) && !myGame.lastKeyStateKey.IsKeyDown(Keys.D))
            {
                selectIndex++;
            }
            else if (myGame.newKeyStateKey.IsKeyDown(Keys.A) && !myGame.lastKeyStateKey.IsKeyDown(Keys.A))
            {
                selectIndex--;
            }
            if (selectIndex < 0) selectIndex = menuIndex;
                if (selectIndex > menuIndex) selectIndex = 0;
                if (button[selectIndex].name == "logo")
                {
                    selectIndex = menuIndex;
                }
                if (selectIndex < 0) selectIndex = menuIndex;
                if (selectIndex > menuIndex) selectIndex = 0;
                updateButtons();
        }
        public void updateButtons()
        {
            button.ForEach(b => { b.isSelected = false; b.renderColor = Color.White; });
            button[selectIndex].isSelected = true;
            button[selectIndex].renderColor = Color.Gold;
        }
        public virtual void Initialize()
        {
            button.ForEach(b => 
            {
                b.Initialize();
                menuIndex++;
            });
            if (button[0].name != "logo")
            {
                selectIndex = 0;
            }
            else
            {
                selectIndex = 1;
            }
            updateButtons();
        }
    }
}
