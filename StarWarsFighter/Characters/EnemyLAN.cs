using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class EnemyLAN : Player
    {
        public EnemyLAN(StarWarsFighter game, Vector2 pos,string shipType,ContentManager cont) : base(game,pos,shipType,cont)
        {

        }
        public override void Update(GameTime gameTime)
        {
            ship.move = enemyMove;
            for (int i = 0; i < totalelapsedtime.Count; i++)
            {
                totalelapsedtime[i] += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            base.baseUpdate(gameTime);
            ship.rotation = (float)Math.PI;
            enemyMove = Vector2.Zero;
            //base.Update(gameTime);
        }
        public override void Initialize()
        {
            base.Initialize();
            ship.rotation = (float)Math.PI;
        }
        public override void die()
        {
            base.die();
            if (StarWarsFighter.inLAN)
            {
                myGame.scorePl1++;
                myGame.sendDeath();
            }
        }
    }
}
