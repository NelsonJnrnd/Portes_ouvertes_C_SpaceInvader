using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Autor: Boucherine Badr
/// ProjectName: Space_Invaders
/// </summary>
namespace spaceInvader
{
    public class Game1 : Game
    {
        #region Constants
        int PLAYER_LIFE_BAR_WIDTH;
        int INVADER_LIFE_BAR_WIDTH;

        const int BASE_SCORE = 0;
        const int INVADER_HP = 15;
        const int STARTING_POS_Y_ALIEN = -200;
        #endregion

        #region Fields and properties
        //Initialisation
        public static Texture2D background;
        public static Texture2D laserGreenTexture;
        public static Texture2D laserRedTexture;
        public static Texture2D doubleLaserBoostTexture;
        public static Texture2D bigLaserTexture;
        public static Texture2D vesselTexture;
        public static Texture2D tripleShootBoostTexture;
        public static Texture2D lifeBarTexture;
        public static Texture2D lifeBarUnderTexture;
        public static Texture2D invaderTexture;
        public static Texture2D invaderLaserTexture;
        public static Texture2D blackScreen;
        public static Texture2D buttonsTexture;
        public static Texture2D buttonsTextureDead;
        public static Texture2D explosionTexture;

        Texture2D[] boostTab;
        static Laser bigLaserGauche;
        static Laser bigLaserDroit;
        PlayerLifeBar playerLifeBar;
        static Player player;
        Vector2 startPos = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2);
        static List<Laser> lasers = new List<Laser>();
        List<Boost> boosts = new List<Boost>();
        List<PlayerLifeBar> playerLifeBars = new List<PlayerLifeBar>();
        List<InvaderLifeBar> invaderLifeBars = new List<InvaderLifeBar>();
        List<Invader> invaders = new List<Invader>();
        List<Laser> invadersLasers = new List<Laser>();
        List<Explosion> explosions = new List<Explosion>();
        SpriteFont font;
        SpriteFont fontTitle;
        SpriteFont scoreFont;
        Timer tripleTimeReset = new Timer(resetTripleTime);
        public enum PlayerShootMode { basic, triple, bigLaser };

        //Definition
        Random rnd = new Random();
        int screenSizeWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int screenSizeHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        int score = BASE_SCORE;
        int playerSpeed = 10;
        int count = 0;
        int randomBoostTime = 0;
        int incrementGauche = 0;
        int incrementDroit = 0;
        int countInvaders = 0;
        int shootInvaderLaser = 0;
        bool isDead = false;
        bool isMenuActive = true;
        bool escShowMenu = false;
        KeyboardState oldState;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #endregion

        #region Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenSizeWidth;
            graphics.PreferredBackBufferHeight = screenSizeHeight;
            graphics.IsFullScreen = true;
        }
        #endregion

        #region Methods
        public static void resetTripleTime(Object sender)
        {
            player.ShootMode = PlayerShootMode.basic;
            RemoveBigLaser();
            //if (bigLaserGauche != null)
            //{
            //    lasers.Remove(bigLaserDroit);
            //    lasers.Remove(bigLaserGauche);
            //}
        }
        //Function that moves the red lasers, depending the player movement
        public void bigLaserUpdatePos()
        {
            bigLaserGauche.Pos = new Vector2(player.getPlayerPos().X + 22, player.getPlayerPos().Y - 470);
            bigLaserDroit.Pos = new Vector2(player.getPlayerPos().X + 77, player.getPlayerPos().Y - 470);
        }
        public void spawnInvader()
        {
            Invader invader = new Invader(new Vector2(rnd.Next(-300, screenSizeWidth + 300), STARTING_POS_Y_ALIEN), invaderTexture, Laser.LaserType.invader, invadersLasers, INVADER_HP);
            InvaderLifeBar invaderLifeBar = new InvaderLifeBar(invader, lifeBarTexture);
            invader.Pv = INVADER_LIFE_BAR_WIDTH;
            invaderLifeBars.Add(invaderLifeBar);
            invaders.Add(invader);
        }

        public static void RemoveBigLaser()
        {
            if (player.IsShootingBigLaser)
            {
                lasers.RemoveAll(laser => laser.Type == Laser.LaserType.big);
                player.IsShootingBigLaser = false;
            }
        }
        #endregion

        #region Events
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (isMenuActive && isDead)
            {
                IsMouseVisible = true;
                spriteBatch.Draw(blackScreen, new Rectangle(new Point(0, 0), new Point(screenSizeWidth, screenSizeHeight)), Color.White);
                spriteBatch.DrawString(fontTitle, "   GAME OVER", new Vector2(328, screenSizeHeight / 10), Color.Red);

                spriteBatch.DrawString(scoreFont, "Score : " + score, new Vector2(670, screenSizeHeight / 4), Color.White);

                //Borne Menu Version                  
                spriteBatch.DrawString(font, "Press R to resart", new Vector2(screenSizeWidth / 3 - font.Texture.Width, 400), Color.White);
                spriteBatch.DrawString(font, "Press Q quit", new Vector2(screenSizeWidth / 2 + font.Texture.Width, 400), Color.White);
                spriteBatch.Draw(buttonsTextureDead, new Vector2(screenSizeWidth / 2 - buttonsTexture.Width / 2, 400), Color.White);
            }
            else
            {
                //Draws the background++
                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

                //Draws all the components
                foreach (Laser laser in lasers)
                {
                    if (laser != null)
                        laser.Draw(spriteBatch);

                }
                foreach (Boost boost in boosts)
                {
                    if (boost != null)
                        boost.Draw(spriteBatch);
                }

                foreach (InvaderLifeBar lifeBar in invaderLifeBars)
                {
                    if (lifeBar.getInvaderLife() > 0)
                    {
                        if (lifeBar != null)
                        {
                            spriteBatch.Draw(lifeBarUnderTexture, new Rectangle((int)lifeBar.getInvader().getPos().X - 26, (int)lifeBar.getInvader().getPos().Y - 18, INVADER_LIFE_BAR_WIDTH, 20), Color.White);
                            lifeBar.Draw(spriteBatch);
                        }
                    }
                }

                foreach (PlayerLifeBar lifeBar in playerLifeBars)
                {
                    if (lifeBar != null)
                    {
                        spriteBatch.Draw(lifeBarUnderTexture, new Vector2(lifeBar.getLifeBarPos().X - 30, lifeBar.getLifeBarPos().Y + 95), Color.White);
                        lifeBar.Draw(spriteBatch);
                    }
                }

                foreach (Invader invader in invaders)
                {
                    invader.Draw(spriteBatch);
                }
                foreach (Laser laser in invadersLasers)
                {
                    laser.Draw(spriteBatch);
                }

                foreach (Explosion explosion in explosions)
                {
                    explosion.Draw(spriteBatch);
                }

                //Draws the player
                player.Draw(spriteBatch);
                //Draws the score label
                spriteBatch.DrawString(scoreFont, "Score : " + score, new Vector2(10, 10), Color.White);

                if (isMenuActive == true)
                {
                    //Pc Menu Version
                    IsMouseVisible = true;

                    //Borne Menu Version                         
                    spriteBatch.DrawString(font, "Press R to resart", new Vector2(screenSizeWidth / 3 - font.Texture.Width, 400), Color.White);
                    spriteBatch.DrawString(font, "Press Q quit", new Vector2(screenSizeWidth / 2 + font.Texture.Width, 400), Color.White);
                    spriteBatch.Draw(buttonsTexture, new Vector2(screenSizeWidth / 2 - buttonsTexture.Width / 2, 400), Color.White);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected override void Initialize()
        {
            base.Initialize();
            player = new Player(100, playerSpeed, vesselTexture, startPos);
            playerLifeBar = new PlayerLifeBar(player, lifeBarTexture);
            PLAYER_LIFE_BAR_WIDTH = lifeBarUnderTexture.Width;
            INVADER_LIFE_BAR_WIDTH = lifeBarUnderTexture.Width / 3;
            player.Pv = PLAYER_LIFE_BAR_WIDTH;
            playerLifeBars.Add(playerLifeBar);
            oldState = Keyboard.GetState();
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Initialisation of all the textures in the game
            vesselTexture = Content.Load<Texture2D>("vessel");
            laserGreenTexture = Content.Load<Texture2D>("laser");
            laserRedTexture = Content.Load<Texture2D>("yellowLaser");
            background = Content.Load<Texture2D>("mainBackground");
            tripleShootBoostTexture = Content.Load<Texture2D>("TripleShootBoost");
            doubleLaserBoostTexture = Content.Load<Texture2D>("doubleLaserBoost");
            bigLaserTexture = Content.Load<Texture2D>("bigLaser");
            lifeBarTexture = Content.Load<Texture2D>("LifeBar");
            lifeBarUnderTexture = Content.Load<Texture2D>("LifeBarRed");
            invaderTexture = Content.Load<Texture2D>("Invader");
            invaderLaserTexture = Content.Load<Texture2D>("redLaser");
            blackScreen = Content.Load<Texture2D>("black");
            buttonsTexture = Content.Load<Texture2D>("buttons");
            buttonsTextureDead = Content.Load<Texture2D>("buttonsDead");
            explosionTexture = Content.Load<Texture2D>("spriteGif_explostion");

            //Fonts
            font = Content.Load<SpriteFont>("Arial");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");
            fontTitle = Content.Load<SpriteFont>("Title");
            //Tab with the diffrents shoot player boosts (1/4 possibility for the two red big lasers | 3/4 for the triple shoot mode)
            boostTab = new Texture2D[] { doubleLaserBoostTexture, tripleShootBoostTexture, tripleShootBoostTexture, tripleShootBoostTexture };
        }
        protected override void UnloadContent() { }
        protected override void Update(GameTime gameTime)
        {
            //Read the input of the keyboard
            var keyState = Keyboard.GetState();
            //If the user click Esc, pause the game and show the menu
            if (keyState.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Escape) || keyState.IsKeyDown(Keys.D6) && !oldState.IsKeyDown(Keys.D6))
            {
                if (escShowMenu)
                {
                    isMenuActive = true;
                    escShowMenu = false;
                    IsMouseVisible = true;
                }
                else
                {
                    IsMouseVisible = false;
                    isMenuActive = false;
                    escShowMenu = true;
                }
            }
            //If the player is dead, show the game over menu
            if (player.Pv <= 0)
            {
                player.ShootMode = PlayerShootMode.basic;
                IsMouseVisible = true;
                isDead = true;
                isMenuActive = true;
            }
            //Check when the menu is open, the different input (R -> restart game | Q -> quit)
            if (isMenuActive)
            {
                if (keyState.IsKeyDown(Keys.Q) || keyState.IsKeyDown(Keys.D7))
                {
                    Exit();
                }
                if (keyState.IsKeyDown(Keys.R) || keyState.IsKeyDown(Keys.F))
                {
                    IsMouseVisible = false;
                    isMenuActive = false;
                    player.Pv = PLAYER_LIFE_BAR_WIDTH;
                    player.Pos = startPos;
                    score = BASE_SCORE;
                    invaderLifeBars.Clear();
                    invaders.Clear();
                    invadersLasers.Clear();
                    lasers.Clear();
                    boosts.Clear();
                    player.ShootMode = PlayerShootMode.basic;
                    isDead = false;
                    escShowMenu = true;
                }
            }
            if (!isMenuActive)
            {
                //Add invaders in the invaders on the list with a random time between 40/200 ms
                if (countInvaders >= rnd.Next(40, 150))
                {
                    spawnInvader();
                    countInvaders = 0;
                }
                player.Update(gameTime, keyState);
                playerLifeBar.Update(gameTime);
                if (count == 10)
                {
                    if (keyState.IsKeyUp(Keys.Space))
                    {
                        if (player != null && player.getPlayerTexture() != null)
                        {
                            lasers.AddRange(player.Shoot());
                        }
                    }
                    else
                    {
                        RemoveBigLaser();
                        player.Speed = 16;
                    }
                    count = 0;
                }
                //Changes the fire mode of the player (deppendeing on the boost eaten(colider))
                for (int i = 0; i < boosts.Count; i++)
                {
                    if (boosts[i] != null && boosts[i].getTexture() != null)
                    {
                        if (boosts[i].BoxCollider.Intersects(player.BoxCollider) && boosts[i].getTexture() == tripleShootBoostTexture)
                        {
                            score += 5;
                            tripleTimeReset.Change(10000, Timeout.Infinite);
                            player.ShootMode = PlayerShootMode.triple;

                            RemoveBigLaser();
                            boosts.Remove(boosts[i]);
                        }
                        else if (boosts[i] != null && boosts[i].getTexture() != null)
                        {
                            if (boosts[i].BoxCollider.Intersects(player.BoxCollider) && boosts[i].getTexture() == doubleLaserBoostTexture)
                            {
                                tripleTimeReset.Change(5000, Timeout.Infinite);
                                score += 5;
                                player.ShootMode = PlayerShootMode.bigLaser;
                                boosts.Remove(boosts[i]);
                            }
                        }
                    }
                }
                //Varibales used like timers
                incrementGauche++;
                incrementDroit--;
                countInvaders++;
                //Add boost on the list of boosts
                if (randomBoostTime >= rnd.Next(300, 650))
                {
                    boosts.Add(new Boost(new Vector2(rnd.Next(0, screenSizeWidth - 10), -50), boostTab[rnd.Next(0, boostTab.Length)], rnd.Next(2, 5)));
                    randomBoostTime = 0;
                }
                //Removes lasers when they're no more on the screen
                for (int i = 0; i < lasers.Count; i++)
                {
                    if (lasers[i].Pos.Y < 0 - lasers[i].getLaserTexture().Height)
                        lasers.Remove(lasers[i]);
                }
                for (int i = 0; i < boosts.Count; i++)
                {
                    if (boosts[i] != null)
                        if (boosts[i].getPos().Y > screenSizeHeight)
                            boosts.Remove(boosts[i]);
                }
                if (bigLaserGauche != null)
                    bigLaserUpdatePos();

                //Functions that moves the items (boosts, lasers)
                foreach (Boost boost in boosts)
                {
                    if (boost != null)
                        boost.Update(gameTime);
                }
                foreach (Laser laser in lasers)
                {
                    if (laser != null)
                        laser.Update(gameTime);
                }
                foreach (Invader invader in invaders)
                {
                    if (invader != null)
                        invader.Update(gameTime);
                }
                foreach (Laser invaderLaser in invadersLasers)
                {
                    if (invaderLaser != null)
                        invaderLaser.Update(gameTime);
                }
                foreach (Explosion explosion in explosions)
                {
                    explosion.Update(gameTime);
                }

                //Check if an invader's laser hit the player, decrease his life for 1/4
                for (int i = 0; i < invadersLasers.Count; i++)
                {
                    if (invadersLasers[i].BoxCollider.Intersects(player.BoxCollider))
                    {
                        invadersLasers.Remove(invadersLasers[i]);
                        player.Pv -= lifeBarUnderTexture.Width / 6;
                    }
                }
                //If the invaders lasers are not on the screen anymore, they are removed from the list
                for (int i = 0; i < invadersLasers.Count; i++)
                {
                    if (invadersLasers.Contains(invadersLasers[i]))
                    {
                        if (invadersLasers[i].Pos.Y > screenSizeHeight)
                        {
                            invadersLasers.Remove(invadersLasers[i]);
                        }
                    }
                }
                for (int i = 0; i < invaders.Count; i++)
                {
                    if (invaders[i].getPos().Y > screenSizeHeight)
                    {
                        if (invaders.Contains(invaders[i]))
                        {
                            player.Pv -= lifeBarUnderTexture.Width / 6;
                            invaders[i].Pv = 0;
                            invaders.Remove(invaders[i]);
                        }
                    }
                }
                //Check if a laser hits an Invader and decrese his life. (remove them from the list, if he has no more life)
                for (int i = 0; i < lasers.Count; i++)
                {
                    for (int y = 0; y < invaders.Count; y++)
                    {
                        if (i < lasers.Count && y < invaders.Count)
                        {
                            if (lasers.Contains(lasers[i]) && invaders.Contains(invaders[y]))
                            {
                                if (lasers[i].BoxCollider.Intersects(invaders[y].boxCollider))
                                {
                                    if (lasers[i].getLaserTexture() != bigLaserTexture)
                                    {
                                        lasers.Remove(lasers[i]);
                                    }
                                    else
                                    {
                                        invaders[y].Pv = 0;
                                    }
                                    invaders[y].Pv -= INVADER_LIFE_BAR_WIDTH / 10;
                                    if (invaders[y].Pv <= 0)
                                    {
                                        if (invaders.Contains(invaders[y]))
                                        {
                                            score += 10;

                                            Explosion explosion = new Explosion();
                                            Animation animation = new Animation();
                                            animation.Initialize(explosionTexture, new Vector2(invaders[y].getPos().X + explosionTexture.Width / 14 / 2, invaders[y].getPos().Y + explosionTexture.Height / 2), explosionTexture.Width / 14, explosionTexture.Height, 0, 14, 20, Color.White, 1, false);

                                            explosion.Initialize(animation, new Vector2(invaders[y].getPos().X + explosionTexture.Width / 14 / 2, invaders[y].getPos().Y + explosionTexture.Height / 2));

                                            explosions.Add(explosion);

                                            invaders.Remove(invaders[y]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //Variables used for gametime delay
                randomBoostTime++;
                count++;
                shootInvaderLaser++;
                base.Update(gameTime);
            }
            oldState = keyState;

            foreach (InvaderLifeBar lifeBar in invaderLifeBars)
            {
                if (lifeBar != null)
                {
                    lifeBar.Update(gameTime);
                }
            }
        }
        private void RestartButton_Click(object sender, EventArgs e)
        {
            IsMouseVisible = false;
            isMenuActive = false;
            player.Pv = PLAYER_LIFE_BAR_WIDTH;
            player.Pos = startPos;
            score = 0;
            invaders.Clear();
            invadersLasers.Clear();
            lasers.Clear();
            boosts.Clear();
            player.ShootMode = PlayerShootMode.basic;
            isDead = false;
        }
        private void QuitButton_Click(object sender, EventArgs e)
        {
            Exit();
        }
        #endregion
    }
}
