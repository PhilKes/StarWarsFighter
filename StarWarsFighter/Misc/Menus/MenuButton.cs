using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class MenuButton
    {
        public Texture2D texture;
        public Texture2D renderTexture;
        public Vector2 texPosition;
        public Vector2 position;
        public Rectangle boundingBox;
        public SpriteBatch spriteBatch;
        public Menu myMenu;
        public string name;
        public char type;
        public Color renderColor;
        public bool isSelected;

        public MenuButton(Menu game, Vector2 pos,string pname,char ptype)
        {
            type = ptype;
            name = pname;
            position = pos;
            myMenu = game;
            isSelected = false;
            renderColor = Color.White;
        }
        public virtual  void Initialize()
        {
            spriteBatch = new SpriteBatch(myMenu.myGame.GraphicsDevice);
            LoadContent();
        }
        public virtual void Update(GameTime gameTime)
        {
        }

        public void detectMouseClick(int x, int y)
        {
            Rectangle mouseClickRectangle = new Rectangle(x, y, 6, 6);
            if (boundingBox.Intersects(mouseClickRectangle))
            {
                myMenu.handleButton(name);
            }

        }

        public virtual void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(renderTexture, texPosition, renderColor);
            if (type != 'c' && type !='l')
                spriteBatch.DrawString(GUI.btnFont, name, position-new Vector2(GUI.btnFont.MeasureString(name).Length()/2,GUI.btnFont.MeasureString(name).Y/2), Constants.fontColor);
            else if(type=='c')
                spriteBatch.DrawString(GUI.btnFont, name, texPosition- new Vector2(150,0), Constants.fontColor);
            spriteBatch.End();
        }
        public  void LoadContent()
        {
            string path = "Misc/Menu/button";
            if (type == 'l')
            { 
                for (int i = 0; i < Constants.empireShip.Count; i++)
                {
                    if (name == Constants.empireShip[i])
                    {
                        path = "Ships/Player/" + name + "/full";
                        break;
                    }
                }
                for (int i = 0; i < Constants.rebelsShip.Count; i++)
                {
                    if (name == Constants.rebelsShip[i])
                    {
                        path = "Ships/Player/" + name + "/full";
                        break;
                    }
                }
                if (path == "Misc/Menu/button")
                    path = "Misc/Menu/" + name;
            }
            if (type != 'c')
            {
                texture = myMenu.myGame.Content.Load<Texture2D>(path);
                renderTexture = texture;
            }
            texPosition = new Vector2((int)(position.X - renderTexture.Width / 2), (int)(position.Y - renderTexture.Height / 2));
            boundingBox = new Rectangle((int)texPosition.X, (int)texPosition.Y, renderTexture.Width, renderTexture.Height);
        }
    }
}
