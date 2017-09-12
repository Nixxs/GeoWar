using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GeoWar
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameRoot : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static GameRoot _instance;

        // DEBUG CODE
        private SpriteFont debugTextFont;
        // DEBUG CODE

        public GameRoot()
        {
            _instance = this;

            // here we define the screen
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";

            // these are to unlink it from MonoGame's default frame rate and to use variable timesteps for update cycles
            Instance.IsFixedTimeStep = false;
        }

        /// <summary>
        /// A helpful property that returns a reference to this gameroot object, this will be
        /// used when classes need a reference to the instance. eg Art will need access to this
        /// object's content.
        /// </summary>
        public static GameRoot Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// A reference to this object's Viewport
        /// </summary>
        public static Viewport Viewport
        {
            get { return Instance.GraphicsDevice.Viewport; }
        }

        /// <summary>
        /// The screen size of this object's viewport
        /// </summary>
        public static Vector2 ScreenSize
        {
            get { return new Vector2(Viewport.Width, Viewport.Height); }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // load the base initialize first
            base.Initialize();

            // all other initializations come after base
            EntityManager.Add(PlayerShip.Instance);

            // set the music to repeating
            MediaPlayer.IsRepeating = true;
            // play the music defined in the sound class
            MediaPlayer.Play(Sound.Music);

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
            Art.Load(Instance);
            Sound.Load(Instance.Content);

            // DEBUG CODE
            debugTextFont = Content.Load<SpriteFont>("debugText");
            // DEBUG CODE
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
            Input.Update();
            EntityManager.Update(gameTime);
            EnemySpawner.Update(gameTime);
            PlayerStatus.Update(gameTime);

            base.Update(gameTime);
        }

        private void DrawRightAlignedString(string text, float y)
        {
            // this gets the width of the string in pixels
            float textWidth = Art.Font.MeasureString(text).X;
            // draws the string on the top right side of the screen 5 pixels from the right side
            spriteBatch.DrawString(Art.Font, text, new Vector2(ScreenSize.X - textWidth - 5, y), Color.White);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // the sort mode of texture uses the default deferred mode where sprites are drawn in the
            // order that they are called but ordered by texture first (don't really understand what that means though)
            // blendstate of additive adds the destination data to the source data without using alpha its how overlapping
            // textures are blended together
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            EntityManager.Draw(spriteBatch);

            // if the player is in a game over state display the score in the middle of the screen
            if (PlayerStatus.IsGameOver)
            {
                string gameOverText = string.Format("Game Over\nYour Score: {0}\nHigh Score: {1}", PlayerStatus.Score, PlayerStatus.HighScore);
                Vector2 textSize = Art.Font.MeasureString(gameOverText);
                // this is using vector math to find the centre of the screen and position the text right in
                // the middle of it textSize/2 get the middle of the text screensize/2 gets the middle of the
                // screen
                spriteBatch.DrawString(Art.Font, gameOverText, ScreenSize / 2 - textSize / 2, Color.White);
            }

            // draw the players lives on the top left of the screen 5,5
            spriteBatch.DrawString(Art.Font, string.Format("Lives: {0}", PlayerStatus.Lives), new Vector2(5), Color.White);
            DrawRightAlignedString(string.Format("Score: {0}", PlayerStatus.Score), 5);
            DrawRightAlignedString(string.Format("Multiplier: X{0}", PlayerStatus.Multiplier), 35);

            // draw the mouse pointer if the player is aiming with the mouse
            if (Input.isAimingWithMouse == true)
            {
                spriteBatch.Draw(Art.Pointer, Input.MousePosition, Color.White);
            }

            // DEBUG CODE
            string aimMode = Input.isAimingWithMouse ? "Mouse Aim" : "Keyboard aim";
            //spriteBatch.DrawString(debugTextFont, string.Format("Control: {3}\nAim: {0}\nMove: {1}\nOrientation: {2}\nSpawn Chance: 1 in {4}", Input.GetAimDirection(),  Input.GetMovementDirection(), PlayerShip.Instance.Orientation, aimMode, EnemySpawner.inverseSpawnchance), new Vector2(50, 70), Color.DeepSkyBlue);
            // DEBUG CODE

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
