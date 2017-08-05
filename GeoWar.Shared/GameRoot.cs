using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public GameRoot()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _instance = this;  
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
            Art.Load(Instance);
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
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
