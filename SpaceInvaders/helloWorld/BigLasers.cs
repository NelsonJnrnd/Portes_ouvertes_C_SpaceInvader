using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using spaceInvader;

namespace helloWorld
{
    class BigLasers
    {
        BigLaser _laserLeft;
        BigLaser _laserRight;
        Player _source;

        public BigLasers(int rotation, int speedX, int speedY, Player source)
        {
            _source = source;
            _laserLeft = new BigLaser(new Vector2(_source.Pos.X + 22, _source.Pos.Y - 470), rotation, speedX, speedY, this);
            _laserRight = new BigLaser(new Vector2(_source.Pos.X + 77, _source.Pos.Y - 470), rotation, speedX, speedY, this);
        }

        internal BigLaser LaserLeft { get => _laserLeft;}
        internal BigLaser LaserRight { get => _laserRight;}

        public void Draw(SpriteBatch spriteBatch)
        {
            updateLaserPos();
            _laserLeft.Draw(spriteBatch);
            _laserRight.Draw(spriteBatch);
        }
        public void updateLaserPos()
        {
            _laserLeft.Pos = new Vector2(_source.Pos.X + 22, _source.Pos.Y - 470);
            _laserRight.Pos = new Vector2(_source.Pos.X + 77, _source.Pos.Y - 470);
        }
    }
}
