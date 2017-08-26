using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoWar
{
    class Enemy : Entity
    {
        private const float inactiveTime = 500; // time before enemy can start moving
        private float timeUntilStart = inactiveTime; //track how long the enemy has waited
        public bool _isActive;
        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>(); // list for storing behviours
        private Random rand = new Random();

        // track when an enemy can start moving
        public bool IsActive
        {
            get
            {
                return timeUntilStart <= 0;
            }
        }

        public Enemy(Texture2D image, Vector2 position)
        {
            this.image = image;
            Position = position;
            Radius = image.Width / 2f;
            // this is transparent and not white so enemys start invisibile and will fade in
            // gradually until the IsActive switch returns true
            color = Color.Transparent; 
        }

        public override void Update(GameTime gameTime)
        {
            // if the enemy has waited its inactive time start working through behaviours
            // otherwise handle the inactive time (fade the enemy in slowly)
            if (timeUntilStart <= 0)
            {
                // exeute next bit of code in all assigned behaviours
                ApplyBehaviours();
            }
            else
            {
                // subtract number of milliseconds since last update was run
                timeUntilStart -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                // slowly fade in the enemy over time each frame as timeUntilStart/inactiveTime 
                // approaches 0 (since timeUntilStart is getting smaller each frame) the multiplies
                // (1 - (timeUntilStart / inactiveTime) will slowly incease from 0 - 1 each frame.
                color = Color.White * (1 - (timeUntilStart / inactiveTime));
            }

            // update the position based on the set velocity multiplied by the time that's passed since
            // last update
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            // limit the postion of the enemy to the size of the screen
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

            // the enemies will have a constant acceleration applied to them each update which will
            // result in the enemy speeding up to infinity over time. This is to limit that accelleration
            // to a terminal velocity. it's like setting friction.
            Velocity = Velocity * 0.9f;
        }

        // when this is run, it will kill off the enemy
        public void WasShot()
        {
            IsExpired = true;
        }

        // method for handling enemy to enemy collisions
        // we want them to just bounce of each other
        public void HandleCollision(Enemy other, GameTime gameTime)
        {
            // the difference between the this vector and the other determines the direction that the
            // enemy should bounce toward (away from the other)
            Vector2 direction = Position - other.Position;
            // as the distance between the two enemies increases after a collision we reduce the acceleration
            // applied. As direction.lengthsquard gets larger as the two objects move away rom each other the
            // acceleration applied gradually gets smaller too
            Velocity += 70000 * (direction / (direction.LengthSquared() + 1)) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        // method for adding behaviours to the behaviour list
        private void AddBehaviour(IEnumerable<int> behaviour)
        {
            // enumerables provide enumerators as a kind of "object" of self that keeps track of 
            // where it is at in the execution order
            behaviours.Add(behaviour.GetEnumerator());
        }

        private void ApplyBehaviours()
        {
            // iterate through the list of behaviours and move them along
            for (int i = 0; i < behaviours.Count; i++)
            {
                // movenext returns false when all the code in the enumerable has been executed within it's
                // created enumerator. so we'll remove the behaviour from the behaviours list if there is not
                // mode code to run. (in most cases behaviours run infinatley in while loops but just in case.
                if(behaviours[i].MoveNext() == false)
                {
                    behaviours.RemoveAt(i--);
                }
            }
        }

        // a factory for creating seeker enemies
        public static Enemy CreateSeeker(Vector2 position, GameTime gameTime)
        {
            // create the new enemy object
            Enemy enemy = new Enemy(Art.Seeker, position);
            // add the behaviour for a seeker
            enemy.AddBehaviour(enemy.FollowPlayer(2800f, gameTime));

            return enemy;
        }

        public static Enemy CreateWanderer(Vector2 position, GameTime gameTime)
        {
            Enemy enemy = new Enemy(Art.Wanderer, position);
            enemy.AddBehaviour(enemy.MoveRandomly(4000f, gameTime));
            return enemy;
        }

        // the follow player behaviour
        IEnumerable<int> FollowPlayer(float acceleration, GameTime gameTime)
        {
            while (true)
            {
                // get the vector for the ships position relative to the current postion
                // then scale it down to the acceleration value 
                // need to normalize this increase in velocity by time as well so that it runs the same on all machine speeds
                Velocity += (PlayerShip.Instance.Position - Position).ScaleTo(acceleration) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                // if the enemy is moving (ie its active, then orientate itself to point to the player/the
                // direction it's moving in
                if (Velocity != Vector2.Zero)
                {
                    Orientation = Velocity.ToAngle();
                }
                yield return 0;
            }
        }

        // the random movement behaviour
        IEnumerable<int> MoveRandomly(float speed, GameTime gameTime)
        {
            float direction = rand.NextFloat(0, MathHelper.TwoPi);

            while (true)
            {
                // set the direction initially after spawn then only run this once every 6 frames
                direction += rand.NextFloat(-0.1f, 0.1f);
                direction = MathHelper.WrapAngle(direction);

                for (int i = 0; i < 6; i++)
                {
                    Velocity += MathUtil.FromPolar(direction, speed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    // the orientation for the wanderer doesn't matter because its a circle
                    // all we are doing here is rotating it
                    Orientation -= 6f * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    // set the bounds for the entity to move in to be the game bounds
                    // minus the width and height of the enemy. doing it this way instead of doing the usual
                    // vector2 clamp allows us to use the bounds.Contains method later to check if the enemy
                    // has moved outside the bounds
                    var bounds = GameRoot.Viewport.Bounds;
                    bounds.Inflate(-image.Width, -image.Height);

                    // if the enemy is outside the bounds, make it move away from the edge
                    if (bounds.Contains(Position.ToPoint()) == false)
                    {
                        direction = (GameRoot.ScreenSize / 2 - Position).ToAngle() + rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);
                    }

                    yield return 0;
                }
            }
        }
    }
}
