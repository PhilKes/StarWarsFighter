using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class Fighter : Enemy
    {

        public Fighter(StarWarsFighter game,Vector2 pos,ContentManager content) :base(game,pos,content)
        {
            value = 20;
        }
        public override void Initialize()
        {
            base.Initialize();
            weapon.ForEach(w =>
            {
                w.Initialize();
                //w.speed = -w.speed;
                totalelapsedtime.Add(0f);
            });
            weapon.ForEach(w =>
            {
                colliders.Add(w);
            });
        }
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < totalelapsedtime.Count; i++)
            {
                totalelapsedtime[i] += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            base.Update(gameTime);
            if (!ship.died)
            {
                for (int i = 0; i < weapon.Count; i++)
                {
                    if (totalelapsedtime[i] > weapon[i].rateOfFire)
                    {
                        if (weapon[i].ammo > 0)
                        {
                            weapon[i].fireBullet(false);
                            totalelapsedtime[i] -= totalelapsedtime[i];
                        }
                    }
                }
            }
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        public override void movement(GameTime gameTime)
        {
            ship.move.Y += ship.speed/2 ;
            base.movement(gameTime);
        }
    }
}
