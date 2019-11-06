using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace StarWarsFighter
{
    public class RocketCanon : Weapon
    {

        public RocketCanon(StarWarsFighter game, Character pChar, Vector2 pos) : base(game, pChar,pos)
        {
            damage = 75;
            ammo = 5;
            rateOfFire = 2f;
            speed = -6;
            bulletColor = Color.White;
            collisionBoxOffset = new Vector2(12, 12);

        }
        public override void Draw(GameTime gameTime)
        {
            // base.Draw(gameTime);
            if (character == myGame.player)
                drawAmmo();
            bullet.ForEach(b =>
            {
                b.Draw(gameTime);
            });

        }
        public override void Initialize()
        {
            LoadContent();
            //bulletOffset.Add(new Vector2(0,-60));

        }

        public override void detectCollision(List<CollidingObject> colliders)
        { 
            bullet.ForEach(b =>
            {
                b.detectCollision(colliders);
            });
        }
        public void detectExplosion(Rectangle boomBox,Vector2 pos)
        {
            Rectangle fullboomBox = new Rectangle(boomBox.X+boomBox.Width, boomBox.Y + boomBox.Height,3*boomBox.Width,3*boomBox.Height);
            
            Bullet collide=new Bullet(myGame,character,this,pos,new Vector2(0,0));
            collide.collisionBox=new Rectangle();
            collide.isColliding=false;
            collide.collisionBox=fullboomBox;
            collide.isColliding=true;
            //drawWireFraming(fullboomBox);
            myGame.detectCollisions(collide);
        }


    }
}

