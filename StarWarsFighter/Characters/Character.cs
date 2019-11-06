using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Xna.Framework.Content;

namespace StarWarsFighter
{
    public abstract class Character : DrawableGO
    {
        public List<CollidingObject> colliders{get;set;}
        public List<Weapon> weapon{get;set;}
        public Ship ship{get;set;}
        public int health{get;set;}
        public List<float> totalelapsedtime{get;set;}
        public bool hasShield { get; set; }

        public Character(StarWarsFighter game,Vector2 pos, string shipType,ContentManager cont) : base(game)
        {
            manager = cont;
            totalelapsedtime = new List<float>();
            colliders = new List<CollidingObject>();
            weapon = new List<Weapon>();
            hasShield = false;
            position = pos;
            health = 0;
            ship = new Ship(game, this, shipType, position);
        }

        public Character(StarWarsFighter game, Vector2 pos, ContentManager cont) : base(game)       //Konstruktor für Enemy
        {
            manager = cont;
            totalelapsedtime = new List<float>();
            colliders = new List<CollidingObject>();
            weapon = new List<Weapon>();
            position = pos;
            health = 0;
            string temp = "";
            switch (GetType().Name)
            {
                case "Minion":
                    if (Enemy.side == "Rebels")
                        temp = Constants.rebelsShip[0];
                    else temp = Constants.empireShip[0];
                    break;
                case "Fighter":
                    if (Enemy.side == "Rebels")
                        temp = Constants.rebelsShip[1];
                    else temp = Constants.empireShip[1];
                    break;
                case "Destroyer":
                    if (Enemy.side == "Rebels")
                        temp = "RebelsDestroyer";
                    else temp = "EmpireDestroyer";
                    break;
                default:
                    temp = Constants.empireShip[0];
                    break;
            }
            ship = new Ship(game, this, temp, position);
        }
        public virtual void applyDamage(int damage)
        {
            gotHit();
            health -= damage;
            isDead();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            weapon.ForEach(w => w.Update(gameTime));
            ship.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            weapon.ForEach(w => w.Draw(gameTime));
            ship.Draw(gameTime);
        }
        public override void Initialize()
        {
            ship.Initialize();
            colliders.Add(ship);
            base.Initialize();
        }
        public virtual void die()
        {
            myGame.gameAudio.StopSound(ship.type + "_fly");
            myGame.gameAudio.PlaySound(ship.type + "_explode");
            
            ship.died = true;
            ship.renderTexture = ship.texture[Constants.TEX_EXPLODE];
            float scY= (float)ship.texture[Constants.TEX_FULL].Height / (float)(ship.texture[Constants.TEX_EXPLODE].Height);
            float scX = (float)ship.texture[Constants.TEX_FULL].Width / (float)(ship.texture[Constants.TEX_EXPLODE].Width/Constants.DEAD_FRAMECOUNT);
            ship.scale = (float)((scY + scX) / 2);
            if (ship.scale <= 1) ship.scale = 1f;
            ship.checkOutOfBox();

            colliders.ForEach(c =>
            {
                c.isColliding=false;
            });
            //colliders[colliders.FindIndex(c => c is Shield)] = null;
            ship.isColliding = false;
        }

        public virtual void remove()
        {
            //manager.Dispose();
            myGame.removeCharacter(this);
        }

        public void gotHit()
        {
            myGame.gameAudio.PlaySound(ship.type + "_hit");
            ship.renderColor = Color.Red;
            Thread thread = new Thread(
             new ThreadStart(ship.hitColorChange));
            thread.Start();

        }

        public virtual bool isDead()
        {
            if (health <= 0)
            {
                die();
                health = 0;
                return true;
            } 
            else return false;
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            myGame.gameAudio.AddSound(ship.type + "_hit");
            myGame.gameAudio.AddSound(ship.type + "_fly");
            myGame.gameAudio.AddSound(ship.type + "_explode");
        }
        public virtual void detectCollision(List<CollidingObject> colliding)
        {
            /*
            if (this is Enemy)
            {

            }*/
            colliders.ForEach(myCol => 
            {
                myCol.detectCollision(colliding);
            });
        }

        public virtual void handleCollision(CollidingObject otherCol)
        {
            gotHit();
        }
    }
}
