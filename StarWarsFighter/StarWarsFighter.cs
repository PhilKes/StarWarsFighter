using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.Sockets;
using System.Net;
using WF = System.Windows.Forms;
using System.IO;
using System.Threading;

namespace StarWarsFighter
{

    public static class Constants
    {
        public static int TEX_FULL = 0;
        public static int TEX_EXPLODE = 1;
        public static int TEX_DEAD = 2;
        public static int TEX_WEAPON_ICON = 3;
        public static int DEAD_FRAMECOUNT = 10;
        public static List<string> rebelsShip = new List<string>() { "Awing", "Xwing", "MilleniumFalcon", "Ywing" };
        public static List<string> empireShip = new List<string>() { "TieFighter", "TieInterceptor", "TieBomber", "SlaveOne" };
        public static Color fontColor = new Color(236, 199, 96);
        public static List<Keys> moveKeys = new List<Keys>()
        {
            Keys.W,
            Keys.S,
            Keys.A,
            Keys.D
        };
        public static List<Keys> weaponControlKey = new List<Keys>()
        {
            Keys.Space,
            Keys.LeftControl,
            Keys.LeftAlt
        };
        public static List<Buttons> weaponControlBtn = new List<Buttons>()
        {
           Buttons.RightTrigger,
           Buttons.LeftTrigger,
           Buttons.LeftShoulder,
        };
        public static double ScaleRange(this double Value,
            double FromMinValue, double FromMaxValue,
            double ToMinValue, double ToMaxValue)
        {
            try
            {
                return (Value - FromMinValue) *
                    (ToMaxValue - ToMinValue) /
                    (FromMaxValue - FromMinValue) + ToMinValue;
            }
            catch
            {
                return double.NaN;
            }
        }

    }
    public class StarWarsFighter : Game
    {

        #region Variablen
        public static bool debug = false;
        public static bool isFullscreen = false;

        public Texture2D mouseIcon { get; set; }
        public static int windowWidth { get; set; }
        public static int windowHeight { get; set; }
        public bool showMouse { get; set; }

        public GraphicsDeviceManager graphics { get; set; }
        SpriteBatch spriteBatch { get; set; }
        StarBackground background { get; set; }
        public GUI gui { get; set; }
        public GameAudio gameAudio { get; set; }
        public Score score { get; set; }

        public Player player { get; set; }
        public List<Character> character { get; set; }
        public List<Enemy> enemy { get; set; }
        public List<Item> item { get; set; }

        public MainMenu myMainMenu { get; set; }
        public PauseMenu myPauseMenu { get; set; }
        public DeathMenu myDeathMenu { get; set; }

        public KeyboardState lastKeyStateKey { get; set; }
        public GamePadState lastKeyStatePad { get; set; }
        public MouseState mouseState { get; set; }
        public MouseState lastmouseState { get; set; }
        public KeyboardState newKeyStateKey { get; set; }
        public GamePadState newKeyStatePad { get; set; }

        public List<float> enemySpawntime { get; set; }
        public List<char> enemySpawn { get; set; }

        public float inGameTime { get; set; }
        public static bool inLAN { get; set; }
        public static State gameState { get; set; }

        public enum State
        {
            inMainMenu,
            inGame,
            inPauseMenu,
            inDeathMenu,
            inLANGame

        }
        #endregion
        #region Network

        public TcpClient client;
        string IP;
        int PORT { get; set; }
        public int scorePl1 { get; set; }
        public int scorePl2 { get; set; }
        EnemyLAN LANenemy;
        bool isEnemyConnected=false;
        int BUFFER_SIZE = 2048;
        byte[] readBuffer;
        MemoryStream readStream,writeStream;
        BinaryReader reader;
        BinaryWriter writer;

        #endregion

        public StarWarsFighter()
        {
            graphics = new GraphicsDeviceManager(this);
            windowWidth = Settings.Default.Width;
            windowHeight = Settings.Default.Height;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.IsFullScreen = isFullscreen;
            Content.RootDirectory = "Content";
            graphics.ApplyChanges();
            enemySpawntime = new List<float>();
            background = new StarBackground(this);
            //IsMouseVisible = true;
            enemySpawn = new List<char>();
            showMouse = true;
            lastKeyStateKey = Keyboard.GetState();
            lastKeyStatePad = GamePad.GetState(PlayerIndex.One);
            mouseState = Mouse.GetState();
            lastmouseState = mouseState;
        }


        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gui = new GUI(this);
            gui.Initialize();
            this.Services.AddService(typeof(SpriteBatch), spriteBatch);
            gameAudio = new GameAudio("All");
            base.Initialize();
            myMainMenu = new MainMenu(this);
            Mouse.WindowHandle = Window.Handle;
            gameState = State.inMainMenu;
            gameAudio.playAudioList(gameState);
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                showMouse = false;
            }
            DrawableGO.myGame = this;
            //LoadenemyList();
        }
        public void initializeGame(string playerType)
        {
            inGameTime = 0;
            inLAN = false;
            LoadenemyList();
            Ship.shipDir = "Ships/Player/";
            Components.Clear();
            gameAudio.StopAll();
            character = new List<Character>();
            enemy = new List<Enemy>();
            item = new List<Item>();
            showMouse = false;
            score = new Score(this);
            background = new StarBackground(this);
            player = new Player(this, new Vector2(windowWidth / 2, windowHeight), playerType, Content);
            gameAudio.AddSound("Music");
            gameAudio.PlaySound("Music");
            score.DrawOrder = 10;
            gui.DrawOrder = 20;
            Components.Add(score);
            Components.Add(gui);
            character.Add(player);
            if (!debug) Components.Add(background);
            initItems();
            Components.Add(player);
            base.Initialize();
            gameAudio.setAudioList(State.inGame);
            gameAudio.StopAll();
            gameAudio.playAudioList(StarWarsFighter.State.inGame);

            //
        }
        public void initializeLANGame(string playerType,bool host)
        {
            inGameTime = 0;
            //LoadenemyList();
            inLAN = true;
            Ship.shipDir = "Ships/Player/";
            Components.Clear();
            gameAudio.StopAll();
            scorePl1 = 0;
            scorePl2 = 0;
            character = new List<Character>();
            //enemy = new List<Enemy>();
            item = new List<Item>();
            showMouse = false;
            score = new Score(this);
            background = new StarBackground(this);
            player = new Player(this, new Vector2(windowWidth / 2, windowHeight), playerType, new ContentManager(Services, Content.RootDirectory));
            gameAudio.AddSound("Music");
            gameAudio.PlaySound("Music");
            score.DrawOrder = 10;
            gui.DrawOrder = 20;
            Components.Add(score);
            Components.Add(gui);
            character.Add(player);
            if (!debug) Components.Add(background);
            if(host)
                initItems();
            Components.Add(player);
            base.Initialize();
            gameAudio.setAudioList(State.inGame);
            gameAudio.StopAll();
            gameAudio.playAudioList(StarWarsFighter.State.inGame);
            gameState = State.inLANGame;
            //
        }
        public void initItems()
        {
            AddItem(new Item(this, new Vector2(100, 500)));

            item.ForEach(i => i.Initialize());
        }
        private void Update_Ingame(GameTime gT)
        {
            inGameTime += (float)gT.ElapsedGameTime.TotalSeconds;

            if ((newKeyStateKey.IsKeyDown(Keys.Escape) || newKeyStatePad.IsButtonDown(Buttons.Start)) && !lastKeyStateKey.IsKeyDown(Keys.Escape) && !lastKeyStatePad.IsButtonDown(Buttons.Start))
            {
                gameAudio.setAudioList(State.inGame);
                gameAudio.PauseAll();

                gameState = State.inPauseMenu;
                //IsMouseVisible = true;
                showMouse = true;
                myPauseMenu = new PauseMenu(this);

                gameAudio.UpdateAudio();
                gameAudio.setAudioList(StarWarsFighter.State.inPauseMenu);
                gameAudio.UnpauseSound("Music");
                //gameAudio.playAudioList(StarWarsFighter.State.inPauseMenu);
            }
            base.Update(gT);
        }
        protected override void Update(GameTime gameTime)
        {
            newKeyStateKey = Keyboard.GetState();
            newKeyStatePad = GamePad.GetState(PlayerIndex.One);
            mouseState = Mouse.GetState();
            if (gameState == State.inGame)
            {
                Update_Ingame(gameTime);
                loadEnemies();
                detectCollisions();
            }
            else if (gameState == State.inPauseMenu)
            {
                if ((newKeyStateKey.IsKeyDown(Keys.Escape) || newKeyStatePad.IsButtonDown(Buttons.Start)) && !lastKeyStateKey.IsKeyDown(Keys.Escape) && !lastKeyStatePad.IsButtonDown(Buttons.Start))
                {
                    gameAudio.UnpauseAll();
                    gameState = State.inGame;
                    //IsMouseVisible = false;
                    showMouse = false;
                    //gameAudio.playAudioList(StarWarsFighter.State.inPauseMenu);
                }
                myPauseMenu.Update(gameTime);
                if (inLAN)
                    base.Update(gameTime);
            }
            else if (gameState == State.inMainMenu)
            {
                myMainMenu.Update(gameTime);
            }
            else if (gameState == State.inDeathMenu)
            {
                myDeathMenu.Update(gameTime);
                if (inLAN)
                    base.Update(gameTime);
            }
            else if (gameState == State.inLANGame)
            {
                Update_LANGame(gameTime);
            }
            gameAudio.UpdateAudio();
            lastKeyStateKey = Keyboard.GetState();
            lastmouseState = Mouse.GetState();
            lastKeyStatePad = GamePad.GetState(PlayerIndex.One);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (gameState == State.inGame)
            {
                base.Draw(gameTime);
            }
            else if (gameState == State.inPauseMenu)
            {
                base.Draw(gameTime);
                myPauseMenu.Draw(gameTime);
            }
            else if (gameState == State.inMainMenu)
            {
                myMainMenu.Draw(gameTime);
            }
            else if (gameState == State.inDeathMenu)
            {
                base.Draw(gameTime);
                myDeathMenu.Draw(gameTime);
            }
            else if (gameState == State.inLANGame)
            {
                Draw_LANGame(gameTime);
            }
            if (showMouse)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(mouseIcon, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);
                spriteBatch.End();
            }
        }

        public void removeCharacter(Character character)        // <-------- Player's death
        {
            if (character == player)
            {
                Console.WriteLine("Game over biatch");
                this.character.Remove(character);
                Components.Remove(character);
                //player = null;
                gameAudio.PauseAll();
                gameState = State.inDeathMenu;
                myDeathMenu = new DeathMenu(this);
                showMouse = true;
            }
            else if (character is Enemy)
            {
                enemy.Remove((Enemy)character);
                this.character.Remove(character);
                Components.Remove(character);
                if (enemy.Count == 0 && enemySpawntime.Count == 0)
                {
                    gameState = State.inPauseMenu;
                    myPauseMenu = new PauseMenu(this);
                    showMouse = true;
                }
            }
            else if (character is Player)
            {
                this.character.Remove(character);
                LANenemy = null;
                Components.Remove(character);
            }
        }
        public void removeItem(Item item2Remove)
        {
            item.Remove(item2Remove);
            Components.Remove(item2Remove);
        }

        protected override void LoadContent()
        {
            mouseIcon = Content.Load<Texture2D>("Misc/mouseCursor");
        }

        private void LoadenemyList()
        {
            //enemySpawntime.Add(2f);
            enemySpawntime.Clear();
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream("Content/Levels/Level1.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                int line = 0;
                string s = "";

                while (!((s = sreader.ReadLine()) == "end"))
                {
                    line++;
                    var temp = s.Split(',');
                    enemySpawntime.Add(float.Parse(temp[0]));
                    enemySpawn.Add(char.Parse(temp[1]));
                }

            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Couldnt load file:Level1.txt");
            }
        }
        private void loadEnemies()
        {
            int i = 0;
            // for (int i = 0; i < enemySpawntime.Count; i++)
            //{
            while (i < enemySpawntime.Count)
            {

                if (inGameTime >= enemySpawntime[i])
                {
                    Console.WriteLine(enemySpawn[i] + " spawned at: " + inGameTime);
                    Enemy temp = new Minion(this, new Vector2(StarWarsFighter.windowWidth / 2, 0), Content);
                    if (enemySpawn[i] == 'M')
                    {
                        temp = new Minion(this, new Vector2(StarWarsFighter.windowWidth / 2, 0), Content);
                        AddEnemy(temp);
                        enemySpawntime.Remove(enemySpawntime[i]);
                        enemySpawn.Remove(enemySpawn[i]);
                    }
                    else if (enemySpawn[i] == 'F')
                    {
                        temp = new Fighter(this, new Vector2(StarWarsFighter.windowWidth / 2, 0), Content);
                        AddEnemy(temp);
                        enemySpawntime.Remove(enemySpawntime[i]);
                        enemySpawn.Remove(enemySpawn[i]);
                    }
                    else if (enemySpawn[i] == 'D')
                    {
                        temp = new Destroyer(this, new Vector2(StarWarsFighter.windowWidth / 2, 0), Content);
                        temp.Initialize();
                        if (!detectSpawnCollision(temp))
                        {
                            enemySpawntime.Remove(enemySpawntime[i]);
                            enemySpawn.Remove(enemySpawn[i]);
                            temp = new Destroyer(this, new Vector2(StarWarsFighter.windowWidth / 2, 0), Content);
                            AddEnemy(temp);
                        }
                        else
                        {
                            temp.Dispose();
                            temp = null;
                            enemySpawntime[i] += 1f;
                        }
                    }
                    i++;
                    //break;
                }
                else
                    break;
            }
        }
        private bool detectSpawnCollision(Enemy enemy)
        {
            if (detectCollisions(new Rectangle((int)(enemy.ship.texPosition.X + enemy.ship.collisionBoxOffset.X / 2),
                                                    (int)(enemy.ship.texPosition.Y + enemy.ship.collisionBoxOffset.Y / 2),
                                                    enemy.ship.renderTexture.Width, enemy.ship.renderTexture.Height)))
            {
                return true;
            }

            return false;
        }

        private void AddEnemy(Enemy enemi)
        {
            enemy.Add(enemi);
            character.Add(enemi);
            Components.Add(enemi);
        }
        private void AddItem(Item item)
        {
            this.item.Add(item);
            Components.Add(item);
        }
        private void RemoveItem(Item item)
        {
            this.item.Remove(item);
            Components.Remove(item);
        }

        public void detectCollisions()
        {
            List<CollidingObject> colliding = new List<CollidingObject>();

            character.ForEach(C =>
            {
                C.colliders.ForEach(Coll =>
                {
                    colliding.Add(Coll);
                });
            });
            character.ForEach(C =>
            {
                C.colliders.ForEach(c =>
                {
                    colliding.Remove(c);
                });
                C.detectCollision(colliding);
                C.colliders.ForEach(c =>
                {
                    colliding.Add(c);
                });
            });

            character.ForEach(C =>
            {
                C.ship.lastSafePos = C.ship.position;
            });

            item.ForEach(i =>
            {
                if(player!=null)
                    i.detectCollision(player.colliders);
                if(LANenemy!=null)
                    i.detectCollision(LANenemy.colliders);
            });
            //Console.WriteLine("_--------------------------");
        }
        public void detectCollisions(CollidingObject colliding)
        {
            character.ForEach(C =>
            {
                if (colliding.character is Player && C is Player)
                {
                    return;
                }
                List<CollidingObject> coll = new List<CollidingObject>();
                coll.Add(colliding);
                C.detectCollision(coll);

            });
            character.ForEach(C =>
            {
                C.ship.lastSafePos = C.ship.position;
            });
            //Console.WriteLine("_--------------------------");
        }
        public bool detectCollisions(Rectangle colBox)
        {
            bool isColliding = false;
            character.ForEach(C =>
            {
                C.colliders.ForEach(c =>
                {
                    if (colBox.Intersects(c.collisionBox))
                    {
                        isColliding = true;
                    }
                });
            });
            return isColliding;
        }

        #region Network Methods

        public bool connect2Server()
        {
            try
            {
                IP = Settings.Default.DefaultIP;
                PORT = Settings.Default.DefaultPort;
                client = new TcpClient();
                client.NoDelay = true;
                client.Connect(IP, PORT);
                readBuffer = new byte[BUFFER_SIZE];

                client.GetStream().BeginRead(readBuffer, 0, BUFFER_SIZE, StreamReceived, null);
                readStream = new MemoryStream();
                writeStream = new MemoryStream();
                reader = new BinaryReader(readStream);
                writer = new BinaryWriter(writeStream);

                return true;
            }
            catch (Exception e) { return false;/* WF.MessageBox.Show("Couldnt connect");*/ }
        }
        private void StreamReceived(IAsyncResult ar)
        {
            int bytesRead = 0;
            try
            {
                lock (client.GetStream())
                {
                    bytesRead = client.GetStream().EndRead(ar);
                }

            }
            catch (Exception e)
            {
               //WF.MessageBox.Show(e.Message);
            }
            if (bytesRead == 0)
            {
                client.Close();
                return;
            }
            byte[] data = new byte[bytesRead];
            for (int i = 0; i < bytesRead; i++)
                data[i] = readBuffer[i];

            ProcessData(data);
            client.GetStream().BeginRead(readBuffer, 0, BUFFER_SIZE, StreamReceived, null);
        }

        private void ProcessData(byte[] data)
        {
            readStream.SetLength(0);
            readStream.Position = 0;

            readStream.Write(data, 0, data.Length);
            readStream.Position = 0;
            Protocol p;
            try
            {
                p = (Protocol)reader.ReadByte();
                if (p == Protocol.Connected)
                {
                    /*byte id = reader.ReadByte();
                    string ip = reader.ReadString();*/
                    if (!isEnemyConnected)
                    {
                        // WF.MessageBox.Show(String.Format("{0} - {1} connected", ip, id));
                        /*writeStream.Position = 0;
                        writer.Write((byte)Protocol.Connected);
                        SendData(GetDataFromMemoryStream(writeStream));*/
                        //spawnEnemyPlayer();
                        Thread.Sleep(100);
                        sendRespawn();
                        sendItems();
                        isEnemyConnected = true;
                    }
                }
                else if (p == Protocol.Disconnected)
                {
                    /*byte id = reader.ReadByte();
                    string ip = reader.ReadString();*/
                    //WF.MessageBox.Show(String.Format("{0} - {1} disconnected", ip, id));
                    removeCharacter(LANenemy);
                    
                    isEnemyConnected = false;
                }
                else if (p == Protocol.PlayerMoved)
                {
                    float x = reader.ReadSingle();
                    //float y = reader.ReadSingle();
                    float pX = reader.ReadSingle();
                    float pY = reader.ReadSingle();
                    /*byte id = reader.ReadByte();
                    string ip = reader.ReadString();*/
                    //Player.enemyMove = new Vector2(x, 0);
                    LANenemy.ship.move = new Vector2(x, 0);
                    LANenemy.ship.position = new Vector2(pX, pY);
                    //WF.MessageBox.Show(String.Format("{0} - {1} disconnected", ip, id));
                }
                else if (p == Protocol.PlayerShot)
                {
                    int wNr = reader.ReadInt16();
                    LANenemy.weapon[wNr].fireBullet(false);
                }
                else if (p == Protocol.Respawn)
                {
                    string stype = reader.ReadString();
                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    spawnEnemyPlayer(stype,x,y);
                }
                else if (p == Protocol.Death)
                {
                    //scorePl2 = reader.ReadInt16();
                    //scorePl1 = reader.ReadInt16();
                    scorePl2++;
                   /* byte id = reader.ReadByte();
                    string ip = reader.ReadString();*/
                }
                else if (p == Protocol.Item)
                {
                    item.ForEach(i => Components.Remove(i));
                    this.item.Clear();
                    
                    while (true)
                    {
                        try
                        {
                            float x = reader.ReadSingle();
                            float y = reader.ReadSingle();
                            Item i=new Item(this, new Vector2(windowWidth-x, windowHeight-y));
                            i.Initialize();
                            AddItem(i);
                        }
                        catch (Exception e)
                        {
                            break;
                        }
                    } 
                }
            }
            catch (Exception e)
            {
                //WF.MessageBox.Show(e.Message);
            }
        }
        private byte[] GetDataFromMemoryStream(MemoryStream ms)
        {
            byte[] result;

            //Async method called this, so lets lock the object to make sure other threads/async calls need to wait to use it.
            lock (ms)
            {
                int bytesWritten = (int)ms.Position;
                result = new byte[bytesWritten];

                ms.Position = 0;
                ms.Read(result, 0, bytesWritten);
            }

            return result;
        }
        public void SendData(byte[] b)
        {
            //Try to send the data.  If an exception is thrown, disconnect the client
            try
            {
                lock (client.GetStream())
                {
                    client.GetStream().BeginWrite(b, 0, b.Length, null, null);
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("Client {0}:  {1}", IP, e.ToString());
            }
        }

        public void sendMovement(Vector2 move)
        {
            writeStream.Position = 0;
            writer.Write((byte)Protocol.PlayerMoved);
            writer.Write(move.X);
            writer.Write(windowWidth-player.ship.position.X);
            writer.Write(windowHeight-player.ship.position.Y);
            SendData(GetDataFromMemoryStream(writeStream));
        }
        public void sendShot(int weaponNr)
        {
            writeStream.Position = 0;
            writer.Write((byte)Protocol.PlayerShot);
            writer.Write(weaponNr);

            SendData(GetDataFromMemoryStream(writeStream));
        }
        public void sendItems()
        {
            writeStream.Position = 0;
            writer.Write((byte)Protocol.Item);
            item.ForEach(i =>
            {
                writer.Write(i.position.X);
                writer.Write(i.position.Y);
            });
            writer.Write("end");

            SendData(GetDataFromMemoryStream(writeStream));
        }
        public void sendDeath()
        {
            writeStream.Position = 0;
            writer.Write((byte)Protocol.Death);
            writer.Write(scorePl1);
            writer.Write(scorePl2);
            SendData(GetDataFromMemoryStream(writeStream));
        }
        public void sendRespawn()
        {
            writeStream.Position = 0;
            writer.Write((byte)Protocol.Respawn);
            writer.Write(player.ship.type);
            writer.Write(windowWidth-player.ship.position.X);
            writer.Write(windowHeight-player.ship.position.Y);
            SendData(GetDataFromMemoryStream(writeStream));
        }

        private void spawnEnemyPlayer(string type,float x,float y)
        {
            LANenemy = new EnemyLAN(this, new Vector2(windowWidth / 2, 0), type, Content);
            LANenemy.ship.position = new Vector2(x, y);
            character.Add(LANenemy);
            Components.Add(LANenemy);
        }

        private void Update_LANGame(GameTime gameTime)
        {
            Update_Ingame(gameTime);
            detectCollisions();
        }

        private void Draw_LANGame(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion
    }
}


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
*/
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
