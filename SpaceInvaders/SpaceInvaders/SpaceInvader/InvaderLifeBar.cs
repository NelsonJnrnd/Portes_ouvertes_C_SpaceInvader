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
    class InvaderLifeBar
    {
 
int _invaderPv;
        Invader _invader;
        Vector2 _pos;
        Texture2D _texture;
        Game1 g = new Game1();

        public Texture2D Texture { get => _texture; set => _texture = value; }

        public InvaderLifeBar(Invader invader, Texture2D texture)
        {
            _invader = invader;
            _invaderPv = invader.Pv;
            _texture = texture;
            _pos = new Vector2(0, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_invader.Pv > 0)
            {
                if (_texture != null)
                {
                    spriteBatch.Draw(_texture, new Rectangle((int)_pos.X - 26, (int)_pos.Y - 18, _invader.Pv, 20), Color.White);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            _pos = new Vector2(_invader.getPos().X, _invader.getPos().Y);
        }

        public Texture2D getLifeBarTexture()
        {
            return _texture;
        }

        public int getLifeBarPv()
        {
            return _invaderPv;
        }

        public Vector2 getLifeBarPos()
        {
            return _pos;
        }

        public int getInvaderLife()
        {
            return _invader.Pv;
        }

        public Invader getInvader()
        {
            return _invader;
        }
    }
}
