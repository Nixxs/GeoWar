using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeoWar
{
    /// <summary>
    /// an abstract class for all the entities in the game, as an abstract class the 
    /// child entitys like player ship or bullet will inherit methods and properites from 
    /// this class, but this class will never be implemented on it's own. It's just for common
    /// properties and methods to be stored that are shared by all entities.
    /// </summary>
    abstract class Entity
    {
        // protected properties are accessible only by the child class and this class
        protected Texture2D image;
        protected Color color = Color.White;

        public Vector2 Position;
        public Vector2 Velocity;
        public float Orientation;
        public float Radius = 20; // used for circular collision detection
        public bool IsExpired; // true if the entity was destroyed and should be deleted

        // if the image has been set then the size of the object is equal to the image size
        // otherwise size will be zero
        public Vector2 Size
        {
            get
            {
                // could also be written as:
                // return image == null ? Vector2.Zero : new Vector2(image.Wdith, image.Height)
                // I prefer to use the long hand version because it is more human readable
                if (image == null)
                {
                    return Vector2.Zero;
                }
                else
                {
                    return new Vector2(image.Width, image.Height);
                }
            }
        }

        // all entities must have an Update method defined that returns nothing
        // but each child entity can implement this update method differently
        // it just must have this as a method to be valid
        public abstract void Update(GameTime gameTime);

        // a virtual method that can be redefined by the child entity, its kind of 
        // like a default method for the child if no other "Draw" method is defined
        // then this one is given to it
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, 1f, SpriteEffects.None, 0);
        }
    }
}
