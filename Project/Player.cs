using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Project
{
    public class Player : Thing
    {
        int health; 
        int frames = 0;
        double totalElapsed;
        double jumpStartTime;
        bool isjumping = false;
        const int gravity = 2;
        Vector2 velocity = Vector2.Zero;
        bool pausing = false;
        bool unpausing = false;
        public bool Controller;
        SpriteFont font;
        int score;
        private bool starcollide = false;
        private int StarCol;
        Thing Collider;


        public Player(Game game, Texture2D _texture, Vector2 _position, Rectangle _boundingBox, SpriteFont _font) : base(game, _texture, _position, _boundingBox)
        {
            health = 4; 
            Game.paused = false;
            score = 0;
            font = _font;
        }

        public override void Update(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (!Game.paused)
            {
                // Handle timing issues
                float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                totalElapsed += elapsed;
                long delay = (long)totalElapsed / 80;

                int heightOverFloor = GetHeightOverFloor(Game);
                frames = 6;

                // What does the player want to do?
                int dir = 0;
                Vector2 leftthumbstick = GamePad.GetState(0).ThumbSticks.Left;


                if (Keyboard.GetState().IsKeyDown(Keys.A) || leftthumbstick.X < 0) dir--;
                if (Keyboard.GetState().IsKeyDown(Keys.D) || leftthumbstick.X > 0) dir++;
                bool jump = (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(0).IsButtonDown(Buttons.A)) && heightOverFloor == 0;
                bool falling = !isjumping && heightOverFloor > 0;
                bool sliding = (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || GamePad.GetState(0).IsButtonDown(Buttons.LeftTrigger)) && heightOverFloor == 0;
                if (jump)
                {
                    isjumping = true;
                    jumpStartTime = totalElapsed;
                }
                else if (isjumping && jumpStartTime + 500 < totalElapsed)
                {
                    isjumping = false;
                }

                // Determine horizontal speed modifier
                int div = 7;
                if (sliding)
                {
                    div = 4;
                }

                // Vertical movement (ignores frames)
                if (falling)
                {
                    int inc = (int)elapsed / 3;
                    // Fall no further than floor
                    if (inc > heightOverFloor) inc = heightOverFloor;
                    BigBoundingBox.Y += inc;
                    Position.Y += inc;
                }
                if (isjumping)
                {
                    double jumpTime = totalElapsed - jumpStartTime;
                    int inc = (int)(elapsed * 20 / (jumpTime + 10));
                    BigBoundingBox.Y -= inc;
                    Position.Y -= inc;
                }

                if (dir != 0)
                {
                    // Select the frame
                    int frameStart = dir == -1 ? 3 : 7;

                    int frameCount = 3;
                    if (sliding)
                    {
                        frameStart = dir == -1 ? 0 : 12;
                        frameCount = 1;
                    }
                    frames = (isjumping || falling) ? frameStart : ((int)delay % frameCount) + frameStart;


                    // Horizontal movement
                    int inc = (int)elapsed / div * dir;
                    // Dip a toe in
                    BigBoundingBox.X += inc;

                    Collider = this.IsColliding(Game);
                    if (Collider is Block)
                    {
                        // Hit something, so undo the change and stop
                        BigBoundingBox.X -= inc;
                    }
                    else
                    {
                        Position.X += inc;
                        delay = (long)totalElapsed / 80;
                    }
                }

                if (Collider == null)
                {
                    // Only needed if stars move
                    Collider = this.IsColliding(Game);
                }
                if (Collider is Star)
                {
                    score++;
                    starcollide = true;
                    Game.DeadThings.Add(Collider);
                }
                else
                {
                    starcollide = false;
                }

                // Now draw it
                sourceRect = new Rectangle(42 * frames, 0, 42, 60);
            }
            spriteBatch.Draw(Texture, Position, sourceRect, Color.White);
            spriteBatch.DrawString(font, "SCORE:  " + score, new Vector2(100, 700), Color.White);
            spriteBatch.DrawString(font, "HEALTH:  " + health, new Vector2(1200, 700), Color.White);



            if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(0).IsButtonDown(Buttons.Start))
            {
                pausing = true;
                if (GamePad.GetState(0).IsButtonDown(Buttons.Start))
                    Controller = true;
                else
                    Controller = false;
            }
            else if (pausing)
            {
                Game.paused = true;
                pausing = false;
            }

            if (!pausing && Game.paused)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(0).IsButtonDown(Buttons.A))
                {
                    unpausing = true;
                }
                else if (unpausing)
                {
                    Game.paused = false;
                    unpausing = false;
                }
            }


        }        
    }
}