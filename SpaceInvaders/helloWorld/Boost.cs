using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace spaceInvader
{
    class Boost
    {
        Vector2 pos;
        Texture2D texture;
        public Rectangle BoxCollider
        {
            get
            {
                return new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
            }
        }


        int speed;
        public Boost(Vector2 pos, Texture2D texture, int speed)
        {
            this.pos = pos;
            this.texture = texture;
            this.speed = speed;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, pos, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            pos.Y += speed;
        }


        public Texture2D getTexture()
        {
            return texture;
        }

        public Vector2 getPos()
        {
            return pos;
        }
    }
}
