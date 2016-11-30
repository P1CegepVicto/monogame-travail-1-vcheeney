using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;

namespace Projet1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Song song;

        SoundEffect son;
        SoundEffectInstance pop;

        SoundEffect son2;
        SoundEffectInstance boum;

        SpriteFont font;

        int tireenemy = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle fenetre;

        Hero hero;
        BulletHero[] bullethero = new BulletHero[10]; //Tableau des bullets du héro (bh = bullets héros)

        GameObject viseur;
        Vector2 mouseCoordinates;

        Enemy enemy;
        GameObject bulletenemy;

        Layers background;
        Layers overlay;
        Layers dust;

        double distanceHE;      //Distance Hero/Enemy
        double distanceHEx;     //Distance Hero/Enemy en X
        double distanceHEy;     //Distance Hero/Enemy en Y

        double distanceHV;      //Distance Hero/Viseur
        double distanceHVx;     //Distance Hero/Viseur en X
        double distanceHVy;     //Distance Hero/Viseur en Y


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
            font = Content.Load<SpriteFont>("Font");

            #region Fenetre/BackGround/DustLayers ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Définition de la taille de la fenêtre
            fenetre = graphics.GraphicsDevice.Viewport.Bounds;
            fenetre.Width = graphics.GraphicsDevice.DisplayMode.Width;
            fenetre.Height = graphics.GraphicsDevice.DisplayMode.Height;

            background = new Layers(0,0);
            background.sprite1 = Content.Load<Texture2D>("Static layers\\BG.png");

            overlay = new Layers(0, 0);
            overlay.sprite1 = Content.Load<Texture2D>("Static layers\\overlay.png");

            dust = new Layers(0, 0, 500, 0);
            dust.sprite1 = Content.Load<Texture2D>("Static layers\\dustvague.png");
            dust.sprite2 = Content.Load<Texture2D>("Static layers\\dustvague.png");
            dust.position2.X = dust.sprite1.Width;
            dust.vitesse = 1;
            #endregion

            #region LoadHero ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Hero
            hero = new Hero(100, 100, 75, 75, true, 100, 0,0,0); // C'est le condensé de tout mes propriété de départ de mon héro.(positionX,positionY,origineX,origineY,estVivant, health)
            hero.sprite = Content.Load<Texture2D>("Tanks\\TANK1.png");
            hero.spritecanon = Content.Load<Texture2D>("Tanks\\TANK1canon.png");
            hero.originecanon.X = hero.spritecanon.Width / 2;
            hero.originecanon.Y = hero.spritecanon.Height / 2;


            hero.vitesse = 5;
            hero.health = 100;

            for (int i = 0; i < bullethero.Length; i++)
            {
                bullethero[i] = new BulletHero((int)hero.position.X, (int)hero.position.Y, 10, 10, false); //ici on a pas le Health, il est facultatif.
                bullethero[i].sprite = Content.Load<Texture2D>("Bullets\\BULLET1B.png");
                bullethero[i].origine.X = bullethero[i].sprite.Width / 2;
                bullethero[i].origine.Y = bullethero[i].sprite.Height / 2;
                bullethero[i].vitesse = 10;
            }

            hero.spritehealthbar = Content.Load<Texture2D>("Objects\\Health spritesheet.png");
            hero.afficherhealthbar = new Rectangle(0, 0, 150, 25);
            #endregion ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            #region LoadEnemy ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Enemy
            enemy = new Enemy(1400, 400, 75, 75, true, 100, 0,0,0);
            enemy.sprite = Content.Load<Texture2D>("Tanks\\TANKENEMY.png");
            enemy.spritecanon = Content.Load<Texture2D>("Tanks\\TANKENEMYcanon.png");
            enemy.originecanon.X = enemy.spritecanon.Width / 2;
            enemy.originecanon.Y = enemy.spritecanon.Height / 2;

            enemy.vitesse = 1;
            enemy.health = 100;


            bulletenemy = new GameObject((int)enemy.position.X, (int)enemy.position.Y, 10, 10, false);
            bulletenemy.sprite = Content.Load<Texture2D>("Bullets\\BULLETENEMYB.png");
            bulletenemy.origine.X = bulletenemy.sprite.Width / 2;
            bulletenemy.origine.Y = bulletenemy.sprite.Height / 2;

            enemy.spritehealthbar = Content.Load<Texture2D>("Objects\\Health spritesheet.png");
            enemy.afficherhealthbar = new Rectangle(0, 0, 150, 25);

            #endregion///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Viseur
            viseur = new GameObject(0, 0, 0, 0, true);
            viseur.sprite = this.Content.Load<Texture2D>("viseur.png");

            //Musique d'ambiance
            song = Content.Load<Song>("Sounds\\music");
            MediaPlayer.Play(song);

            //Sounds effects
            son = Content.Load<SoundEffect>("Sounds\\pop");
            pop = son.CreateInstance();

            son2 = Content.Load<SoundEffect>("Sounds\\boum");
            boum = son2.CreateInstance();

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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Liste des actions qui updates
            UpdateViseur();

            UpdateHero();
            UpdateBulletHero();

            UpdateEnemy();
            UpdateBulletEnemy();

            UpdateColisions();

            UpdateDustLayers();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background.sprite1, background.position1, Color.White);

            //Afficher les bullets
            for (int i = 0; i < bullethero.Length; i++)
            {
                spriteBatch.Draw(bullethero[i].sprite, bullethero[i].position, null, Color.White, bullethero[i].angle, bullethero[i].origine, 1f, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(bulletenemy.sprite, bulletenemy.position, null, Color.White, bulletenemy.angle, bulletenemy.origine, 1f, SpriteEffects.None, 0f);

            //Afficher les personnages
            spriteBatch.Draw(enemy.sprite, enemy.position);
            spriteBatch.Draw(enemy.spritecanon, enemy.positioncanon, null, Color.White, enemy.angle, enemy.originecanon, 1.0f, SpriteEffects.None, 0f);
            spriteBatch.Draw(hero.sprite, hero.position, Color.White);
            spriteBatch.Draw(hero.spritecanon, hero.positioncanon, null, Color.White, hero.angle, hero.originecanon, 1.0f, SpriteEffects.None, 0f);

            //Afficher la barre de vie du personnage
            spriteBatch.Draw(hero.spritehealthbar, hero.positionhealthbar, hero.afficherhealthbar, Color.White);

            //Afficher la barre de vie de l'ennemi
            spriteBatch.Draw(enemy.spritehealthbar, enemy.positionhealthbar, enemy.afficherhealthbar, Color.White);

            //Afficher le viseur.
            this.spriteBatch.Draw(this.viseur.sprite, this.mouseCoordinates, Color.White);


            //Afficher la poussière et l'overlay
            spriteBatch.Draw(dust.sprite1, dust.position1);
            spriteBatch.Draw(dust.sprite2, dust.position2, effects: SpriteEffects.FlipHorizontally);

            spriteBatch.Draw(overlay.sprite1, overlay.position1, Color.White);


            //Mode développement:
            spriteBatch.DrawString(font, hero.shootdelay.ToString(), hero.position, Color.Black);


            spriteBatch.End();

            base.Draw(gameTime);
        }




        public void UpdateViseur()
        {
            var mouseState = Mouse.GetState();
            this.mouseCoordinates = new Vector2(mouseState.X, mouseState.Y);

            viseur.position.X = this.mouseCoordinates.X;
            viseur.position.Y = this.mouseCoordinates.Y;
        }

        public void UpdateHero() ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////UPDATE HERO
        {
            //Statut de l'hero (On regarde si le héro a encore des points de vie. Si non, on "kill" le héro et ses bullets (POUR L'INSTANT) et on sort le hero & bullets de la zone de jeu.
            if (hero.health <= 0)
            {
                hero.estVivant = false;
                //bullethero.estVivant = false; À vérifier! XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                hero.position.X = fenetre.Width;
                hero.position.Y = fenetre.Height;

                //bullethero.position.X = fenetre.WidthÀ vérifier! XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                //bullethero.position.Y = fenetre.Height;À vérifier! XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            }

            if (hero.estVivant == true)
            {

                //Déclaration des variable hero.direction si une touche n'est pas appuyée; (Sans ça la commande finale ne fonctionne pas)
                hero.direction.X = 0;
                hero.direction.Y = 0;

                //Détection des touches de mouvement/autre
                if (Keyboard.GetState().IsKeyDown(Keys.D) && hero.position.X < fenetre.Width - 150)
                {
                    hero.direction.X = hero.vitesse;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A) && hero.position.X > 0)
                {
                    hero.direction.X = -hero.vitesse;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W) && hero.position.Y > 0)
                {
                    hero.direction.Y = -hero.vitesse;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S) && hero.position.Y < fenetre.Height - 150)
                {
                    hero.direction.Y = hero.vitesse;
                }

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && hero.shootdelay == 0)
                    {
                        hero.test = 1;
                    }
                    if (Mouse.GetState().LeftButton == ButtonState.Released && hero.test == 1)
                    {
                        hero.nbbullet = 1;
                        hero.test = 0;
                        hero.shootdelay = hero.delay;
                    }

                //Le timer diminue petit à petit
                if (hero.shootdelay>0)
                hero.shootdelay--;

                #region//Empêche le hero de sortir de la zone de jeu:
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
                if (hero.position.Y < 0)
                {
                    hero.position.Y = 0;
                }
                #endregion

                //Commande finale
                //Ça envois les changements de direction selon les touches appuyées. (Exemple: (2,-2) = -> + î)
                hero.position += hero.direction;
            }

            //Position et angle du canon hero 
            hero.positioncanon.X = hero.position.X + (hero.sprite.Width / 2);
            hero.positioncanon.Y = hero.position.Y + (hero.sprite.Height / 2);

            float angle = (float)Math.Atan2((hero.position.Y + hero.sprite.Height / 2) - (viseur.position.Y + viseur.sprite.Height / 2), (hero.position.X + hero.sprite.Width / 2) - (viseur.position.X + viseur.sprite.Width / 2));
            Debug.WriteLine(MathHelper.ToDegrees(angle).ToString());

            hero.angle = (float)((angle) - 1.5707963267948966192313216916398);

            //Update de la position de la barre de vie:
            hero.positionhealthbar.X = hero.position.X;
            hero.positionhealthbar.Y = hero.position.Y - 20;

        }

        public void UpdateBulletHero() //////////////////////////////////////////////Bullet hero;
        {


            for (int i = 0; i < bullethero.Length; i++)
            {
                if (bullethero[i].estVivant == false)
                {
                    bullethero[i].position.X = fenetre.Width + 200;
                    bullethero[i].position.Y = fenetre.Height + 200;
                }

                if (hero.estVivant == true) //IF !!!HERO!!! EST VIVANT;
                {
                    if (hero.nbbullet == 1)
                    {
                        bullethero[i].estVivant = true;
                        bullethero[i].position.X = hero.position.X + ((hero.sprite.Width / 2));
                        bullethero[i].position.Y = hero.position.Y + ((hero.sprite.Height / 2));

                        double grandissement1;

                        distanceHVx = bullethero[i].position.X - viseur.position.X - ((viseur.sprite.Width) / 2);
                        distanceHVy = bullethero[i].position.Y - viseur.position.Y - ((viseur.sprite.Height) / 2);
                        distanceHV = Math.Sqrt(Math.Pow(distanceHVx, 2) + Math.Pow(distanceHVy, 2));

                        grandissement1 = (distanceHV / bullethero[i].vitesse);

                        bullethero[i].direction.X = -(float)(distanceHVx / grandissement1);
                        bullethero[i].direction.Y = -(float)(distanceHVy / grandissement1);

                        hero.nbbullet = 0;
                        pop.Play();
                    }

                    bullethero[i].position += bullethero[i].direction;
                    bullethero[i].angle -= (float)(Math.PI / 15);
                }

                //bullethero.angle +=(float)(Math.PI/30);

                //On kill le "bullet" si il sort de la zone de jeu.
                if (bullethero[i].position.Y > fenetre.Height || bullethero[i].position.Y < 0 || bullethero[i].position.X > fenetre.Width || bullethero[i].position.X < 0)
                {
                    bullethero[i].estVivant = false;
                }
            }
        }

        public void UpdateEnemy() ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////UPDATE ENEMY
        {
            //Statut de l'ennemi: On vérifie si l'ennemi est encore vivant, sinon, on le "kill" et le sort de la zone de jeu avec ses bullets (POUR L'INSTANT)
            if (enemy.health <= 0)
            {
                enemy.estVivant = false;
                bulletenemy.estVivant = false;

                enemy.position.X = fenetre.Width;
                enemy.position.Y = fenetre.Height;

                bulletenemy.position.X = fenetre.Width;
                bulletenemy.position.Y = fenetre.Height;
            }

            //Si l'ennemi est vivant:
            if (enemy.estVivant == true)
            {
                //Mouvement de l'ennemi                                                                                         ///////////////////////
                enemy.direction.X = 0;
                enemy.direction.Y = 0;

                if (enemy.position.X > fenetre.Width - enemy.sprite.Width || enemy.position.X < 0)
                {
                    enemy.vitesse = -enemy.vitesse;
                }
                enemy.direction.X = enemy.vitesse;

                //On vient coller l'ennemi au plafond en "settant" sa vitesse en Y à -5 si il n'est pas au plafond
                if (enemy.position.Y > 500)
                {
                    enemy.direction.Y = -5;
                }                                                                                                               /////////////////////////


                //Empêche l'ennemi de sortir de la zone de jeu:
                if (enemy.position.X > fenetre.Width - enemy.sprite.Width)
                {
                    enemy.position.X = fenetre.Width - enemy.sprite.Width;
                }
                if (enemy.position.X < 0)
                {
                    enemy.position.X = 0;
                }
                if (enemy.position.Y > fenetre.Height - enemy.sprite.Height)
                {
                    enemy.position.Y = fenetre.Height - enemy.sprite.Height;
                }
                if (enemy.position.Y < 0)
                {
                    enemy.position.Y = 0;
                }


                //Update de la position de l'enemy à chaque frame
                enemy.position += enemy.direction;
            }

            //Position et angle du canon ennemi
            enemy.positioncanon.X = enemy.position.X + (enemy.sprite.Width / 2);
            enemy.positioncanon.Y = enemy.position.Y + (enemy.sprite.Height / 2);

            float angle = (float)Math.Atan2(enemy.position.Y - hero.position.Y, enemy.position.X - hero.position.X);
            Debug.WriteLine(MathHelper.ToDegrees(angle).ToString());

            enemy.angle = (float)((angle) - 1.5707963267948966192313216916398);

            //Update de la position de la barre de vie:
            enemy.positionhealthbar.X = enemy.position.X;
            enemy.positionhealthbar.Y = enemy.position.Y - 20;


        }

        public void UpdateBulletEnemy() //////////////////////////////////////////////Bullet enemy;
        {
            if (bulletenemy.estVivant == false)
            {
                bulletenemy.position.X = fenetre.Width + 200;
                bulletenemy.position.Y = fenetre.Height + 200;
                tireenemy = 0;
            }

            if (enemy.estVivant == true) //IF !!!ENEMY!!! ESTVIVANT;
            {
                bulletenemy.estVivant = true;

                if (tireenemy == 0)
                {
                    bulletenemy.position.X = enemy.position.X + ((enemy.sprite.Width / 2));
                    bulletenemy.position.Y = enemy.position.Y + ((enemy.sprite.Width / 2));

                    double grandissement1;
                    float grandissement;

                    distanceHEx = hero.position.X - enemy.position.X;
                    distanceHEy = hero.position.Y - enemy.position.Y;
                    distanceHE = Math.Sqrt(Math.Pow(distanceHEx, 2) + Math.Pow(distanceHEy, 2));

                    grandissement1 = (distanceHE / 10);
                    grandissement = (float)grandissement1;

                    bulletenemy.direction.Y = ((hero.position.Y + (hero.sprite.Width / 2) - bulletenemy.sprite.Width / 2) - bulletenemy.position.Y) / grandissement;
                    bulletenemy.direction.X = ((hero.position.X + (hero.sprite.Height / 2) - bulletenemy.sprite.Height / 2) - bulletenemy.position.X) / grandissement;

                    tireenemy = 1;
                    pop.Play();
                }

                bulletenemy.position += bulletenemy.direction;

                if (bulletenemy.position.Y > fenetre.Height || bulletenemy.position.Y < 0 || bulletenemy.position.X > fenetre.Width || bulletenemy.position.X < 0)
                {
                    bulletenemy.estVivant = false;
                }

                bulletenemy.angle -= (float)(Math.PI / 15);
            }
        }

        public void UpdateColisions() /////////////////////////////////////////////////////////////////////////////////////////////////////Update Collisions
        {
            if (hero.hitbox(35).Intersects(bulletenemy.hitbox(6)) && bulletenemy.estVivant == true)
            {
                hero.health = hero.health-10;
                bulletenemy.estVivant = false;
                boum.Play();

                //Update de la barre de vie du héro
                hero.afficherhealthbar.Y = (-(hero.health-100))*25;
            }


            for (int i = 0; i < bullethero.Length; i++)
            {
                if (enemy.hitbox(35).Intersects(bullethero[i].hitbox(6)) && bullethero[i].estVivant == true)
                {
                    enemy.health = enemy.health - 10;
                    bullethero[i].estVivant = false;
                    boum.Play();

                    //Update de la barre de vie de l'ennemi
                    enemy.afficherhealthbar.Y = (-(enemy.health - 100)) * 25;
                }

                if (bulletenemy.hitbox(6).Intersects(bullethero[i].hitbox(6)) && bullethero[i].estVivant == true)
                {
                    bulletenemy.estVivant = false;
                    bullethero[i].estVivant = false;
                    boum.Play();
                }
            }
        }

        public void UpdateDustLayers() /////////////////////////////////////////////////////////////////////////////////////////////////////Update Collisions
        {
            

            if (dust.time == 900)
            {
                dust.time = 0;
                dust.vitesse = -dust.vitesse;
            }


            dust.position1.X -= dust.vitesse;
            dust.position2.X -= dust.vitesse;

            if (dust.position1.X == -dust.sprite1.Width)
                dust.position1.X = fenetre.Width;

            if (dust.position2.X == -dust.sprite2.Width)
                dust.position2.X = fenetre.Width;




            dust.time++;
            
        }

    }
}
