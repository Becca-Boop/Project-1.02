using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Thing
    {
        protected Vector2 Position;
        protected Texture2D Texture;
        protected Rectangle LittleBoundingBox;
        public Rectangle BigBoundingBox;
        protected Rectangle sourceRect;
        public Game Game;

        public Thing(Game game, Texture2D _texture, Vector2 _position, Rectangle _boundingBox)
        {
            Game = game;
            Texture = _texture;
            Position = _position;
            LittleBoundingBox = _boundingBox;
            BigBoundingBox = new Rectangle(LittleBoundingBox.X + (int)Position.X, LittleBoundingBox.Y + (int)Position.Y, LittleBoundingBox.Width, LittleBoundingBox.Height);
        }

        public Thing IsColliding(Game Game)
        {
            foreach (var Thing in Game.Things)
            {
                if (this.IsColliding(Thing))
                {
                    return Thing;
                }
            }
            return null;
        }


        public bool IsColliding(Thing otherThing)
        {
            if (otherThing == this)
            {
                return false;
            }
            else
            {
                return BigBoundingBox.Intersects(otherThing.BigBoundingBox);
            }
        }



        public virtual int GetHeightOver(Thing Thing)
        {
            return 9999;
        }
        public int GetHeightOverFloor(Game game)
        {
            int result = 9999;
            foreach (var Thing in Game.Things)
            {
                int height = Thing.GetHeightOver(this);
                if (height < result)
                {
                    result = height;
                }
            }
            
            return result;
        }

        public virtual void Update(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}

