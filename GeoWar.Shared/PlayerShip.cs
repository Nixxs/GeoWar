using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GeoWar
{
    class PlayerShip : Entity
    {
        private static PlayerShip _instance;
        const float shootCooldown = 120; //number of milli seconds between shots
        float cooldownRemaining = 0; // keep track of how many frames has past since last shot
        static Random rand = new Random(); // need this to generate random floats

        // if the player instance property hasn't already been created then create one
        // and return that value, otherwise just return the object that had been assigned
        // earlier
        public static PlayerShip Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayerShip();  
                }

                return _instance;
            }
        }

        /// <summary>
        /// This is a struct i think, which is almost like a class within a class
        /// i have to assume it inherits everything from the parent class "entity"
        /// and we define anything else we would want as well then it can be used
        /// as the creation of the player class. Its like a constructor for the player.
        /// that allows the user to create a player object without having to use the 
        /// new keyword.
        /// </summary>
        private PlayerShip()
        {
            image = Art.Player;
            Position = GameRoot.ScreenSize / 2;
            Radius = 10;
        }

        public override void Update(GameTime gameTime)
        {
            // need to consider changing this to use time instead of update cycles
            // eg velocity needs to be distance/second instead of distance per update cycle
            const float speed = 500; // speed is the multiplier for the movement direction
            Velocity = (speed * (float)gameTime.ElapsedGameTime.TotalSeconds) * Input.GetMovementDirection(); // velocity is the final delta value to move the player from his current position to his new position between update cycles
            Position += Velocity;
            // this is a smart way to limit the movement of the ship to the inside of the screen extents
            // the other way to do this is to set the max x max y etc.. and reset the position of the player
            // whenever he goes beyond these positions. Instead, here we use vector clamp which allows us to
            // set the min and max vectors that the postion variable can be set to. the Size/2 is to account 
            // for the texture half disappearing if it was just set to 0,0 min and screensize max.
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

            // here we are using our helper extension function from the extensions.cs file
            // to return radians of velocity so that we can assign the orientation value
            if (Velocity.LengthSquared() > 0)
            {
                Orientation = Velocity.ToAngle();
            }

            Vector2 aim = Input.GetAimDirection();

            // if the player is aiming in a direction and there is no cooldown left on shooting then
            // create bullets objects, we can also add in a requirement for a button down here as well
            // at the moment as long as the player has an aim button pushed (arrow keys, mouse movement, thumbstick)
            // then we will shoot.
            if (aim.LengthSquared() > 0 && cooldownRemaining <= 0 && gameTime.TotalGameTime.TotalMilliseconds >= 500)
            {
                cooldownRemaining = shootCooldown; // reset the cooldown remaining to full cd
                float aimAngle = aim.ToAngle(); // get the angle that the bullets should be moving in
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle); // a quanternion is used for determining orientation in a 3D space and is required by the vector2 transform method

                float randomSpread = rand.NextFloat(-0.3f, 0.3f); // generate some variability in the bullet angle to give it a machinegun effect
                Vector2 bulletVelocity = (float)gameTime.ElapsedGameTime.TotalSeconds * MathUtil.FromPolar(aimAngle + randomSpread, 600f); // generate the velocity of the bullet based on the angle it leaves the ship at plus a magnitude for bullet speed

                // the starting position of the bullet is the position of the player ship plus an offset of 25pix in the x axis  
                // and 8 pix in the y axis but oriented in the aim direction of the player 
                // B   B
                //   P
                // this tutorial code is something i don;t really understand the math for yet
                Vector2 offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, bulletVelocity));

                offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, bulletVelocity));
            }

            // reduce the cooldown remaining by 1 frame
            if (cooldownRemaining > 0)
            {
                cooldownRemaining -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }
    }
}
