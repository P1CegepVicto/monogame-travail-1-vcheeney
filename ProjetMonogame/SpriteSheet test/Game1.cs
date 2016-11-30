using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static SpriteSheet_test.GameObjectAnime;

namespace SpriteSheet_test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        KeyboardState keys = new KeyboardState();
        KeyboardState previousKeys = new KeyboardState();
        GameObjectAnime rambo;
        private int runState;

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

            // TODO: use this.Content to load your game content here



            rambo = new GameObjectAnime();
            rambo.direction = Vector2.Zero;
            rambo.vitesse = 2;
            rambo.objetState = GameObjectAnime.etats.attenteDroite;
            rambo.position = new Rectangle(350, 250, 65, 65);   //Position initiale de Rambo
            rambo.sprite = Content.Load<Texture2D>("Rambo.png");

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


            //On appelle la méthode Update de Rambo qui permet de gérer les états
            UpdateRambo(gameTime);
            previousKeys = keys;


            keys = Keyboard.GetState();
            rambo.position.X += (int)(rambo.vitesse * rambo.direction.X);


            if (keys.IsKeyDown(Keys.Right))
            {
                rambo.direction.X = 2;
                rambo.objetState = GameObjectAnime.etats.runDroite;
            }
            if (keys.IsKeyUp(Keys.Right) && previousKeys.IsKeyDown(Keys.Right))
            {
                rambo.direction.X = 0;
                rambo.objetState = GameObjectAnime.etats.attenteDroite;
            }








            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            spriteBatch.Begin();
            spriteBatch.Draw(rambo.sprite, rambo.position, rambo.spriteAfficher, Color.White);

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }










        public void UpdateRambo(GameTime gameTime)
        {
            if (rambo.objetState == GameObjectAnime.etats.attenteDroite)
            {
                rambo.spriteAfficher = rambo.tabAttenteDroite[rambo.waitState];
            }
            if (rambo.objetState == etats.runDroite)
            {
                if (runState == 1)
                    rambo.spriteAfficher = new Rectangle(60, 30, 65, 65);
                if (runState == 2)
                    rambo.spriteAfficher = new Rectangle(130, 30, 65, 65);
                if (runState == 3)
                    rambo.spriteAfficher = new Rectangle(193, 30, 65, 65);
                if (runState == 4)
                    rambo.spriteAfficher = new Rectangle(260, 30, 65, 65);
                if (runState == 5)
                    rambo.spriteAfficher = new Rectangle(320, 30, 65, 65);
                if (runState == 6)
                    rambo.spriteAfficher = new Rectangle(385, 30, 65, 65);
            }




            //Compteur permettant de gérer le changement d'images
            rambo.cpt++;
            if (rambo.cpt == 6) //Vitesse défilement
            {
                //Gestion de la course
                rambo.runState++;
                if (rambo.runState == rambo.nbEtatRun)
                {
                    rambo.runState = 0;
                }
                rambo.cpt = 0;
            }
        }







   


    }
}
