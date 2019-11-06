using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class Score : DrawableGO
    {
        public StarWarsFighter game{get;set;}
        public int score{get;set;}
        //private Vector2 texPosition{get;set;}
        public static SpriteFont scoreFont{get;set;}
        //public SpriteBatch spriteBatch{get;set;}
        public Score(StarWarsFighter game) : base(game)
        {
            this.game = game;
            texPosition = Vector2.Zero;
            score = 0;
            DrawOrder = 10;
        }
        public override void Initialize()
        {
            base.Initialize();
            //spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (StarWarsFighter.inLAN) 
                spriteBatch.DrawString(scoreFont, myGame.scorePl1 + " : " + myGame.scorePl2, new Vector2(StarWarsFighter.windowWidth / 2 - scoreFont.MeasureString(myGame.scorePl1 + " : " + myGame.scorePl2).Length()/2, StarWarsFighter.windowHeight / 2 - scoreFont.MeasureString("I").Y/2), Color.White);
            else
                spriteBatch.DrawString(scoreFont, "Score:" + score.ToString(), texPosition, Color.White);
            spriteBatch.End();
        }

        public void addScore(int add)
        {
            score += add;
        }
        protected override void LoadContent()
        {
            scoreFont = game.Content.Load<SpriteFont>("ScoreFont");
        }
    }
}
