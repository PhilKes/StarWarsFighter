using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace StarWarsFighter
{
    public abstract class Weapon: CollidingObject
    {
        public int damage{get;set;}
        public int speed{get;set;}
        public float rateOfFire{get;set;}
        public List<Bullet> bullet{get;set;}
        public List<Vector2> bulletOffset{get;set;}
        public ContentManager contents{get;set;}
        public Color bulletColor{get;set;}
        public int ammo{get;set;}
        public string blastSound { get; set; }
        public Weapon(StarWarsFighter game,Character pChar,Vector2 pos) : base(game,pChar,pos)
        {
            bullet = new List<Bullet>();
            bulletOffset = new List<Vector2>();
            bulletColor = Color.White;
            ammo = 0;
            damage = 0;
            rateOfFire = 0f;
            speed = 0;
            contents = new ContentManager(game.Services, game.Content.RootDirectory);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (character == myGame.player)
                drawAmmo();
            bullet.ForEach(b =>
            {
                b.Draw(gameTime);
            });
            /*if (character is Player)
                drawAmmo();
            bullet.ForEach(b =>
            {
                b.Draw(gameTime);
            });*/

        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            bullet.ForEach(b =>
            {
                b.Update(gameTime);
            });
        }
        protected override void LoadContent()
        {
            //base.LoadContent();
            try
            {
                blastSound = GetType().Name + "blast_" + character.ship.type;
                if (!(character is Cruiser))
                {
                    myGame.gameAudio.AddSound(GetType().Name + "blast_" + character.ship.type);
                    //myGame.gameAudio.AddSound(GetType().Name + "_blast");
                }
                else if (character is Cruiser)
                {
                   
                    myGame.gameAudio.AddSound(GetType().Name + "_blast");
                    blastSound = GetType().Name + "_blast";
                }
            }
            catch (Exception e)
            {
                myGame.gameAudio.AddSound(GetType().Name + "_blast");
                
                blastSound = GetType().Name + "_blast";
            }
            spriteBatch =new SpriteBatch(myGame.GraphicsDevice);
            if(character is Player)
                texture.Add(myGame.Content.Load<Texture2D>("Misc/GUI/Weapons/"+GetType().Name));
        }
        public override void detectCollision(List<CollidingObject> colliders)
        {
            base.detectCollision(colliders);
            bullet.ForEach(b=> 
            {
                b.detectCollision(colliders);
            });
        }
        public virtual void fireBullet(bool isPlayer)
        {
            if(!StarWarsFighter.debug)ammo--;
            bulletOffset.ForEach(bO =>
            {
                Vector2 blob;
                if (isPlayer) blob = character.ship.position + bO;
                else blob = character.ship.position - bO;
                    bullet.Add(new Bullet((StarWarsFighter)Game, character, this,blob , collisionBoxOffset));
                bullet[bullet.Count - 1].Initialize();
                bullet[bullet.Count - 1].renderColor = bulletColor;
                if(isPlayer)bullet[bullet.Count - 1].move = new Vector2(0, speed);
                else bullet[bullet.Count - 1].move = new Vector2(0, -speed);
            });

            myGame.gameAudio.PlaySound(blastSound);
        }
        
        public void drawAmmo()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture[0],new Vector2(0,StarWarsFighter.windowHeight-(character.weapon.FindIndex(w=>w==this)+1)* GUI.font.MeasureString("o").Y),bulletColor);
            spriteBatch.DrawString(GUI.font,ammo.ToString(),new Vector2(35, StarWarsFighter.windowHeight - (character.weapon.FindIndex(w => w == this)+1) * GUI.font.MeasureString("o").Y),Color.White);
            spriteBatch.End();
        }
        public virtual void removeBullet(Bullet laserBullet)
        {
            laserBullet.Dispose();
            //laserBullet.content.Unload();
            bullet.Remove(laserBullet);
        }
    }
}
