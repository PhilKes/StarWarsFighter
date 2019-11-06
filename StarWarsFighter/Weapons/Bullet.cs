using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace StarWarsFighter
{
    public class Bullet: CollidingObject
    {
        public Weapon myWeapon{get;set;}
        public Vector2 spawnPosition{get;set;}
   
        public Vector2 move{get;set;}

        public Bullet(StarWarsFighter game,Character pChar,Weapon weapon,Vector2 pos,Vector2 collisionOffset) : base(game,pChar,pos)
        {
            myWeapon = weapon;
            collisionBoxOffset = collisionOffset;
            spawnPosition = pos;
            DrawOrder = 2;

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        public override void Initialize()
        {
            base.Initialize();
            renderColor = Color.Red;
            collisionBox=new Rectangle((int)(texPosition.X- collisionBoxOffset.X/2), (int)(texPosition.Y- collisionBoxOffset.Y/2), (int)(renderTexture.Width+ collisionBoxOffset.X), (int)(renderTexture.Height+ collisionBoxOffset.Y));
            isColliding=true;
            
            move = new Vector2(0, -myWeapon.speed);

        }
        public override void Update(GameTime gameTime)
        {
            translate(move);
            base.Update(gameTime);
            
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            texture.Add(myGame.Content.Load<Texture2D>("Weapons/" + myWeapon.GetType().Name + "/bullet"));
            renderTexture = texture[0];
            texPosition = position - new Vector2(renderTexture.Width / 2, renderTexture.Height / 2);
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();
        } 
        public override void handleCollision(CollidingObject collider)
        {
            //base.handleCollision(collider);
            if (collider == null) return;
            if (!(collider.character is Enemy && character is Enemy))
            {
                if (collider is Ship)
                {
                    Character temp = (Character)collider.character;
                    if (temp.hasShield)
                    {
                        handleCollision(temp.colliders.Find(b => b is Shield));
                        
                    }
                    else
                    { 
                        if (this.myWeapon is RocketCanon)
                        {
                            RocketCanon temp2 = (RocketCanon)myWeapon;
                            temp2.detectExplosion(collisionBox, position);
                        }

                        temp.applyDamage(myWeapon.damage);
                        if (!(this is Bomb))
                            myWeapon.removeBullet(this);
                    }
                }
                else if (collider is CruiserCanon)
                {
                    CruiserCanon temp = (CruiserCanon)collider;
                    //this.collisionBox.Clear();
                    if (!(this is Bomb))
                        myWeapon.removeBullet(this);
                    temp.applyDamage(myWeapon.damage);
                }
            }
            if (collider is Shield)
            {
                Shield s = (Shield)collider;
                s.gotHit(myWeapon.damage);
                myWeapon.removeBullet(this);
            }
            
        }

    }
}
