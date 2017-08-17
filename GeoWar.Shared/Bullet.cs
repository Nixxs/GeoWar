using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoWar
{
    class Bullet : Entity
    {
        public Bullet(Vector2 position, Vector2 velocity)
        {
            image = Art.Bullet;
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = 8;
        }

        public override void Update(GameTime gameTime)
        {
            // orientation is updated each frame as long as the bullet is still moving
            if (Velocity.LengthSquared() > 0)
            {
                Orientation = Velocity.ToAngle();
            }

            // update the position of the bullet based on velocity each frame
            // consider doing this as pixels/second rather than pixels per frame
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // check if the position of the bullet is off screen or not
            // if it is off screen, set IsExpired to true thus removing it from the entity list
            if (GameRoot.Viewport.Bounds.Contains(Position.ToPoint()) == false)
            {
                IsExpired = true;
            }
        }
    }
}
