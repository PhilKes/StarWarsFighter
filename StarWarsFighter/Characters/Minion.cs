using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class Minion : Enemy
    {
        public int speed{get;set;}
        public Shield shield { get; set; }

        public Minion(StarWarsFighter game, Vector2 pos, ContentManager cont) : base(game,pos,cont)
        {
            speed = 6;
            health = 30;
            value = 10;
            //hasShield = true;
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            //colliders[colliders.FindIndex(c=>c is Shield)].Draw(gameTime);
        }
        public override void Initialize()
        {
            base.Initialize();
            //shield = new Shield(myGame, this, ship.position);
            //colliders.Add(shield);
            //DrawOrder = 3;
           
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
        public override void movement(GameTime gameTime)
        {
            base.movement(gameTime);
            if(myGame.player!=null)
                followPlayer();
           
            ship.move += new Vector2(0, speed/2);

        }
        public override void detectCollision(List<CollidingObject> colliding)
        {
            colliding.RemoveAll(c=>c.character is Cruiser);
            /*int temp = colliding.Count;
            for (int i = 0; i < temp; i++)
            {
                if (i>=colliding.Count)
                {
                    break;
                }
                else if (colliding[i].character is Cruiser)
                {
                    colliding.Remove(colliding[i]);
                }

            }*/

            base.detectCollision(colliding);
        }
        public virtual void followPlayer()
        {
            if ((myGame.player.ship.position.X + 40) < ship.position.X)
            {
                ship.move.X -= speed;
            }
            else if ((myGame.player.ship.position.X - 40) > ship.position.X)
            {
                ship.move.X += speed;
            }
        }
    }
}
