using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace spaceInvader
{
    class Animation
    {
        public Texture2D spriteStrip; // The image representing the collection of images used for animation
        public float scale; // The time since we last updated the frame
        int elapsedTime; // The time since we last updated the frame
        public int frameTime; // The time we display a frame until the next 
        public int frameCount; // The number of frames that the animation contains
        public int currentFrame; // The index of the current frame we are displaying
        Color color; // The color of the frame we will be displaying
        Rectangle sourceRect = new Rectangle(); // The area of the image strip we want to display
        Rectangle destinationRect = new Rectangle(); // The area where we want to display the image strip in the game
        public int FrameWidth; // Width of a given frame
        public int FrameHeight; // Height of a given frame
        public bool Active; // The state of the Animation
        public bool Looping; // Determines if the animation will keep playing or deactivate after one run
        public Vector2 Position; // Width of a given frame
        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int currentFrame, int frameCount, int frametime, Color color, float scale, bool looping)
        {
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;
            this.currentFrame = currentFrame;

            Looping = looping;
            Position = position;
            spriteStrip = texture;
            currentFrame = 0;

            // Set the time to zero
            elapsedTime = 0;

            // Set the Animation to active by default
            Active = true;
        }


        public void Update(GameTime gameTime)
        {
            if (Active == false) return;

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime > frameTime)
            {
                currentFrame++;
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;

                    if (Looping == false)
                    {
                        Active = false;
                    }
                }
                elapsedTime = 0;
            }

            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
            destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale) / 2,
                (int)Position.Y - (int)(FrameHeight * scale) / 2,
                (int)(FrameWidth * scale),
                (int)(FrameHeight * scale));

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
            }
        }
    }
}