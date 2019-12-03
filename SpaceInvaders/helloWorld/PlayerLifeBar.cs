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
    class PlayerLifeBar
    {
        int _playerPv;
        Player _player;
        Vector2 _pos;
        Texture2D _texture;
        Game1 g = new Game1();

        public Texture2D Texture { get => _texture; set => _texture = value; }

        public PlayerLifeBar(Player player, Texture2D texture)
        {
            _player = player;
            _playerPv = player.getPlayerPv();
            _texture = texture;
            _pos = new Vector2(0,0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
            {
                spriteBatch.Draw(_texture,new Rectangle((int)_pos.X - 30,(int)_pos.Y + 95, _player.Pv,_texture.Height),Color.White);
            }
        }

        public void Update(GameTime gameTime)
        {
            _pos = new Vector2(_player.getPlayerPos().X, _player.getPlayerPos().Y);            
        }

        public Texture2D getLifeBarTexture()
        {
            return _texture;
        }

        public int getLifeBarPv()
        {
            return _playerPv;
        }

        public Vector2 getLifeBarPos()
        {
            return _pos;
        }
    }
}
