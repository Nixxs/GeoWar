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
            //ship logic goes here
        }
    }
}
