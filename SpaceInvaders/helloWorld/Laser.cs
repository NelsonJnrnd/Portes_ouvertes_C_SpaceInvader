using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace spaceInvader
{
    class Laser
    {
        public enum LaserType { green, red, big, invader}
        private Vector2 _pos;
        private int _speedX;
        private int _speedY;
        private int _rotation;
        private bool _isBigLaser;
        private LaserType _type;
        public virtual Vector2 Pos { get => _pos; set => _pos = value; }

        public Texture2D Texture { get; set; }
        public LaserType Type { get => _type; set => _type = value; }

        public Laser(LaserType laserType,Vector2 pos,int rotation,int speedX,int speedY, bool isBigLaser = false)
        {
            Type = laserType;
            switch (Type)
            {
                case LaserType.green:
                    Texture = Game1.laserGreenTexture;
                    break;
                case LaserType.red:
                    Texture = Game1.laserRedTexture;
                    break;
                case LaserType.big:
                    Texture = Game1.bigLaserTexture;
                    break;
                case LaserType.invader:
                    Texture = Game1.invaderLaserTexture;
                    break;
                default:
                    break;
            }
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

        
        public virtual void Draw(SpriteBatch spriteBatch)
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
    }
}
