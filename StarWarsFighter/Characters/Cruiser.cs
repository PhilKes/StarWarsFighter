using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public abstract class Cruiser: Enemy
    {
        public List<CruiserCanon> canon{get;set;}
        public List<CruiserCanon> deadCanon{get;set;}
        public List<Vector2> canonOffsets{get;set;}
        public Color canonBulletColor{get;set;}
        public Cruiser(StarWarsFighter game, Vector2 pos, ContentManager cont) : base(game,pos,cont)
        {
            canon = new List<CruiserCanon>();
            canonOffsets = new List<Vector2>();
            deadCanon = new List<CruiserCanon>();
            canonBulletColor = Color.Violet;
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            canon.ForEach(c =>
            {
                c.Draw(gameTime);
            });
            deadCanon.ForEach(c =>
            {
                c.Draw(gameTime);
            });

        }
        public override void Initialize()
        {
            base.Initialize();
           canon.ForEach(c =>
            {
                c.Initialize();
            });
            canon.ForEach(c =>
            {
                colliders.Add(c);
            });
            DrawOrder = 2;
            ship.isColliding = false;

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            canon.ForEach(c =>
            {
                c.Update(gameTime);
            });
            deadCanon.ForEach(dc =>
            {
                dc.Update(gameTime);
            });

        }
        protected override void LoadContent()
        {
            base.LoadContent();
        }
        public override void detectCollision(List<CollidingObject> colliding)
        {
            List<CollidingObject> playerremove = colliding.FindAll(c => c is Ship && c.character is Player);            //Speichere alle Ship-Collider vom Spieler
            colliding.RemoveAll(cO =>playerremove.Contains(cO));                                                        //alle ship-collider vom spieler entfernen(Cruiser colliden nicht mit Player-Ship)

            colliding.RemoveAll(c => c.character is Enemy);                                                           //Cruiser colliden nicht mit Fightern

            base.detectCollision(colliding);

            colliding.AddRange(playerremove);                                                                           //Player-ship wieder dazufügen, da Canon-Bullets damit colliden
            canon.ForEach(c => 
            {
                c.detectCollision(colliding);
            });
         
            /*colliders.ForEach(c => 
            {
                colliding.Add(c);
            });*/

        }
        public void removeCanon(CruiserCanon can)
        {

            can.died = true;
            can.renderTexture = can.texture[Constants.TEX_EXPLODE];
            can.checkOutOfBox();
            colliders.Remove(can);

        }

        public void canonDied(CruiserCanon can2Remove)
        {
            can2Remove.isDestroyed = true;
            deadCanon.Add(can2Remove);
            can2Remove.renderTexture = can2Remove.texture[Constants.TEX_DEAD];
            can2Remove.scale = 1f;
            can2Remove.rotation = 0f;
            can2Remove.texPosition = can2Remove.position - new Vector2(can2Remove.renderTexture.Width / 2, can2Remove.renderTexture.Height / 2);
            can2Remove.died = false;
            can2Remove.frameCount = 0;
            can2Remove.health = 100;
            canon.Remove(can2Remove);
            
            if (canon.Count < 1)
            {
                colliders[0].isColliding = true;
                deadCanon.Clear();
                die();
            }
        }
        public override void movement(GameTime gameTime)
        {
            //base.movement(gameTime);
        }
        public override void die()
        {
            health = 0;
            myGame.score.addScore(value);
            base.die();
            
        }
        public override bool isDead()
        {
            if (canon.Count <= 0)
            {
                myGame.gameAudio.PlaySound(ship.type + "_explode");
                if(StarWarsFighter.debug)Console.WriteLine(GetType().Name + " died");
                ship.died = true;
                ship.renderTexture = ship.texture[Constants.TEX_EXPLODE];
                ship.checkOutOfBox();
                return true;
            }
            else return false;
        }

    }
}
