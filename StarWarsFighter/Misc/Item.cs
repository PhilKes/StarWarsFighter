using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class Item : CollidingObject
    {
        public int ammo { get; set; }
        private bool isPicked = false;
        private float timesincePick = 0;
        private const float time = 5;
        public Item(StarWarsFighter game,Vector2 pos) : base(game,null,pos)
        {
            ammo = Settings.Default.AmmoFromBox;
            position = pos;
        }
        public override void detectCollision(List<CollidingObject> colliders)
        {
            base.detectCollision(colliders);
        }
        public override void handleCollision(CollidingObject collider)
        {
            if (collider.character is Player)
            {
                playerPickupUp((Player)collider.character);
                isPicked = true;
                isColliding = false;
                //myGame.removeItem(this);
            }
        }

        protected void playerPickupUp(Player player)
        {
            player.weapon.ForEach(w=> 
            {
                w.ammo += this.ammo;
            });
        }

        public override void Draw(GameTime gameTime)
        {
            if (!isPicked)
            {
                base.Draw(gameTime);
            }

        }
        public override void Initialize()
        {
            base.Initialize();
            DrawOrder = 3;
        }
        public override void Update(GameTime gameTime)
        {
            if (isPicked)
            {
                timesincePick +=(float) gameTime.ElapsedGameTime.TotalSeconds;
                if (timesincePick >= time)
                {
                    isPicked = false;
                    isColliding = true;
                    timesincePick = 0f;
                }
            }

            base.Update(gameTime);
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            texture.Add(myGame.Content.Load<Texture2D>("Misc/Items/AmmoBox"));
            renderTexture = texture[0];
            texPosition = position - new Vector2(renderTexture.Width / 2, renderTexture.Height / 2);
            collisionBox=new Rectangle((int)(texPosition.X+collisionBoxOffset.X/2), (int)(texPosition.Y + collisionBoxOffset.Y/2),(int)(renderTexture.Width+collisionBoxOffset.X),(int)(renderTexture.Height + collisionBoxOffset.Y));
            isColliding=true;
            myGame.gameAudio.AddSound("AmmoBox_pickup");
        }
    }
}
