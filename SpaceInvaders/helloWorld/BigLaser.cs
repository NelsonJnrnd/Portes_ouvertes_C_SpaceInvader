using helloWorld;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace spaceInvader
{
    internal class BigLaser : Laser
    {
        BigLasers _source;

        public override Vector2 Pos { get => base.Pos; set => base.Pos = value; }
        public BigLaser(Vector2 pos, int rotation, int speedX, int speedY, BigLasers source) : base(LaserType.big, pos, rotation, speedX, speedY, true)
        {
            _source = source;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            _source.updateLaserPos();
            base.Draw(spriteBatch);
        }
    }
}
