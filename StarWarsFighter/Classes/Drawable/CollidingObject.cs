using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public abstract class CollidingObject : DrawableGO
    {
        public Rectangle collisionBox{get;set;}
        public Vector2 collisionBoxOffset{get;set;}
        public Boolean isColliding{get;set;}
        public Character character{get;set;}
        public bool died{get;set;}
        public int maxFrame{get;set;}
        public int frameCount{get;set;}
        public int frameDur{get;set;}
        public float frameDurCount{get;set;}

        public CollidingObject(StarWarsFighter game,Character pChar,Vector2 pos) : base(game)
        {
            collisionBoxOffset = Vector2.Zero;
            position = pos;
            died = false;
            character = pChar;
            frameDurCount = 0;
            frameCount = 0;
            frameDur = 75;
            maxFrame = Constants.DEAD_FRAMECOUNT;
            collisionBox = new Rectangle();
            isColliding = new bool();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (died)
            {
                frameDurCount += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameDurCount>=frameDur)
                {
                   
                    frameCount++;
                    frameDurCount = 0;
                    if (frameCount >= maxFrame)
                    {
                        if (this is Ship)
                        {
                            character.remove();
                        }
                        else if(character is Cruiser)
                        {
                            Cruiser temp = (Cruiser)character;
                            temp.canonDied((CruiserCanon)this);
                        }
                    }
                }
            }
        }
        public void translate(Vector2 moveIt)
        {
            position += moveIt;
            checkOutOfBox();
        }
        public override void Draw(GameTime gameTime)
        {
            if (!died)
            {
                base.Draw(gameTime);
            }
            else
            {
                spriteBatch.Begin();
                //spriteBatch.Draw(renderTexture, texPosition, new Rectangle(128 * frameCount, 0, 128, 128), renderColor);
                //spriteBatch.Draw(renderTexture, texPosition-new Vector2((scale-1)*renderTexture.Width/ (2*maxFrame), (scale - 1) * renderTexture.Height/(2*maxFrame)),
                spriteBatch.Draw(renderTexture, texPosition-new Vector2((float)(scale-1)*renderTexture.Width/(maxFrame*2), (float)(scale-1)*renderTexture.Height/2),
                new Rectangle((renderTexture.Width/ maxFrame) * frameCount, 0,renderTexture.Width / maxFrame, renderTexture.Width / maxFrame), renderColor, 0f,
                new Vector2(0,0),
                (float)scale, SpriteEffects.None, 0f);
                spriteBatch.End();
            }
            if (StarWarsFighter.debug)
            {
                drawWireFrame();
            }

        }

        public void drawWireFrame()
        {
            //spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            RasterizerState state = new RasterizerState();
            state.FillMode = FillMode.WireFrame;
            spriteBatch.GraphicsDevice.RasterizerState = state;
            Texture2D t;
            t = new Texture2D(Game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.Red });

                //if (isColliding[i])
            spriteBatch.Draw(renderTexture, collisionBox, Color.White);
            
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.Draw(t, new Rectangle((int)position.X - 2, (int)position.Y - 2, 4, 4), Color.Red);
            spriteBatch.End();
            
        }


        public virtual void detectCollision(List<CollidingObject> colliders)
        {
            colliders.ForEach(c =>
            {

                if (isColliding && c.isColliding)
                {
                    if (collisionBox.Intersects(c.collisionBox))
                    {
                        handleCollision(c);
                        //colliders.Remove(c);
                    }
                }
 
            });
        }
        
        public virtual void handleCollision(CollidingObject collider){}
        public virtual void checkOutOfBox()
        {
            bool changed = false;
            int renderFactor = 2;
            if (died) renderFactor = 20;
            //keep Player in screenbound
            //Linke Seite
            if ((position.X - renderTexture.Width/ renderFactor) < 0) { position.X = renderTexture.Width/ renderFactor; changed = true; }
            //Rechte Seite
            if ((position.X + renderTexture.Width/ renderFactor) > StarWarsFighter.windowWidth)
            { position.X = StarWarsFighter.windowWidth - renderTexture.Width/ renderFactor; changed = true; }
            //Oben
            if ((position.Y - renderTexture.Height / 2) < 0)
            { position.Y = renderTexture.Height / 2; changed = true; }
            //Unten
            if ((position.Y + renderTexture.Height / 2) > StarWarsFighter.windowHeight) { position.Y = StarWarsFighter.windowHeight - renderTexture.Height / 2; changed = true; }

            if (this is Bullet)
            {
                if (!(this is Bomb))
                {
                    if (changed)
                    {
                        Bullet temp = (Bullet)this;
                        temp.myWeapon.removeBullet(temp);
                        this.Dispose();
                    }
                }
            }
             texPosition = position - new Vector2(renderTexture.Width / renderFactor, renderTexture.Height / 2); 


            collisionBox = new Rectangle((int)(texPosition.X-collisionBoxOffset.X/2), (int)(texPosition.Y-collisionBoxOffset.Y/2), (int)(renderTexture.Width/(0.5*renderFactor)+ collisionBoxOffset.X), (int)(renderTexture.Height+ collisionBoxOffset.Y));
            
            
        }
    }
}
