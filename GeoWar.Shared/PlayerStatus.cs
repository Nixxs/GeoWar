﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoWar
{
    static class PlayerStatus
    {
        private const float multiplierExpiryTime = 1f;
        private const int maxMultiplier = 20;

        public static int _lives;
        public static int _score;
        public static int _multiplier;

        // the multiplyer expires after a certain time so we need to keep track of how much time is 
        // left before we expire it back to 1
        private static float multiplierTimeLeft;
        // every 2k point sthe player will get an extra life so this is to keep track of how close he
        // is to that next life
        private static int scoreForExtraLife;

        // for storing how many lives the player has 
        public static int Lives
        {
            get
            {
                return _lives;
            }
            private set
            {
                _lives = value;
            }
        }

        // score stage facility
        public static int Score
        {
            get
            {
                return _score;
            }
            private set
            {
                _score = value;
            }
        }

        // keep track of the multiplier
        public static int Multiplier
        {
            get
            {
                return _multiplier;
            }
            private set
            {
                _multiplier = value;
            }
        }

        // the the reset method on construction
        static PlayerStatus()
        {
            Reset();
        }

        // the reset method sets all the values back to the start
        public static void Reset()
        {
            Score = 0;
            Multiplier = 1;
            Lives = 4;
            scoreForExtraLife = 2000;
            multiplierTimeLeft = 0;
        }

        public static void Update(GameTime gameTime)
        {
            // reset the multiplier clock and value if the player hasn't killed anything for a while 
            if (Multiplier > 1)
            {
                // if the player has a multiplier applied, subtract some time from it when the player kills something
                // we will increase the multiplier timer
                multiplierTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                // if the player hadn't killed anything in a while and therefore hadn't had time added to the multiplier
                // timer, then reset the multiplyer back to 1
                if (multiplierTimeLeft <= 0)
                {
                    multiplierTimeLeft = multiplierExpiryTime;
                    ResetMultiplier();
                }
            }
        }

        public static void AddPoints(int basePoints)
        {
            // first check if the player is alive or not, if he is dead then just exit
            if (PlayerShip.Instance.IsDead)
            {
                return;
            }
            
            // add to the score the number of points and apply the multiplier
            Score += basePoints * Multiplier;

            // add a life to the player each time he gets 2000 points
            while (Score >= scoreForExtraLife)
            {
                // increase the score for extra life by 2000 each time a life has been added
                // this loop keeps running until the score required to gain an extra life is above the
                // current score. This is to handle the event where the player might get 4000 points in one
                // kill which would mean he should get 2 lives this loop will keep moving through and applying the
                // earned lives until the score for extra life has been caught up and above the current score.
                scoreForExtraLife += 2000;
                Lives += 1;
            }
        }

        public static void IncreaseMultiplier()
        {
            // first check if the player is alive or not, if he is dead then just exit
            if (PlayerShip.Instance.IsDead)
            {
                return;
            }

            // reset the multiplyer time left to the expiry time since the player just got a kill
            multiplierTimeLeft = multiplierExpiryTime;

            // increment multiplier if it is less than the max multiplier possible
            if (Multiplier < maxMultiplier)
            {
                Multiplier += 1;
            }
        }

        // method for resetting the multiplier
        public static void ResetMultiplier()
        {
            Multiplier = 1;
        }

        // method for subtracting lives from the player
        public static void RemoveLife()
        {
            Lives -= 1;
        }
    }
}
