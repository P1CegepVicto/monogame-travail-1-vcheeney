using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;

namespace Project2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public float state = 1;

        public int nbEnemyMax = 5;
        public int nbEnemyRestant = 5;
        public int nbEnemy = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle fenetre;

        GameObject background;

        GameObject hero;
        GameObject bullethero;

        GameObject[] enemy;
        GameObject[] bulletenemy;

        Song song;

        SpriteFont font;

        Random de = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //On met le jeu en fullscreen
            this.graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            this.graphics.ToggleFullScreen();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            //Définition de la taille de la fenêtre
            fenetre = graphics.GraphicsDevice.Viewport.Bounds;
            fenetre.Width = graphics.GraphicsDevice.DisplayMode.Width;
            fenetre.Height = graphics.GraphicsDevice.DisplayMode.Height;

            background = new GameObject(0, 0, 0, 0, true);
            background.sprite = Content.Load<Texture2D>("background0.png");

            //Hero
            hero = new GameObject(500, 500, 75, 75, true, 100);
            hero.sprite = Content.Load<Texture2D>("playerShip1_blue.png");
            hero.vitesse = 10;

            //Bullet Hero
            bullethero = new GameObject(fenetre.Width + 2000, fenetre.Height + 2000, 0, 0, false);
            bullethero.sprite = Content.Load<Texture2D>("laserBlue13.png");
            bullethero.vitesse = 30;

            //Enemy
            enemy = new GameObject[nbEnemyMax];
            for (int i = 0; i< enemy.Length; i++)
            {
                enemy[i] = new GameObject(fenetre.Width, 200, 10,10 ,false, 100);
                enemy[i].sprite = Content.Load<Texture2D>("enemyBlack3.png");

                enemy[i].direction.X = -de.Next(3, 7);
                enemy[i].direction.Y = de.Next(-2, 2);
            }

            //Bullet Enemy
            bulletenemy = new GameObject[nbEnemyMax];
            for (int i = 0; i< bulletenemy.Length;i++)
            {
                bulletenemy[i] = new GameObject(fenetre.Width + 2000, fenetre.Height + 2000, 10, 10, false);
                bulletenemy[i].sprite = Content.Load<Texture2D>("laserRed15.png");
                bulletenemy[i].vitesse = 30;

                bulletenemy[i].direction.Y = bulletenemy[i].vitesse;
                bulletenemy[i].direction.X = 0;
            }

            //Musique de background
            song = Content.Load<Song>("Sounds\\Hyperfun");
            MediaPlayer.Play(song);

            //Font
            font = Content.Load<SpriteFont>("Font");


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (state == 1)
            {
            UpdateHero();
            UpdateEnemy(gameTime);
            UpdateCollision();
            }




            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Wheat);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            spriteBatch.Draw(background.sprite, background.position, Color.White);

            spriteBatch.Draw(bullethero.sprite, bullethero.position);
            spriteBatch.Draw(hero.sprite, hero.position, Color.White);


            for (int i = 0; i < enemy.Length; i++)
            {
                spriteBatch.Draw(bulletenemy[i].sprite, bulletenemy[i].position, Color.White);
                spriteBatch.Draw(enemy[i].sprite, enemy[i].position, Color.White);
            }

            spriteBatch.DrawString(font, ("Temps ecoule: "+gameTime.TotalGameTime.Minutes.ToString()+" : "+gameTime.TotalGameTime.Seconds.ToString() + " : "+gameTime.TotalGameTime.Milliseconds.ToString()), new Vector2(10, 0), Color.Black);
            spriteBatch.DrawString(font, "Nombre d'ennemis restant: "+nbEnemyRestant.ToString(), new Vector2(10, 40), Color.Black);
            spriteBatch.DrawString(font, "Hero health: " + hero.health.ToString(), new Vector2(10, 1000), Color.Black);


            if (hero.health <= 0)
            {
                spriteBatch.DrawString(font, "YOU LOSE!", new Vector2(1000, 500), Color.Black);
            }
            if (nbEnemyRestant <= 0)
            {
                spriteBatch.DrawString(font, "YOU WIN!", new Vector2(1000, 500), Color.Black);
            }




            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void UpdateHero()
        {
            int shoothero = 0;

            if (hero.health <= 0)
            {
                hero.estVivant = false;
                hero.position.X = fenetre.Width + 2000;
                hero.position.Y = fenetre.Height + 2000;
            }

            if (hero.estVivant == true)
            {
                //Déclaration des variable hero.direction si une touche n'est pas appuyée; (Sans ça la commande finale ne fonctionne pas)
                hero.direction.X = 0;
                hero.direction.Y = 0;

                //Détection des touches de mouvement/autre
                if (Keyboard.GetState().IsKeyDown(Keys.D) && hero.position.X < fenetre.Width - hero.sprite.Width)
                {
                    hero.direction.X = hero.vitesse;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A) && hero.position.X > 0)
                {
                    hero.direction.X = -hero.vitesse;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W) && hero.position.Y > 600)
                {
                    hero.direction.Y = -hero.vitesse;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S) && hero.position.Y < fenetre.Height - hero.sprite.Height)
                {
                    hero.direction.Y = hero.vitesse;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && bullethero.estVivant == false)
                {
                    bullethero.estVivant = true;
                    shoothero = 1;
                }

                //Empêche le hero de sortir de la zone de jeu:
                #region
                if (hero.position.X > fenetre.Width - hero.sprite.Width)
                {
                    hero.position.X = fenetre.Width - hero.sprite.Width;
                }
                if (hero.position.X < 0)
                {
                    hero.position.X = 0;
                }
                if (hero.position.Y > fenetre.Height - hero.sprite.Height)
                {
                    hero.position.Y = fenetre.Height - hero.sprite.Height;
                }
                if (hero.position.Y < 600)
                {
                    hero.position.Y = 600;
                }
                #endregion

                
                //Commande finale
                //Ça envois les changements de direction selon les touches appuyées. (Exemple: (2,-2) = -> + î)
                hero.position += hero.direction;

            }
            //UpdateBulletHero
            if (bullethero.estVivant == false)
            {
                bullethero.position.X = fenetre.Width + 2000;
                bullethero.position.Y = fenetre.Height + 2000;
            }

            if (bullethero.estVivant == true)
            {
                if (shoothero == 1)
                {
                    bullethero.direction.X = 0;
                    bullethero.direction.Y = -bullethero.vitesse;
                    bullethero.position.X = hero.position.X + hero.sprite.Width / 2 - bullethero.sprite.Width / 2;
                    bullethero.position.Y = hero.position.Y + hero.sprite.Height / 2 + 15;
                    shoothero = 0;
                }

                bullethero.position += bullethero.direction;


                if (bullethero.position.Y < 0 - bullethero.sprite.Height)
                {
                    bullethero.estVivant = false;
                }

            }

        }


        public void UpdateEnemy(GameTime gameTime)
        {
            //New enemy will spawn every 5 seconds if the amount of enemies (that have spawned) is lower than the maximum amount of enemies;
            if (nbEnemy * 2 < gameTime.TotalGameTime.Seconds && nbEnemy<nbEnemyMax)
            {
                enemy[nbEnemy].estVivant = true;
                nbEnemy++;
            }
            
            for (int i = 0; i< enemy.Length; i++)
            {
                //We kill the enemy if his health is equal or lower to 0;
                if (enemy[i].health <= 0 && enemy[i].estVivant == true)
                {
                    enemy[i].estVivant = false;
                    enemy[i].position.X = fenetre.Width;
                    nbEnemyRestant--;
                }

                //The enemy moves if he is alive;
                if (enemy[i].estVivant == true)
                {
                    enemy[i].position += enemy[i].direction;
                }


                //We "reorient" the enemy is he leaves the game space;
                if (enemy[i].position.Y<0 || enemy[i].position.Y > 500)
                {
                    enemy[i].direction.Y = -enemy[i].direction.Y;
                }

                if (enemy[i].position.X < 0 - enemy[i].sprite.Width)
                {
                    enemy[i].position.X = fenetre.Width;
                }

                //The enemy shoots his bullet
                if (enemy[i].estVivant == true && bulletenemy[i].estVivant == false)
                {
                    bulletenemy[i].estVivant = true;
                    bulletenemy[i].position.X = enemy[i].position.X + enemy[i].sprite.Width / 2 - bulletenemy[i].sprite.Width / 2;
                    bulletenemy[i].position.Y = enemy[i].position.Y + 50;
                }

                if (bulletenemy[i].estVivant == true)
                {
                    bulletenemy[i].position += bulletenemy[i].direction;
                }


                if (bulletenemy[i].position.Y > fenetre.Height)
                {
                    bulletenemy[i].estVivant = false;
                }


            }




        }
        
        public void UpdateCollision()
        {

            for (int i = 0; i< nbEnemy; i++)
            {
                if (hero.hitbox(0).Intersects(bulletenemy[i].hitbox(0)) && bulletenemy[i].estVivant == true)
                {
                    hero.health -= 25;
                    bulletenemy[i].estVivant = false;
                    bulletenemy[i].position.X = fenetre.Width + 2000;
                    bulletenemy[i].position.Y = fenetre.Height + 2000;

                }

                if (enemy[i].hitbox(0).Intersects(bullethero.hitbox(0)) && bullethero.estVivant == true)
                {
                    enemy[i].health -= 100;
                    bullethero.estVivant = false;
                }

            }



        }


    }
}
