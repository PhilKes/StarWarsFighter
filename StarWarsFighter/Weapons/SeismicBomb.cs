using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class SeismicBomb :Weapon
    {
        public SeismicBomb(StarWarsFighter game, Character chara, Vector2 pos) : base(game, chara, pos)
        {
            damage = 120;
            ammo = 5;
            rateOfFire = 2f;
            speed = 0;
            bulletColor = Color.White;
        }
        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            if (character == myGame.player)
                drawAmmo();
            bullet.ForEach(b =>
            {
                b.Draw(gameTime);
            });
        }
        public override void Initialize()
        {
            LoadContent();
            myGame.gameAudio.AddSound(GetType().Name + "_countdown");
            myGame.gameAudio.AddSound(GetType().Name + "_explosion");
            //bulletOffset.Add(new Vector2(0,-60));

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            bullet.ForEach(b =>
            {
                b.move = new Vector2(0, 6);
                b.Update(gameTime);
            });
        }
        public override void detectCollision(List<CollidingObject> colliders)
        {
            bullet.ForEach(b =>
            {
                b.detectCollision(colliders);
            });
        }
        public void detectExplosion(Rectangle boomBox, Vector2 pos)
        {
            Rectangle fullboomBox = new Rectangle(boomBox.X + boomBox.Width, boomBox.Y + boomBox.Height, 3 * boomBox.Width, 3 * boomBox.Height);

            Bullet collide = new Bullet(myGame, character, this, pos, new Vector2(0, 0));
            collide.collisionBox=new Rectangle();
            collide.isColliding=false;
            collide.collisionBox=fullboomBox;
            collide.isColliding=true;
            drawWireFraming(fullboomBox);
            myGame.detectCollisions(collide);
        }
        public override void fireBullet(bool isPlayer)
        {
            //base.fireBullet(pos);
            if (!StarWarsFighter.debug) ammo--;
            bulletOffset.ForEach(bO =>
            {
                bullet.Add(new Bomb((StarWarsFighter)Game, character, this, character.ship.position + bO, collisionBoxOffset));
                bullet[bullet.Count - 1].Initialize();
                bullet[bullet.Count - 1].renderColor = bulletColor;
                bullet[bullet.Count - 1].move = new Vector2(0, 0);
            });

            myGame.gameAudio.PlaySound(GetType().Name + "_blast");
        }
        public override void removeBullet(Bullet laserBullet)
        {
            base.removeBullet(laserBullet);
        }
        public void drawWireFraming(Rectangle fullboomBox)
        {
            spriteBatch = new SpriteBatch(myGame.GraphicsDevice);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            RasterizerState state = new RasterizerState();
            state.FillMode = FillMode.WireFrame;
            spriteBatch.GraphicsDevice.RasterizerState = state;

            spriteBatch.Draw(character.ship.renderTexture, fullboomBox, Color.White);
            spriteBatch.End();
        }
    }
}
