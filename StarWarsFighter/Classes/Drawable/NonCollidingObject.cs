using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StarWarsFighter
{
    public class NonCollidingObject : DrawableGO
    {
        public NonCollidingObject(StarWarsFighter game) : base(game)
        {

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
            base.Update(gameTime);
        }

    }
}
