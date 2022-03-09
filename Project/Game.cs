using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework.Content;

namespace Project
{
    /// <summary> 
    /// This is the main type for your game.
    /// </summary> 
    /// 
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static bool paused = false;
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Texture2D PlayerSprite;
        public Texture2D BlockSprite;
        public Texture2D StarSprite;
        private Texture2D background;
        public Texture2D PauseOverlay;
        public Texture2D PauseOverlayController;
        public List<Thing> Things = new List<Thing>();
        public List<Thing> DeadThings;

        public Player Player;

        Map map;
        SpriteFont font;

        public int frameCount = 0;

        public Game()
        {
            int height;
            int width;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1440;
            // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 810;
            // set this value to the desired height of your window 
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            Window.IsBorderless = false;
            height = (graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height - 810) / 2;
            width = (graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width - 1440) / 2;
            Window.Position = new Point(width, height - 30);
            graphics.ApplyChanges();
        }

        public void Log(String s)
        {
            System.Diagnostics.Trace.WriteLine(s);
        }

        protected override void Initialize()
        {
            map = new Map();
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            PlayerSprite = Content.Load<Texture2D>("SPSS");
            BlockSprite = Content.Load<Texture2D>("block");
            StarSprite = Content.Load<Texture2D>("star");
            background = Content.Load<Texture2D>("Background");
            PauseOverlay = Content.Load<Texture2D>("Pause Menu");
            PauseOverlayController = Content.Load<Texture2D>("Pause Menu Controller");
            font = Content.Load<SpriteFont>("Score");
            Tiles.Content = Content;



            int width = 46;
            int height = 26;

            //map.Generate(width, height, 32);






            Things.Add(new Block(this, BlockSprite, new Vector2(600, 540), new Rectangle(0, 0, 28, 40)));
            Things.Add(new Block(this, BlockSprite, new Vector2(50, 460), new Rectangle(0, 0, 28, 40)));
            Things.Add(new Block(this, BlockSprite, new Vector2(28, 580), new Rectangle(0, 0, 600, 20)));
            Things.Add(new Block(this, BlockSprite, new Vector2(50, 500), new Rectangle(0, 0, 300, 20)));
            Things.Add(new Block(this, BlockSprite, new Vector2(720, 500), new Rectangle(0, 0, 300, 20)));
            Things.Add(new Star(this, StarSprite, new Vector2(770, 450), new Rectangle(0, 0, 40, 40)));
            Things.Add(new Star(this, StarSprite, new Vector2(200, 450), new Rectangle(0, 0, 40, 40)));
            Things.Add(new Star(this, StarSprite, new Vector2(300, 450), new Rectangle(0, 0, 40, 40)));
            Things.Add(new Star(this, StarSprite, new Vector2(400, 450), new Rectangle(0, 0, 40, 40)));
            Things.Add(new Star(this, StarSprite, new Vector2(500, 450), new Rectangle(0, 0, 40, 40)));
            Things.Add(new Star(this, StarSprite, new Vector2(1100, 450), new Rectangle(0, 0, 40, 40)));
            Player = new Player(this, PlayerSprite, new Vector2(100, 520), new Rectangle(2, 2, 35, 48), font);
            Things.Add(Player);



            Random rnd = new Random();
            int size = 64;
            int X = 10;
            int Y = 8;
            for (int x = 0; x < X; x++)
            {
                for (int y = 0; y < Y; y++)
                {
                    int number = rnd.Next(-1, 2);

                    if (number > 0)
                        Things.Add(new Block(this, BlockSprite, new Vector2(x * size + 500, y * size + 310), new Rectangle(0, 0, size, size)));

                }
            }
            

        }
        /// <summary> 
        /// UnloadContent will be called once per game and is the place to unload 
        /// game-specific content. 
        /// </summary> 

        protected override void UnloadContent()
        {
        }

        int c = 0;
        protected override void Update(GameTime gameTime)
        {

            if (paused == true)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();
            }
            


        }

        /// <summary> 
        /// This is called when the game should draw itself. 
        /// </summary> 
        /// <param name="gameTime">Provides a snapshot of timing values.</param> 

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, 1440, 810), Color.White);

            map.Draw(spriteBatch);

            DeadThings = new List<Thing>();

            foreach (var Thing in Things)
            {
                Thing.Update(gameTime, spriteBatch);
            }

            c++;
            if (c < 500) System.Diagnostics.Debug.WriteLine(DeadThings.Count);
            foreach (var Thing in DeadThings)
            {
                Things.Remove(Thing);
            }

            if (paused)
            {
                if (Player.Controller)
                    spriteBatch.Draw(PauseOverlayController, Vector2.Zero, Color.White);
                else
                    spriteBatch.Draw(PauseOverlay, Vector2.Zero, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);

        }

    }

}