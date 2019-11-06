using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class LaserCanon : Weapon
    {
       
        public LaserCanon(StarWarsFighter game, Character pChar,Vector2 pos) : base(game, pChar,pos)
        {
            damage = 12;
            ammo = 100;
            rateOfFire = 0.25f;
            speed = -30;
            bulletColor = Color.Red;
        }
        public override void Draw(GameTime gameTime)
        {
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

        }

        public override void detectCollision(List<CollidingObject> colliders)
        {
            bullet.ForEach(b =>
            {
                b.detectCollision(colliders);
            });
        }

    }
}
