using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class StarBackground : DrawableGO
    {
        private Vector2 bgPos1, bgPos2;
        private int speed;
        private int endHeight;

        //Constructor
        public StarBackground(StarWarsFighter game) : base(game)
        {
            //texture = null;
            // width = windowWidth;
            //spriteBatch = sprite;
            myGame = game;
            endHeight = StarWarsFighter.windowHeight;
            speed = 2;
            spriteBatch = (SpriteBatch)myGame.Services.GetService(typeof(SpriteBatch));
        }

        protected override void LoadContent()
        {
            texture.Add(Game.Content.Load<Texture2D>("Misc/space_full"));
            renderTexture = texture[0];
        }
        public override void Initialize()
        { 
             LoadContent();
            base.Initialize();
           
            bgPos1 = new Vector2(0, 0);
            bgPos2 = new Vector2(0, -renderTexture.Height+2);
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            DrawOrder = 1;
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin();
            spriteBatch.Draw(renderTexture, bgPos1, Color.White);
            spriteBatch.Draw(renderTexture, bgPos2, Color.White);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            bgPos1.Y += speed;
            bgPos2.Y += speed;

            //Scrolling background
            if (bgPos1.Y >= endHeight) { bgPos1.Y = -renderTexture.Height+2; /*bgPos2.Y = -texture.Height;*/ }
            if (bgPos2.Y >= endHeight) { bgPos2.Y = -renderTexture.Height+2; /*bgPos2.Y = -texture.Height;*/ }

        }
    }
}

