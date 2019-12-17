using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvader
{
    class Explosion 
    {
        public Animation _explosionAnimation;
        public bool Active;

       

        private Vector2 _position;
        private Texture2D _texture;
        private int _timeToLive;

        public Vector2 Size { get; set; }

        public Explosion()
        {

        }

        public void Initialize(Animation animation, Vector2 position)
        {
            _position = position;
            _explosionAnimation = animation;
            _texture = animation.spriteStrip;
            Size = new Vector2(animation.FrameWidth, animation.FrameHeight);
            Active = true;
            _timeToLive = 30;
        }

        public void Update(GameTime gameTime)
        {
            _explosionAnimation.Update(gameTime);

            _timeToLive--;
            if (_timeToLive <= 0)
            {
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _explosionAnimation.Draw(spriteBatch);
        }
    }
}