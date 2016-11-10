using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle fenetre;
        GameObject hero;

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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Définition de la taille de la fenêtre
            fenetre = graphics.GraphicsDevice.Viewport.Bounds;
            fenetre.Width = graphics.GraphicsDevice.DisplayMode.Width;
            fenetre.Height = graphics.GraphicsDevice.DisplayMode.Height;

            hero = new GameObject(fenetre.Width/4-75,(fenetre.Height/2)-75,75,75,true); // C'est le condensé de tout mes propriété de départ de mon héro.(positionX,positionY,origineX,origineY,estVivant)
            hero.sprite = Content.Load<Texture2D>("TANK1.png");






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
            //Liste des actions qui updates
            
            UpdateHero();


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

            spriteBatch.Draw(hero.sprite, hero.position, Color.White);

            spriteBatch.End();



            base.Draw(gameTime);
        }















        public void UpdateHero()
        {
            //Déclaration des variable hero.direction si une touche n'est pas appuyée; (Sans ça la commande finale ne fonctionne pas) test un deux un deux
            hero.direction.X = 0;
            hero.direction.Y = 0;

            //Détection des touches de mouvement/autre
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();


                if (Keyboard.GetState().IsKeyDown(Keys.D) && hero.position.X<fenetre.Width-150)
                {
                    hero.direction.X = 20;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A) && hero.position.X>0)
                {
                    hero.direction.X = -20;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W) && hero.position.Y>0)
                {
                    hero.direction.Y = -20;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S) && hero.position.Y<fenetre.Height-150)
                {
                    hero.direction.Y = 20;
                }

            //Commande finale
            //Ça envois les changements de direction selon les touches appuyées. (Exemple: (2,-2) = -> + î)
            hero.position += hero.direction;

        }

    }
}
