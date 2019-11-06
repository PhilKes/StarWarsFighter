using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using Microsoft.Xna.Framework.Content;

namespace StarWarsFighter
{
    public class Player: Character
    {
        public int maxHealthship { get; set; }
        public static Vector2 enemyMove { get; set; }
        public string type { get; set; }
        public Player(StarWarsFighter game,Vector2 pos,string shipType,ContentManager cont) : base(game,pos,shipType,cont)
        {
            type = ship.type;
            Ship.shipDir = "Ships/Player/";
            health = 100;
            ship.speed =10;
           
        }
        public override void Initialize()
        {
            base.Initialize();
          
            ship.rotation = 0f;
            DrawOrder = 5;
            myGame.gameAudio.PlaySound(ship.type + "_fly");

            LoadValues();

            //weapon.Add(new SeismicBomb(myGame, this, ship.position - new Vector2(20, 20)));
            weapon.ForEach(w =>
            {
                w.Initialize();
                totalelapsedtime.Add(0f);
            });
            weapon.ForEach(w =>
            {
                colliders.Add(w);
            });
        }
        public override void Update(GameTime gameTime)
        {
            ship.rotation = 0f;
            for (int i = 0; i < totalelapsedtime.Count; i++)
            {
                totalelapsedtime[i] += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (!ship.died)
            {
                keyInput(gameTime);
            }

            base.Update(gameTime);

        }
        protected void baseUpdate(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void keyInput(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            
            if (keyState.IsKeyDown(Constants.moveKeys[0]) || (padState.ThumbSticks.Left.Y > 0))
            {
                float temp = padState.ThumbSticks.Left.Y;
                if (temp <= 0) temp = 1;
                ship.move.Y += -ship.speed * temp;
            }
            if (keyState.IsKeyDown(Constants.moveKeys[1]) || (padState.ThumbSticks.Left.Y < 0))
            {
                float temp = padState.ThumbSticks.Left.Y;
                if (temp <= 0) temp = 1;
                ship.move.Y += ship.speed * temp;
            }
            if (keyState.IsKeyDown(Constants.moveKeys[2]) || (padState.ThumbSticks.Left.X < 0))
            {
                float temp = padState.ThumbSticks.Left.X;
                if (temp <= 0) temp = 1;
                ship.move.X += -ship.speed * temp;
                ship.rotation = -0.05f;
            }
            if (keyState.IsKeyDown(Constants.moveKeys[3]) || (padState.ThumbSticks.Left.X > 0))
            {
                float temp = padState.ThumbSticks.Left.X;
                if (temp <= 0) temp = 1;
                ship.move.X += ship.speed * temp;
                ship.rotation = 0.05f;
            }
            if (StarWarsFighter.inLAN && ship.move!=Vector2.Zero)
                myGame.sendMovement(-ship.move);

            for (int i = 0; i < weapon.Count; i++)
            {
                if (keyState.IsKeyDown(Constants.weaponControlKey[i])|| padState.IsButtonDown(Constants.weaponControlBtn[i]))
                {
                    if (totalelapsedtime[i] > weapon[i].rateOfFire)
                    {
                        if (weapon[i].ammo > 0)
                        {
                            weapon[i].fireBullet(true);
                            totalelapsedtime[i] -= totalelapsedtime[i];
                            if (StarWarsFighter.inLAN) myGame.sendShot(i);
                        }
                    }
                }
            }

        }

        public override void handleCollision(CollidingObject otherCol)
        {
            if (otherCol is Ship)
            {
                if (otherCol.character is Enemy)
                {
                    // dontCollide(otherCol);
                    if (otherCol.character is Minion) 
                    {

                        Minion temp = (Minion)otherCol.character;
                        temp.die();
                        applyDamage(10);
                    }
                    else if (otherCol.character is Fighter)
                    {
                        Fighter temp = (Fighter)otherCol.character;
                        temp.die();
                        applyDamage(10);
                    }
                    else
                    { 
                        ship.moveBack(ship.lastSafePos);
                    }
                }
                else if (otherCol.character is Player)
                {
                    //dontCollide(otherCol);
                    ship.moveBack(ship.lastSafePos);
                }
                
            }

            if (isDead())
            {
                Console.WriteLine(GetType().Name + " died");
            }
            ship.lastSafePos = position;
        }

        public override void applyDamage(int damage)
        {
            if(!StarWarsFighter.debug)
                base.applyDamage(damage);
        }
        protected void LoadValues()
        {
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream("Content/" +"Ships/Player/" + ship.type + "/" + ship.type + ".txt");
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
                        maxHealthship = health;
                        ship.speed = Int16.Parse(sreader.ReadLine());
                    }

                    if (s == "lasercannon")                                         //Interpretiere nächsten 2 Zeilen als X,Y für ein Offset
                    {
                        LaserCanon lc = new LaserCanon(myGame, this, position);
                        var temp =sreader.ReadLine().Split(',');
                        lc.bulletColor =new Color(Int16.Parse(temp[0]), Int16.Parse(temp[1]), Int16.Parse(temp[2]));
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
                    if (s == "seismicbomb")                                         //Seimic Bombs , 1 Offset only
                    {
                        SeismicBomb sb = new SeismicBomb(myGame, this, position);
                        //var temp = sreader.ReadLine().Split(',');
                        //sb.bulletColor = new Color(Int16.Parse(temp[0]), Int16.Parse(temp[1]), Int16.Parse(temp[2]));
                        sb.damage = Int16.Parse(sreader.ReadLine());
                        sb.rateOfFire = float.Parse(sreader.ReadLine());
                        sb.ammo = Int16.Parse(sreader.ReadLine());
                        weapon.Add(sb);
                    
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
                       // Weapon wp = weapon.Find(w => w is SeismicBomb);
                        offsets.ForEach(o =>
                        {
                            sb.bulletOffset.Add(o);
                        });
                    }
                    if (s=="rocketcannon")
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
                Console.WriteLine("Couldnt load file:Content/" + Ship.shipDir + ship.type + "/" + ship.type + ".txt");
            }
        }

    }
}
