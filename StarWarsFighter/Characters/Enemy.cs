using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
namespace StarWarsFighter
{
    public abstract class Enemy: Character
    {
        public static string side{get;set;}
        public int value{get;set;}
        public Enemy(StarWarsFighter game,Vector2 pos, ContentManager cont) : base(game,pos,cont)
        {
            value = 0;
            
        }
        public override void Draw(GameTime gameTime)
        {
           base.Draw(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
            movement(gameTime);

            if (ship.move.X < 0) ship.rotation = -0.05f + (float)Math.PI;
            else if (ship.move.X > 0)ship.rotation = 0.05f + (float)Math.PI;
            else ship.rotation = (float)Math.PI;

            base.Update(gameTime);
            
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            myGame.gameAudio.AddSound(ship.type + "_flyby");   
        }

        public override void remove()
        {
            //manager.Dispose();
            myGame.gameAudio.RemoveSound(ship.type + "_flyby");
            myGame.removeCharacter(this);
        }
        public override bool isDead()
        {
            if (base.isDead())
            {
                myGame.score.addScore(value);
                return true;
            }
            return false;
        }
        public virtual void movement(GameTime gameTime)
        {
            if (ship.died)
            {
                ship.move.Y = ship.speed;
            }
            if (ship.position.Y + ship.renderTexture.Height / 2 >= StarWarsFighter.windowHeight)
            {
                myGame.enemySpawn.Add(GetType().Name.First());
                myGame.enemySpawntime.Add(myGame.inGameTime + 3f);
                myGame.removeCharacter(this);
            }
        }
        public override void handleCollision(CollidingObject otherCol)
        {
            //base.handleCollision(otherChar);
            if(otherCol is Ship)                                      
            {
                if (otherCol.character is Enemy)                                            //Enemy<->Enemy
                {
                    if (otherCol.character.ship.position == ship.position)
                    {
                        if (ship.texPosition == new Vector2(0, 0))
                        {
                            ship.lastSafePos = position + new Vector2(ship.renderTexture.Width + ship.renderTexture.Width / 2, 0);
                        }
                        else if (ship.texPosition == new Vector2(StarWarsFighter.windowWidth - ship.renderTexture.Width, 0))
                        {
                            ship.lastSafePos = position - new Vector2(ship.renderTexture.Width + ship.renderTexture.Width / 2, 0);
                        }
                        else
                        {
                            ship.lastSafePos = position + new Vector2(ship.renderTexture.Width + ship.renderTexture.Width / 2, 0);
                        }
                        ship.moveBack(ship.lastSafePos);
                    }
                    else
                    {
                        dontCollide(otherCol);
                        //ship.moveBack(ship.lastSafePos);
                    }
                }
                else if (otherCol.character is Player)
                {
                    Player temp = (Player)otherCol.character;
                    temp.applyDamage(10);
                    die();
                }
                else
                {
                    Console.WriteLine("Wie hast du die Scheiße hingekriegt?");
                }
            }
            /*if (otherCol is Bullet)
            {
                if (otherCol.character is Enemy) { }
                else
                {
                    Bullet temp = (Bullet)otherCol;
                    applyDamage(temp.myWeapon.damage);
                }
            }*/
            isDead();  
        }
        public override void Initialize()
        {
            base.Initialize();
            ship.rotation = (float)Math.PI;
            DrawOrder = 2;
            LoadValues();
            if (!myGame.gameAudio.isSoundPlaying(ship.type + "_fly"))
            {
                myGame.gameAudio.PlaySound(ship.type + "_fly");
            }

                myGame.gameAudio.PlaySound(ship.type + "_flyby");  
        }

        private void LoadValues()
        {
        try
            {
                System.IO.Stream stream = TitleContainer.OpenStream("Content/" + Ship.shipDir + ship.type + "/" + ship.type+".txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                int line = 0;
                string s = "";

                while (!((s = sreader.ReadLine()) == "end"))
                {
                    //Console.WriteLine("Line " + line + ": " + s);
                    line++;
                    if (s == "stats")
                    {
                        health = Int16.Parse(sreader.ReadLine());
                        ship.speed = Int16.Parse(sreader.ReadLine());
                    }
                    if (s == "cruisercannon")
                    {
                        //FEHLT------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        Cruiser cruiser = (Cruiser)this;

                        var temp = sreader.ReadLine().Split(',');
                        Color bulletColor = new Color(Int16.Parse(temp[0]), Int16.Parse(temp[1]), Int16.Parse(temp[2]));

                        int dmg = Int16.Parse(sreader.ReadLine());
                        float rof = float.Parse(sreader.ReadLine());
                        if ((s = sreader.ReadLine()) == "offsets")                  //Interpretiere nächsten 2 Zeilen als X,Y für ein Offset
                        {
                            List<Vector2> offsets = new List<Vector2>();
                            string value = "";
                            while (!((value = sreader.ReadLine()) == "endoffsets")) //solange neuen Offset einlesen bis "endoffsets" in zeile steht
                            {
                                int x = Int16.Parse(value);
                                value = sreader.ReadLine();
                                int y = Int16.Parse(value);
                                offsets.Add(new Vector2(x, y));
                            }

                            cruiser.canonBulletColor = bulletColor;
                            offsets.ForEach(o =>
                            {
                                //Console.WriteLine("Off:(" + o.X + "|" + o.Y + ")");
                                cruiser.canonOffsets.Add(o);

                            });
                            cruiser.canonOffsets.ForEach(cO =>
                            {
                                CruiserCanon cC = new CruiserCanon(myGame, this, ship.position + cO, cruiser.canon.Count, cruiser.canonBulletColor);
                                cC.damage = dmg;
                                cC.rateOfFire = rof;
                                cruiser.canon.Add(cC);
                            });


                        }
                    }
                    if (s == "lasercannon")                                         //Interpretiere nächsten 2 Zeilen als X,Y für ein Offset
                    {
                        LaserCanon lc = new LaserCanon(myGame, this, position);
                        var temp = sreader.ReadLine().Split(',');
                        lc.bulletColor = new Color(Int16.Parse(temp[0]), Int16.Parse(temp[1]), Int16.Parse(temp[2]));
                        lc.damage = Int16.Parse(sreader.ReadLine());
                        lc.rateOfFire = float.Parse(sreader.ReadLine());
                        lc.ammo = Int16.Parse(sreader.ReadLine());
                        weapon.Add(lc);

                        List<Vector2> offsets = new List<Vector2>();
                        string value = "";
                        if ((value = sreader.ReadLine()) == "offsets")
                        {
                            while (!((value = sreader.ReadLine()) == "endoffsets")) //solange neuen Offset einlesen bis "endoffsets" in zeile steht
                            {
                                int x = Int16.Parse(value);
                                value = sreader.ReadLine();
                                int y = Int16.Parse(value);
                                offsets.Add(new Vector2(x, y));
                            }
                        }
                        Weapon wp = weapon.Find(w => w is LaserCanon);
                        offsets.ForEach(o =>
                        {
                            wp.bulletOffset.Add(o);
                        });
                            
                    }

                    if (s == "rocketcannon")
                    {
                        RocketCanon lc = new RocketCanon(myGame, this, position);
                        //var temp = sreader.ReadLine().Split(',');
                        //lc.bulletColor = new Color(Int16.Parse(temp[0]), Int16.Parse(temp[1]), Int16.Parse(temp[2]));
                        lc.damage = Int16.Parse(sreader.ReadLine());
                        lc.rateOfFire = float.Parse(sreader.ReadLine());
                        lc.ammo = Int16.Parse(sreader.ReadLine());
                        weapon.Add(lc);

                        List<Vector2> offsets = new List<Vector2>();
                        string value = "";
                        if ((value = sreader.ReadLine()) == "offsets")
                        {
                            while (!((value = sreader.ReadLine()) == "endoffsets")) //solange neuen Offset einlesen bis "endoffsets" in zeile steht
                            {
                                int x = Int16.Parse(value);
                                value = sreader.ReadLine();
                                int y = Int16.Parse(value);
                                offsets.Add(new Vector2(x, y));
                            }
                        }
                        Weapon wp = weapon.Find(w => w is RocketCanon);
                        offsets.ForEach(o =>
                        {
                            wp.bulletOffset.Add(o);
                        });
                    }

                }

            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Couldnt load file:Content/"+Ship.shipDir+ship.type+"/"+ship.type+".txt");
            }    
        }


        private void dontCollide(CollidingObject otherCol)
        {
            Ship otherShip = (Ship)otherCol;
            //X------------------------------------
            if (otherShip.position.X<ship.position.X)                  //Anderes Ship weiter links als eigenes
            {
                ship.translate(new Vector2(2*ship.speed, 0));
            }
            else if(otherShip.position.X > ship.position.X)                  //Anderes Ship weiter rechts als eigenes
            {
                ship.translate(new Vector2(-2 * ship.speed, 0));
            }
            else if (otherShip.position.X == ship.position.X)                  //Anderes Ship in  eigenem
            {
                ship.translate(new Vector2((int)-ship.renderTexture.Width/2- 0, 0));
                otherShip.translate(new Vector2((int)otherShip.renderTexture.Width/2+ 0, 0));
                //otherShip.translate(new Vector2(ship.speed, 0));
            }
            //Y-----------------------------------
            if (otherShip.position.Y < ship.position.Y)                  //Anderes Ship weiter oben als eigenes
            {
                ship.translate(new Vector2(0, 2 * ship.speed));
            }
            else if (otherShip.position.Y > ship.position.Y)                  //Anderes Ship weiter unten als eigenes
            {
                ship.translate(new Vector2(0, -2 * ship.speed));
            }
            else if (otherShip.position.Y == ship.position.Y)                  //Anderes Ship in  eigenem
            {
                ship.translate(new Vector2(0, -2 * ship.speed));
                ship.translate(new Vector2(0, 2 * ship.speed));
            }
        }
    }
}
