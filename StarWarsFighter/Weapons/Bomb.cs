using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class Bomb : Bullet
    {
        public float elapsedTime { get; set; }
        public float time_ticking { get; set; }
        public int frames_exploding { get; set; }
        public float scaleFactor { get; set; }
        public float frameTime { get; set; }
        public bool isExploding { get; set; }
        public int curFrame { get; set; }
        public Bomb(StarWarsFighter game,Character pChar, Weapon weapon,Vector2 pos, Vector2 collisionOffset) : base(game, pChar, weapon, pos, collisionOffset)
        {
            elapsedTime = 0f;
            isExploding = false;
            time_ticking = 3f;
            frames_exploding = 22;
            curFrame = 0;
            scaleFactor = 0.15f;
            frameTime = 0.15f;
        }
        public override void Initialize()
        {
            base.Initialize();
            elapsedTime = 0f;
            renderColor = Color.White;
            move = Vector2.Zero ;
        }
        public override void Update(GameTime gameTime)
        {
            if (elapsedTime == 0f) myGame.gameAudio.PlaySound(myWeapon.GetType().Name + "_countdown");
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedTime >= time_ticking && !isExploding)
            {
                myGame.gameAudio.StopSound(myWeapon.GetType().Name + "_countdown");
                myGame.gameAudio.PlaySound(myWeapon.GetType().Name + "_explosion");
                isExploding = true;
                texPosition = new Vector2(position.X - ((float)scale * renderTexture.Width / 2), position.Y - ((float)scale * renderTexture.Height / 2));
                //collisionBox.Clear();
                //isColliding.Clear();
                collisionBox=new Rectangle((int)(texPosition.X - collisionBoxOffset.X / 2), (int)(texPosition.Y - collisionBoxOffset.Y / 2), (int)(renderTexture.Width + collisionBoxOffset.X), (int)(renderTexture.Height + collisionBoxOffset.Y));
                isColliding=true;
                elapsedTime -= time_ticking;
            }
            else if (isExploding && elapsedTime >= frameTime && curFrame < frames_exploding)
            {
                //collisionBox.Remove(collisionBox[1]);
                elapsedTime -= frameTime;
                curFrame++;
                scale = curFrame * scaleFactor;
                texPosition = new Vector2(position.X - ((float)scale * renderTexture.Width / 2), position.Y - ((float)scale * renderTexture.Height / 2));
                collisionBox = new Rectangle((int)(texPosition.X - collisionBoxOffset.X / 2), (int)(texPosition.Y - collisionBoxOffset.Y / 2), (int)(renderTexture.Width * scale + collisionBoxOffset.X), (int)(renderTexture.Height * scale + collisionBoxOffset.Y));
                renderTexture = myGame.Content.Load<Texture2D>("Weapons/" + myWeapon.GetType().Name + "/explosion");
            }
            if (curFrame>=frames_exploding)
            {
               myWeapon.removeBullet(this);
            }

        }

        public override void detectCollision(List<CollidingObject> colliders)
        {
            if(isExploding)
                base.detectCollision(colliders);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if(StarWarsFighter.debug)drawWireFrame();
        }

    }
}
