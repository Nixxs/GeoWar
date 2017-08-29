using Microsoft.Xna.Framework.Graphics;

namespace GeoWar
{
    /// <summary>
    /// A class for loading and storing all the textures in the game
    /// like the player, enemies and bullets. a static class cannot and
    /// does not need to be instantiated before execution.
    /// </summary>
    class Art
    {
        private static Texture2D _player;
        private static Texture2D _seeker;
        private static Texture2D _wanderer;
        private static Texture2D _bullet;
        private static Texture2D _pointer;
        private static SpriteFont _font;

        /// <summary>
        /// A helpful and tidy load method to load all the textures from a one line
        /// Art.Load() in the LoadContent() of GameRoot
        /// </summary>
        /// <param name="game"></param>
        public static void Load(GameRoot instance)
        {
            _player = instance.Content.Load<Texture2D>("Art\\Player");
            _seeker = instance.Content.Load<Texture2D>("Art\\Seeker");
            _wanderer = instance.Content.Load<Texture2D>("Art\\Wanderer");
            _bullet = instance.Content.Load<Texture2D>("Art\\Bullet");
            _pointer = instance.Content.Load<Texture2D>("Art\\Pointer");
            _font = instance.Content.Load<SpriteFont>("Font");
        }

        public static Texture2D Player
        {
            get { return _player; }
        }

        public static Texture2D Seeker
        {
            get { return _seeker; }
        }

        public static Texture2D Wanderer
        {
            get { return _wanderer; }
        }

        public static Texture2D Bullet
        {
            get { return _bullet; }
        }

        public static Texture2D Pointer
        {
            get { return _pointer; }
        }

        public static SpriteFont Font
        {
            get { return _font; }
        }
    }
}
