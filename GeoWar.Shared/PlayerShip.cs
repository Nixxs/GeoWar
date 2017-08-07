using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GeoWar
{
    class PlayerShip : Entity
    {
        private static PlayerShip _instance;

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

        public override void Update()
        {
            // need to consider changing this to use time instead of update cycles
            // eg velocity needs to be distance/second instead of distance per update cycle
            const float speed = 8; // speed is the multiplier for the movement direction
            Velocity = speed * Input.GetMovementDirection(); // velocity is the final delta value to move the player from his current position to his new position between update cycles
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
            
        }
    }
}
