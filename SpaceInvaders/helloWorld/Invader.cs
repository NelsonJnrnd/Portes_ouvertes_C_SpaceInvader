using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static spaceInvader.Laser;

namespace spaceInvader
{
    class Invader
    {
        int screenSizeWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int screenSizeHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        int shootLaser = 0;
        bool goLeft = false;
        bool isComming = true;
        int rndShootTime;

        Random rnd = new Random();
        int rndPointY;
        int rndPointX;

        Texture2D _texture;
        Vector2 _pos;
        Texture2D _laserTexture;
        LaserType _laserType;
        List<Laser> _invaderLaserList;
        


        public int Pv { get; set; }
        public Invader(Vector2 pos, Texture2D texture, LaserType laserType, List<Laser> invaderLaserList,int pv)
        {
            _texture = texture;
            _pos = pos;
            _laserType = laserType;
            _invaderLaserList = invaderLaserList;
            this.Pv = pv;
            rndPointY = rnd.Next(10, screenSizeHeight / 2 - _texture.Width / 2);
            rndPointX = rnd.Next(310, screenSizeWidth - 310);
        }
        
        public void Update(GameTime gameTime)
        {
            rndShootTime = rnd.Next(200, 700);

            if (shootLaser >= rndShootTime)
            {
                _invaderLaserList.Add(new Laser(_laserType, new Vector2((int)_pos.X + 44, (int)_pos.Y + 60), 0, 0, -8));
                shootLaser = 0;
            }

            if (_pos.Y > rndPointY && _pos.X <= rndPointX)
            {
                isComming = false;
            }
            else if (_pos.Y > rndPointY && _pos.X >= rndPointX)
            {
                isComming = false;
            }
            else if (_pos.Y == rndPointY)
            {
                isComming = false;
            }
            else if (_pos.Y < rndPointY && _pos.X < rndPointX)
            {
                isComming = true;
            }
            else if (_pos.Y < rndPointY && _pos.X > rndPointX)
            {
                isComming = true;
            }            

            if (isComming)
            {
                isCommingBehaviour();
            }
            else
            {
                isOnTheScreenBehaviour();
            }
            shootLaser++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _pos, Color.White);
        }

        public void isOnTheScreenBehaviour()
        {
            if (_pos.X > screenSizeWidth - _texture.Width)
            {
                goLeft = true;
            }

            if (_pos.X <= 10)
            {
                goLeft = false;
            }

            if (goLeft)
            {
                _pos.X -= 3;
                
            }

            if (!goLeft)
            {
                _pos.X += 3;
            }

            _pos.Y += 1;
        }

        public void isCommingBehaviour()
        {
            if (_pos.X < rndPointX - 10)
            {
                _pos.X += 5;
            }
            else if (_pos.X > rndPointX + 10)
            {
                _pos.X -= 5;
            }

            if (_pos.Y < rndPointY)
            {
                _pos.Y += 5;
            }
        }

        public Rectangle boxCollider
        {
            get
            {
                return new Rectangle((int)_pos.X, (int)_pos.Y, _texture.Width, _texture.Height);
            }
        }    
        
        public Vector2 getPos()
        {
            return _pos;
        }

        public Texture2D getTexture()
        {
            return _texture;
        }
    }
    
}
