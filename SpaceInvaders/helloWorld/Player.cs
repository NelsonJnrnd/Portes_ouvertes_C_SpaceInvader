using System.Collections.Generic;
using helloWorld;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static spaceInvader.Game1;
using static spaceInvader.Laser;

namespace spaceInvader
{
    class Player
    {     
        private int _pv;
        private Vector2 _pos;
        private Texture2D _texture;
        private int _speed;
        private PlayerShootMode _shootMode;
        private bool _isShootingBigLaser;
        int screenSizeWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int screenSizeHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        public int Speed { get => _speed; set => _speed = value; }

        public Player(int pv,int speed, Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _pv = 0;            
            _pos = position;
            _speed = speed;
            ShootMode = PlayerShootMode.basic;
            _isShootingBigLaser = false;
        }

        public Rectangle BoxCollider
        {
            get
            {
                return new Rectangle((int)_pos.X + 20, (int)_pos.Y + 20, _texture.Width - 20, _texture.Height - 20);
            }
        }

        public int Pv { get => _pv; set => _pv = value; }
        public Vector2 Pos { get => _pos; set => _pos = value; }
        public bool IsShootingBigLaser { get => _isShootingBigLaser; set => _isShootingBigLaser = value; }
        public PlayerShootMode ShootMode { get => _shootMode; set => _shootMode = value; }

        public void Update(GameTime gameTime,KeyboardState kbState)
        {
            
            if (_pos.X > 0)
                if (kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.Left))
                    _pos.X -= _speed;

            if (_pos.Y > 0)
                if (kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.Up))
                    _pos.Y -= _speed;
            if (_texture != null)
            {
                if (_pos.Y + _texture.Height < screenSizeHeight)
                    if (kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.Down))
                        _pos.Y += _speed;

                if (_pos.X + _texture.Width < screenSizeWidth)
                    if (kbState.IsKeyDown(Keys.D) || kbState.IsKeyDown(Keys.Right))
                        _pos.X += _speed;
            }            
        } 
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
            {
                spriteBatch.Draw(_texture, _pos, Color.White);
            }
            
        }
        public Vector2 getPlayerPos()
        {
            return _pos;
        }

        public Texture2D getPlayerTexture()
        {
            return _texture;
        }

        public int getPlayerPv()
        {
            return _pv;
        }
        public List<Laser> Shoot()
        {
            List<Laser> lasers = new List<Laser>();
            Speed = 10;
            switch (ShootMode)
            {
                case PlayerShootMode.basic:
                        lasers.Add(new Laser(LaserType.green, new Vector2(getPlayerPos().X + getPlayerTexture().Width / 2 - 8, getPlayerPos().Y), 0, 0, 10));
                        lasers.Add(new Laser(LaserType.green, new Vector2(getPlayerPos().X + getPlayerTexture().Width / 2 + 8, getPlayerPos().Y), 0, 0, 10));
                    break;
                case PlayerShootMode.triple:
                    lasers.Add(new Laser(LaserType.red, new Vector2(getPlayerPos().X + getPlayerTexture().Width / 2 - laserRedTexture.Width + 5, getPlayerPos().Y), 210, 5, 10));
                    lasers.Add(new Laser(LaserType.red, new Vector2(getPlayerPos().X + getPlayerTexture().Width / 2 - laserRedTexture.Width, getPlayerPos().Y), 210, 5, 10));
                    lasers.Add(new Laser(LaserType.green, new Vector2(getPlayerPos().X + getPlayerTexture().Width / 2 - 5, getPlayerPos().Y), 0, 0, 10));
                    lasers.Add(new Laser(LaserType.green, new Vector2(getPlayerPos().X + getPlayerTexture().Width / 2 + 5, getPlayerPos().Y), 0, 0, 10));
                    lasers.Add(new Laser(LaserType.red, new Vector2(getPlayerPos().X + getPlayerTexture().Width / 2 - laserRedTexture.Width + 30, getPlayerPos().Y), -210, -5, 10));
                    lasers.Add(new Laser(LaserType.red, new Vector2(getPlayerPos().X + getPlayerTexture().Width / 2 - laserRedTexture.Width + 35, getPlayerPos().Y), -210, -5, 10));

                    break;
                case PlayerShootMode.bigLaser:
                    if (!IsShootingBigLaser)
                    {
                        IsShootingBigLaser = true;
                        BigLasers bigLasers = new BigLasers(0, 0, 0, this);

                        lasers.Add(bigLasers.LaserLeft);
                        lasers.Add(bigLasers.LaserRight);
                        //lasers.Add(new BigLaser(getPlayerPos(), 0, 0, 0, this));
                    }
                    break;
                default:
                    break;
            }
            return lasers;
        }
    }
}
