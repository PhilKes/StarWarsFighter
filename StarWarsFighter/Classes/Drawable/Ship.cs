using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace StarWarsFighter
{
    public class Ship : CollidingObject
    {
        public static string shipDir { get; set; } 
        public string type{get;set;}
        public Vector2 move;
        public Vector2 lastSafePos{get;set;}
        public int speed{get;set;}

        public Ship(StarWarsFighter game,Character character,string type,Vector2 pos) : base(game,character,pos)
        {
            this.type = type;
            //position = pos;
            shipDir = "Ships/Enemy/"; 
            move = Vector2.Zero;
            speed = 5;
            lastSafePos = position;
        }
        public override void Initialize()
        {
            base.Initialize();
            collisionBox=new Rectangle((int)texPosition.X,(int)texPosition.Y,renderTexture.Width,renderTexture.Height);
            isColliding=true;
            checkOutOfBox();
            DrawOrder = 2;
        }
        public override void Update(GameTime gameTime)
        {  
            Vector2 oldPos = position;
            base.Update(gameTime);
            if (died)
            {
                move = new Vector2(0, speed/2);
            }
            position += (move);
            Vector2 diff = position - oldPos;
            updateBoxes(diff);
            checkOutOfBox();
            move = Vector2.Zero;

        } 
        private void updateBoxes(Vector2 diff)
        {
            texPosition += diff;
            collisionBox = new Rectangle((int)diff.X+collisionBox.X, (int)diff.Y +collisionBox.Y, renderTexture.Width, renderTexture.Height);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            texture.Add(character.manager.Load<Texture2D>(shipDir+ type+"/full"));
            texture.Add(character.manager.Load<Texture2D>("Ships/explode"));
            renderTexture = texture[Constants.TEX_FULL];
            texPosition = position - new Vector2(renderTexture.Width/2,renderTexture.Height/2);
           
        }
        public override void handleCollision(CollidingObject collider)
        {
            //base.handleCollision(collider);
            character.handleCollision(collider);
        }

        public void moveBack(Vector2 oldPosition)
        {
            //if (oldPosition == Vector2.Zero)
             //   oldPosition = position + new Vector2(0, 1);
            position = oldPosition;
            checkOutOfBox();
        }
        public void hitColorChange()
        {
            Thread.Sleep(100);
            renderColor = Color.White;
        }
    }
}
