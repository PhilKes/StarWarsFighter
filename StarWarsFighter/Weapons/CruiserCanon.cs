using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace StarWarsFighter
{
    public class CruiserCanon : Weapon
    {
        public Cruiser myCruiser{get;set;}
        public int health{get;set;}
        public float timeElapsedTotal{get;set;}
        private float randomDelay{get;set;}
        public static float randomDelayMax { get; set; }
        public int randomDelaySeed{get;set;}
        public bool isDestroyed{get;set;}

        public CruiserCanon(StarWarsFighter game,Character pChar,Vector2 pos,int count,Color bulletCl) :base(game,pChar,pos)
        {
            isDestroyed = false;
            collisionBoxOffset = new Vector2(12, 12);
            myCruiser = (Cruiser)pChar;
            damage = 10;
            health = 50;
            rotation = 0f;
            randomDelaySeed = count * 23;
            randomDelayMax = 2.5f;
            randomDelay = (float)count;
            timeElapsedTotal = 0f;
            //rateOfFire =4f;
            renderColor = Color.White;
            position = pos;
            speed =10;
            bulletColor = bulletCl;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            bullet.ForEach(b => b.Draw(gameTime));
        }
        public override void Update(GameTime gameTime)
        {
            timeElapsedTotal += (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
            if (!isDestroyed)
            {
                if (timeElapsedTotal > (rateOfFire + randomDelay))
                {
                    fireBullet(false);
                    randomDelaySeed *= (int)randomDelay;
                    Random random = new Random((int)(randomDelaySeed * randomDelay));
                    randomDelay = (float)(random.NextDouble() * randomDelayMax);
                    timeElapsedTotal -= timeElapsedTotal;
                }
            }
            bullet.ForEach(b => b.Update(gameTime));
        }

        public override void Initialize()
        {
            base.Initialize();
            bulletOffset.Add(new Vector2(-6,40));
            bulletOffset.Add(new Vector2(5, 40));
        }

        public override void fireBullet(bool isPlayer)
        {
            if (!StarWarsFighter.debug) ammo--;
            bulletOffset.ForEach(bO =>
            { 
                bullet.Add(new Bullet((StarWarsFighter)Game, character, this, position + bO, collisionBoxOffset));
                bullet[bullet.Count - 1].Initialize();
                bullet[bullet.Count - 1].renderColor = bulletColor;
                bullet[bullet.Count - 1].move = new Vector2(0, speed);
            });

            myGame.gameAudio.PlaySound(blastSound);
            
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            myGame.gameAudio.AddSound(GetType().Name + "_hit");
            myGame.gameAudio.AddSound(GetType().Name + "_explode");
            myGame.gameAudio.AddSound(GetType().Name + "_blast");
            if (Enemy.side == "Empire")
            {
                texture.Add(character.manager.Load<Texture2D>("Weapons/" + GetType().Name + "/full"));    
            }
            else
            {
                texture.Add(character.manager.Load<Texture2D>("Weapons/" + GetType().Name + "/full_rebel"));     
            }
            texture.Add(character.manager.Load<Texture2D>("Ships/explode"));     
            texture.Add(character.manager.Load<Texture2D>("Weapons/"+GetType().Name+"/dead"));     
            texture.Add(character.manager.Load<Texture2D>("Ships/shield"));

            renderTexture = texture[Constants.TEX_FULL];
            texPosition = position - new Vector2(renderTexture.Width / 2, renderTexture.Height / 2);
            collisionBox=new Rectangle((int)(texPosition.X - collisionBoxOffset.X / 2),
                            (int)(texPosition.Y - collisionBoxOffset.Y / 2), (int)(renderTexture.Width /2 + collisionBoxOffset.X / 2),
                            (int)(renderTexture.Height + collisionBoxOffset.Y / 2));
            isColliding=true;
            checkOutOfBox();
        }

        public void applyDamage(int damage)
        {
            health -= damage;
            gotHit();
            isDead();
        }
        public void hitColorChange()
        {
            Thread.Sleep(100);
            renderColor = Color.White;
        }
        public void gotHit()
        {
            myGame.gameAudio.PlaySound(GetType().Name + "_hit");
            renderColor = Color.Red;
            Thread thread = new Thread(
             new ThreadStart(hitColorChange));
            thread.Start();

        }
        public virtual bool isDead()
        {
            
            if (health <= 0)
            {
                myGame.gameAudio.PlaySound(GetType().Name + "_explode");
                  Console.WriteLine(GetType().Name + " died");
                bulletOffset.Clear();
                collisionBox=new Rectangle();
                isColliding = false;
                myCruiser.removeCanon(this);
                died = true;
                float scY = (float)texture[Constants.TEX_FULL].Height / (float)texture[Constants.TEX_EXPLODE].Height;
                float scX = (float)texture[Constants.TEX_FULL].Width / (float)texture[Constants.TEX_EXPLODE].Width / Constants.DEAD_FRAMECOUNT;
                scale = (float)((scY + scX+0.15f) );
                if (scale <= 0) scale = 1f; 
                return true;
            }
            else return false;
        }
    }
}
