using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StarWarsFighter
{
    public abstract class DrawableGO : DrawableGameComponent
    {
        public List<Texture2D> texture{get;set;}
        public Texture2D renderTexture{get;set;}
        public Vector2 position;
        public Vector2 texPosition{get;set;}
        public Color renderColor{get;set;}
        public SpriteBatch spriteBatch{get;set;}
        public static StarWarsFighter myGame{get;set;}
        public ContentManager manager{get;set;}
        public float rotation{get;set;}
        public float scale{get;set;}
        public DrawableGO(StarWarsFighter game) : base(game)
        {
           
            rotation = 0f;
            texture = new List<Texture2D>();
            position = Vector2.Zero;
            texPosition = Vector2.Zero;
            renderColor = Color.White;
            scale = 1f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Initialize()
        {
            spriteBatch = (SpriteBatch)myGame.Services.GetService(typeof(SpriteBatch));
            base.Initialize();
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin();
            //spriteBatch.Draw(renderTexture, texPosition, renderColor);
            spriteBatch.Draw(renderTexture, texPosition, null, renderColor, rotation,
                             new Vector2(renderTexture.Width/2,renderTexture.Height/2)*
                             (float)(rotation/(Math.PI/2)),
                             (float)scale, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();
            
        }
        public void unload()
        {
            UnloadContent();
        }
        protected override void LoadContent()
        {
            base.LoadContent();
        }
    }
}
