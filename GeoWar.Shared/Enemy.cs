using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoWar
{
    class Enemy : Entity
    {
        private const float inactiveTime = 1500f; // time before enemy can start moving
        private float timeUntilStart = inactiveTime; //track how long the enemy has waited
        public bool _isActive;
        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>(); // list for storing behviours

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
        public static Enemy CreateSeeker(Vector2 position)
        {
            // create the new enemy object
            Enemy enemy = new Enemy(Art.Seeker, position);
            // add the behaviour for a seeker
            enemy.AddBehaviour(enemy.FollowPlayer(40f));

            return enemy;
        }

        // the follow player behaviour
        IEnumerable<int> FollowPlayer(float acceleration)
        {
            while (true)
            {
                // get the vector for the ships position relative to the current postion
                // then scale it down to the acceleration value 
                Velocity += (PlayerShip.Instance.Position - Position).ScaleTo(acceleration);
                // if the enemy is moving (ie its active, then orientate itself to point to the player/the
                // direction it's moving in
                if (Velocity != Vector2.Zero)
                {
                    Orientation = Velocity.ToAngle();
                }
                yield return 0;
            }
        }


    }
}
