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
namespace SpaceInvader
{
    public class Game1 : Game
    {
        //Initialisation
        Texture2D background;
        Texture2D laserGreenTexture;
        Texture2D laserRedTexture;
        Texture2D doubleLaserBoostTexture;
        Texture2D bigLaserTexture;
        Texture2D vesselTexture;
        Texture2D tripleShootBoostTexture;
        Texture2D lifeBarTexture;
        Texture2D lifeBarUnderTexture;
        Texture2D invaderTexture;
        Texture2D invaderLaserTexture;
        Texture2D blackScreen;
        Texture2D buttonsTextureDead;
        Texture2D explosionTexture;

        Texture2D[] boostTab;
        static Laser bigLaserGauche;
        static Laser bigLaserDroit;
        PlayerLifeBar playerLifeBar;
        Player player;       
        Vector2 START_POS = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2);
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

        //Definition
        Random rnd = new Random();
        int screenSizeWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int screenSizeHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        int score = 0;
        int playerSpeed = 10;
        int count = 0;
        int randomBoostTime = 0;
        int incrementGauche = 0;
        int incrementDroit = 0;
        static int initializeBigLasers = 0;
        int countInvaders = 0;
        int shootInvaderLaser = 0;
        bool isDead = false;
        bool menu = true;
        bool escShowMenu = false;
        KeyboardState oldState;       
        static string playerShootMode = "base";
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int PLAYER_LIFE_BAR_WIDTH;
        int INVADER_LIFE_BAR_WIDTH;

        public static void resetTripleTime(Object sender)
        {
            playerShootMode = "base";
            initializeBigLasers = 0;
            if (bigLaserGauche != null)
            {
                lasers.Remove(bigLaserDroit);
                lasers.Remove(bigLaserGauche);
        }
            }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //Fullscreen mode
            graphics.PreferredBackBufferWidth = screenSizeWidth;
            graphics.PreferredBackBufferHeight = screenSizeHeight;
            graphics.IsFullScreen = true;
        }        
        protected override void Initialize()
        {
            base.Initialize();
            player = new Player(100, playerSpeed, vesselTexture, START_POS, this);
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
            buttonsTextureDead = Content.Load<Texture2D>("buttons");
            explosionTexture = Content.Load<Texture2D>("spriteGif_explostion");
            //Fonts
            font = Content.Load<SpriteFont>("Arial");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");
            fontTitle = Content.Load<SpriteFont>("Title");
            //Tab with the diffrents shoot player boosts (1/4 possibility for the two red big lasers | 3/4 for the triple shoot mode)
            boostTab = new Texture2D[] {doubleLaserBoostTexture , tripleShootBoostTexture, tripleShootBoostTexture, tripleShootBoostTexture };
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
                    menu = true;
                    escShowMenu = false;
                    IsMouseVisible = true;
                }
                else
                {
                    IsMouseVisible = false;
                    menu = false;
                    escShowMenu = true;
                }
            }
            //If the player is dead, show the game over menu
            if (player.Pv <= 0)
            {
                playerShootMode = "base";
                IsMouseVisible = true;
                isDead = true;
                menu = true;
            }
            //Check when the menu is open, the different input (R -> restart game | Q -> quit)
            if (menu)
            {
                if (keyState.IsKeyDown(Keys.Q) ||keyState.IsKeyDown(Keys.D7))
                {
                    Exit();
                }
                //Since there's no method to restart the game, all the variables are set to there base statement
                if (keyState.IsKeyDown(Keys.R) || keyState.IsKeyDown(Keys.F))
                {
                    IsMouseVisible = false;
                    menu = false;
                    player.Pv = PLAYER_LIFE_BAR_WIDTH;
                    player.Pos = START_POS;
                    score = 0;
                    invaderLifeBars.Clear();
                    invaders.Clear();
                    invadersLasers.Clear();
                    lasers.Clear();
                    boosts.Clear();
                    playerShootMode = "base";
                    explosions.Clear();
                    isDead = false;
                    escShowMenu = true;
                }
            }            
            if (!menu)
            {
                //Add invaders in the invaders on the list with a random time between 40/200 ms
                if (countInvaders >= rnd.Next(40, 150))
                {
                    Invader invader = new Invader(new Vector2(rnd.Next(-300, screenSizeWidth + 300), -200), invaderTexture, invaderLaserTexture, invadersLasers, 15);
                    InvaderLifeBar invaderLifeBar = new InvaderLifeBar(invader, lifeBarTexture);
                    invader.Pv = INVADER_LIFE_BAR_WIDTH;
                    invaderLifeBars.Add(invaderLifeBar);
                    invaders.Add(invader);
                    countInvaders = 0;
                }
                player.Update(gameTime, keyState);
                playerLifeBar.Update(gameTime);
                if (count == 10)
                {
                    if (keyState.IsKeyDown(Keys.Space) || keyState.IsKeyDown(Keys.G))
                    {
                        //If the space bar is "cliked", the player stop shooting lasers but he increases his move speed
                        if (bigLaserGauche != null)
                        {
                            lasers.Remove(bigLaserGauche);
                            lasers.Remove(bigLaserDroit);
                            initializeBigLasers = 0;
                        }
                        player.Speed = 16;
                    }
                    else
                    {
                    player.Speed = 10;
                        //Switch that deppending on the current fire mode of the player, add the lasers on the list
                        switch (playerShootMode)
                        {
                            case "base":
                                if (player != null && player.getPlayerTexture() != null)
                                {
                                    lasers.Add(new Laser(laserGreenTexture, new Vector2(player.getPlayerPos().X + player.getPlayerTexture().Width / 2 - 8, player.getPlayerPos().Y), 0, 0, 10));
                                    lasers.Add(new Laser(laserGreenTexture, new Vector2(player.getPlayerPos().X + player.getPlayerTexture().Width / 2 + 8, player.getPlayerPos().Y), 0, 0, 10));
                                }
                                break;
                            case "triple":
                                if (player != null && player.getPlayerTexture() != null)
                                {
                                    lasers.Add(new Laser(laserRedTexture, new Vector2(player.getPlayerPos().X + player.getPlayerTexture().Width / 2 - laserRedTexture.Width + 5, player.getPlayerPos().Y), 210, 5, 10));
                                    lasers.Add(new Laser(laserRedTexture, new Vector2(player.getPlayerPos().X + player.getPlayerTexture().Width / 2 - laserRedTexture.Width, player.getPlayerPos().Y), 210, 5, 10));
                                    lasers.Add(new Laser(laserGreenTexture, new Vector2(player.getPlayerPos().X + player.getPlayerTexture().Width / 2 - 5, player.getPlayerPos().Y), 0, 0, 10));
                                    lasers.Add(new Laser(laserGreenTexture, new Vector2(player.getPlayerPos().X + player.getPlayerTexture().Width / 2 + 5, player.getPlayerPos().Y), 0, 0, 10));
                                    lasers.Add(new Laser(laserRedTexture, new Vector2(player.getPlayerPos().X + player.getPlayerTexture().Width / 2 - laserRedTexture.Width + 30, player.getPlayerPos().Y), -210, -5, 10));
                                    lasers.Add(new Laser(laserRedTexture, new Vector2(player.getPlayerPos().X + player.getPlayerTexture().Width / 2 - laserRedTexture.Width + 35, player.getPlayerPos().Y), -210, -5, 10));
                                }
                                break;
                            case "doubleLasers":
                                if (player != null && player.getPlayerTexture() != null)
                                {
                                    if (initializeBigLasers == 0)
                                    {
                                        bigLaserGauche = new Laser(bigLaserTexture, new Vector2(player.getPlayerPos().X + 22, player.getPlayerPos().Y - 700), 0, 0, 0, true);
                                        bigLaserDroit = new Laser(bigLaserTexture, new Vector2(player.getPlayerPos().X + 75, player.getPlayerPos().Y - 700), 0, 0, 0, true);
                                        lasers.Add(bigLaserGauche);
                                        lasers.Add(bigLaserDroit);
                                        initializeBigLasers = 1;
                                    }
                                }
                                break;
                        }
                            
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
                            playerShootMode = "triple";
                            if (bigLaserGauche != null && bigLaserDroit != null)
                            {
                                lasers.Remove(bigLaserGauche);
                                lasers.Remove(bigLaserDroit);
                                initializeBigLasers = 0;
                            }
                            boosts.Remove(boosts[i]);
                        }
                        else if (boosts[i] != null && boosts[i].getTexture() != null)
                        {                            
                            if (boosts[i].BoxCollider.Intersects(player.BoxCollider) && boosts[i].getTexture() == doubleLaserBoostTexture)
                            {
                                tripleTimeReset.Change(5000, Timeout.Infinite);
                                score += 5;
                                playerShootMode = "doubleLasers";
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
                if (randomBoostTime >= rnd.Next(600,2000))
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

                //Functions that moves the items (boosts, lasers, invaders, lifebars, explosions)
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
                foreach (InvaderLifeBar lifeBar in invaderLifeBars)
                {
                    if (lifeBar != null)
                    {
                        lifeBar.Update(gameTime);
                    }
                }
                //Check if an invader's laser hit the player, decrease his life for 1/6 of the life bar width
                for (int i = 0; i < invadersLasers.Count; i++)
                {
                    if (invadersLasers[i].BoxCollider.Intersects(player.BoxCollider))
                    {
                        invadersLasers.Remove(invadersLasers[i]);
                        player.Pv -= PLAYER_LIFE_BAR_WIDTH / 6;
                    }
                }
                //Check if an invader hit the player, decrease the player's life for 33% and kill the invader 
                for (int i = 0; i < invaders.Count; i++)
                {
                    if (invaders[i].boxCollider.Intersects(player.BoxCollider))
                    {
                        invaders[i].Pv = 0;
                        playAndRemoveInvadersDeadAnimation(invaders[i]);
                        
                        player.Pv -= 2 * (PLAYER_LIFE_BAR_WIDTH / 6); 
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
                //If the invader is no more invisible but still alive, decrease the player's life for 1/6 and kills the invader
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
                                            playAndRemoveInvadersDeadAnimation(invaders[y]);
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
        }
        //Functions that shows the animations and remove them when they end
        private void playAndRemoveInvadersDeadAnimation(Invader invader)
        {
            Explosion explosion = new Explosion();
            Animation animation = new Animation();
            animation.Initialize(explosionTexture, new Vector2(invader.getPos().X + explosionTexture.Width / 14 / 2, invader.getPos().Y + explosionTexture.Height / 2), explosionTexture.Width / 14, explosionTexture.Height, 0, 14, 20, Color.White, 1, false);
            explosion.Initialize(animation, new Vector2(invader.getPos().X + explosionTexture.Width / 14 / 2, invader.getPos().Y + explosionTexture.Height / 2));
            explosions.Add(explosion);

            invaders.Remove(invader);
        }
        
        protected override void Draw(GameTime gameTime)
        {            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (menu && isDead)
            {
                IsMouseVisible = true;
                spriteBatch.Draw(blackScreen, new Rectangle(new Point(0, 0), new Point(screenSizeWidth, screenSizeHeight)), Color.White);
                spriteBatch.DrawString(fontTitle, "  GAME OVER", new Vector2(screenSizeWidth / 2 - fontTitle.Texture.Width / 2, screenSizeHeight / 10), Color.Red);
                
                spriteBatch.DrawString(scoreFont, "Score : " + score, new Vector2(670, screenSizeHeight / 4), Color.White);

                //Borne Menu Version
                spriteBatch.Draw(buttonsTextureDead, new Vector2(screenSizeWidth / 2 - buttonsTextureDead.Width / 2, 450), Color.White);
            }
            else
            {
                //Draws the background
                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

                //Draws all the components (lasers, boosts, lifebars, invaders, explosions)
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

                if (menu == true)
                {
                    //Pc Menu Version
                    IsMouseVisible = true;
                    
                    //Borne Menu Version
                    spriteBatch.Draw(buttonsTextureDead, new Vector2(screenSizeWidth  / 2 - buttonsTextureDead.Width  / 2, 250), Color.White);                 
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        //Function that moves the red lasers, depending the player movement
        public void bigLaserUpdatePos()
        {
            bigLaserGauche.Pos = new Vector2(player.getPlayerPos().X + 22, player.getPlayerPos().Y - 470);
            bigLaserDroit.Pos = new Vector2(player.getPlayerPos().X + 77, player.getPlayerPos().Y - 470);
        }       
    }
}
