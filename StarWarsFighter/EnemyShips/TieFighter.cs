using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class TieFighter:Minion
    {
        public TieFighter(StarWarsFighter game, Vector2 pos, ContentManager cont) : base(game,pos,cont)
        {
            speed = 5;
            health = 40;
            value = 10;
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            //movement(gameTime);
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
        public override void movement(GameTime gameTime)
        {
            base.movement(gameTime);
        }
    }
}
