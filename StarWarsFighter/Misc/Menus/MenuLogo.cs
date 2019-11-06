using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
   
    public class MenuLogo: MenuButton
    {
        public static int frameMax = 6;
        public float timeElapsedTotal;
        public float frameDur;
        public float frameCount;
        public int frame;
        public bool mousewasOver;
        public MenuLogo(Menu menu,Vector2 pos,string ptype,char c) :base(menu,pos,ptype,c)
        {
            frame = 0;
            frameDur = 150;
            timeElapsedTotal = 0f;
            frameCount = 0;
            mousewasOver = false;
        }
        public override void Update(GameTime gameTime)
        {
            if (boundingBox.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 5, 5)) ||isSelected)
            {
                if(!mousewasOver)
                    mousewasOver = true;

                timeElapsedTotal += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                //base.Update(gameTime);
                if (timeElapsedTotal >= frameDur)
                {
                    frame++;
                    if (frame >= frameMax)
                    {
                        frame = 0;
                    }
                    timeElapsedTotal -= timeElapsedTotal;
                }
            }
            else if(mousewasOver)
            {
                frame = 0;
                mousewasOver = false;
            }
        }
        public override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(myMenu.myGame.GraphicsDevice);
           
            texPosition = new Vector2(position.X-renderTexture.Width/12,position.Y-renderTexture.Height/2);
            boundingBox = new Rectangle((int)texPosition.X, (int)texPosition.Y, (int)(renderTexture.Width / 6), (int)(renderTexture.Height));
        }
        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            spriteBatch.Begin();
            spriteBatch.Draw(renderTexture, texPosition, new Rectangle(frame*(renderTexture.Width / 6),0,(int)(renderTexture.Width/6),(int)(renderTexture.Height)),Color.White);
            spriteBatch.End();
        }
    }
}
