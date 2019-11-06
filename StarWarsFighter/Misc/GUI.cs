using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarWarsFighter
{
    public class GUI: DrawableGO
    {
        public static Vector2 offset= new Vector2(StarWarsFighter.windowWidth / 100, -15);
        public Vector2 ammoPos{get;set;}
        public static SpriteFont font{get;set;}
        public static SpriteFont btnFont { get; set; }
        public static int healthBar_width = 200;
        public static int healthBar_height = 15;
        public GUI(StarWarsFighter game) : base(game)
        {
            myGame = game;
           
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            texture.Add(Game.Content.Load<Texture2D>("Misc/GUI/Weapons/RocketCanon"));
            texture.Add(Game.Content.Load<Texture2D>("Misc/GUI/Weapons/LaserCanon"));
            font = Game.Content.Load<SpriteFont>("GUIFont");
            btnFont= Game.Content.Load<SpriteFont>("BtnFont");
            offset.Y = -font.MeasureString("0").Y;
            renderTexture = texture[0];
            ammoPos = new Vector2(0, StarWarsFighter.windowHeight-renderTexture.Height)+offset;
        }
        
        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            /*spriteBatch.Begin();
            myGame.player.weapon.ForEach(w => 
            {
                spriteBatch.DrawString(font, w.ammo.ToString(), ammoPos + new Vector2((int)(renderTexture.Width + offset.X), offset.Y*myGame.player.weapon.FindIndex(we=>w==we)), Color.White);
                //spriteBatch.Draw(texture[myGame.player.weapon.FindIndex(we=>we== w)], texPosition - new Vector2(-texture[myGame.player.weapon.FindIndex(we => we == w)].Width, (int)renderTexture.Height - offset.Y), Color.Red);
            });*/
            // spriteBatch.DrawString(font, myGame.player.weapon[0].ammo.ToString(), ammoPos + new Vector2((int)(renderTexture.Width + offset.X), 5*offset.Y), Color.White);
            //spriteBatch.End();
            if (myGame.player != null)
            {
                spriteBatch.Begin();
                Texture2D t;
                t = new Texture2D(Game.GraphicsDevice, 1, 1);
                t.SetData(new[] { Color.White });
                //Healthbar Red
                spriteBatch.Draw(t, new Rectangle(StarWarsFighter.windowWidth - healthBar_width-20 ,
                      StarWarsFighter.windowHeight - healthBar_height - 20, healthBar_width, healthBar_height), Color.Red);
                //HealthBar Green
                spriteBatch.Draw(t, new Rectangle(StarWarsFighter.windowWidth - healthBar_width-20 ,
                     StarWarsFighter.windowHeight - healthBar_height - 20, (int)Constants.ScaleRange(myGame.player.health, 0, myGame.player.maxHealthship, 0, healthBar_width), healthBar_height), Color.Green);
                //Health Numbers
                spriteBatch.DrawString(GUI.font, myGame.player.health + "/" + myGame.player.maxHealthship, new Vector2(StarWarsFighter.windowWidth - (healthBar_width/2) -20* 3,
                    StarWarsFighter.windowHeight - (healthBar_height / 2) - 20 - GUI.font.MeasureString("x").Y / 2), Color.White);
                spriteBatch.End();
            }
        }
        public override void Initialize()
        {
            base.Initialize();
            position = ammoPos;
            texPosition = ammoPos;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
