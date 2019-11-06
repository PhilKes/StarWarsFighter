using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class Destroyer:Cruiser
    {

        public Destroyer(StarWarsFighter game, Vector2 pos, ContentManager cont) : base(game,pos,cont)
        {
            position = pos;
            health = 100;
            value = 120;
            DrawOrder = 1;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);  
        }
        public override void Initialize()
        {
            /*canonOffsets.Add(new Vector2(-111, -137));
            canonOffsets.Add(new Vector2(-38, 80));
            canonOffsets.Add(new Vector2(38, 80));
            canonOffsets.Add(new Vector2(111, -137));*/
            base.Initialize();
            //ship.isColliding[0] = false;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void LoadContent()
        {
            base.LoadContent();
        }
        public override void detectCollision(List<CollidingObject> colliding)
        {
            base.detectCollision(colliding);
        }
    }
}
