using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoWar
{
    static class Sound
    {
        private static Song music;
        public static Song Music
        {
            get
            {
                return music;
            }
            private set
            {

            }
        }

        // a random object for generating random numbers for calling a random sound from a list
        private static readonly Random rand = new Random();

        // an array for holding all the explosion sounds, size 8 array because we thats how many 
        // explosion sounds we have
        private const int numberOfExplosionSounds = 8;
        private static SoundEffect[] explosions;
        // gets a random explosion sound from the list of explosions
        public static SoundEffect Explosion
        {
            get
            {
                return explosions[rand.Next(explosions.Length)];
            }
        }

        // an array of shot sounds
        private const int numberOfShotSounds = 4;
        private static SoundEffect[] shots;
        // get a random shot sound from the list of shot sounds
        public static SoundEffect Shot
        {
            get
            {
                return shots[rand.Next(shots.Length)];
            }
        }

        // a list for storing all the spawn sounds
        private const int numberOfSpawnSounds = 8;
        private static SoundEffect[] spawns;
        // get a random spawn sounds from a list of spawn sounds
        public static SoundEffect Spawn
        {
            get
            {
                return spawns[rand.Next(spawns.Length)];
            }
        }

        // the load method to be called in the main game class under it's load method
        public static void Load(ContentManager content)
        {
            // load up the music
            music = content.Load<Song>("Sound\\Music");

            // load all the explosion sounds to the explosions array
            explosions = new SoundEffect[numberOfExplosionSounds];
            for (int i = 0 ; i < numberOfExplosionSounds; i++)
            {
                explosions[i] = content.Load<SoundEffect>(string.Format("Sound\\explosion-0{0}", i + 1));
            }

            // load all the shot sounds to the shots array
            shots = new SoundEffect[numberOfShotSounds];
            for (int i = 0; i < numberOfShotSounds; i++)
            {
                shots[i] = content.Load<SoundEffect>(string.Format("Sound\\shoot-0{0}", i + 1));
            }

            // load all the spawn sounds to the spawns array
            spawns = new SoundEffect[numberOfSpawnSounds];
            for (int i = 0; i < numberOfSpawnSounds; i++)
            {
                spawns[i] = content.Load<SoundEffect>(string.Format("Sound\\spawn-0{0}", i + 1));
            }
        }
    }
}
