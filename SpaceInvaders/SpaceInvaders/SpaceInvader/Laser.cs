using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SpaceInvader
{
    class Laser
    {
        private Vector2 _pos;
        private int _speedX;
        private int _speedY;
        private int _rotation;
        private bool _isBigLaser;
        
        public Vector2 Pos { get => _pos; set => _pos = value; }
        

        public Laser(Texture2D texture,Vector2 pos,int rotation,int speedX,int speedY, bool isBigLaser = false)
        {
            Texture = texture;
            _pos = pos;
            _speedX = speedX;
            _speedY = speedY;
            _rotation = rotation;
            _isBigLaser = isBigLaser;
        }

        public void Update(GameTime gameTime)
        {
            _pos.Y -= _speedY;
            _pos.X -= _speedX;
        }

        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, _pos, null, Color.White, _rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0f);
        }

        public Texture2D getLaserTexture()
        {
            return Texture;
        }

        public Vector2 getLaserPos()
        {
            return _pos;
        }

        public Rectangle BoxCollider
        {
            get
            {
                if (_isBigLaser)
                {
                    return new Rectangle((int)_pos.X, (int)_pos.Y - 540, getLaserTexture().Width, getLaserTexture().Height);
                } else {
                    return new Rectangle((int)_pos.X, (int)_pos.Y, getLaserTexture().Width, getLaserTexture().Height);
                }
            }
        }

        public Texture2D Texture { get; set; }
    }
}
