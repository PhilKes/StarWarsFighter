using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class Shield : CollidingObject
    {
        public static int offset = 24;
        public int health { get; set; }
        public Shield(StarWarsFighter game,Character pChar,Vector2 pos) : base(game,pChar,pos)
        {
            health = 50;
            Initialize();
        }
        public override void Initialize()
        {
            base.Initialize();
            isColliding = true;
            scale = (float)(character.ship.renderTexture.Height+ offset) / (float)renderTexture.Height;
            
        }
        protected override void LoadContent()
        {
            //base.LoadContent();
            texture.Add(character.manager.Load<Texture2D>("Ships/shield"));
            renderTexture = texture[0];
        }
        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            if (isColliding)
            { 
                texPosition = character.ship.texPosition;
                collisionBox = new Rectangle((int)texPosition.X - offset / 2, (int)texPosition.Y - offset / 2, (int)(renderTexture.Height * scale), (int)(renderTexture.Width * scale));

                spriteBatch.Begin();
                spriteBatch.Draw(renderTexture, texPosition - new Vector2(offset / 2, offset / 2), null, renderColor, rotation,
                                    new Vector2(renderTexture.Width / 2, renderTexture.Height / 2) *
                                    (float)(rotation / (Math.PI / 2)), (float)scale, SpriteEffects.None, 0f);
                spriteBatch.End();
                if (StarWarsFighter.debug) drawWireFrame();
            }
        }
        public override void handleCollision(CollidingObject collider)
        {
            // base.handleCollision(collider);
            if (collider is Bullet)
            {
                Bullet b = (Bullet)collider;
                b.myWeapon.removeBullet(b);
            }
        }
        public void gotHit(int dmg)
        {
            health -= dmg;
            if (health <= 0)
              character.colliders.Remove(this);
        }
    }
}
