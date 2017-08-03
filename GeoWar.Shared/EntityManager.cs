using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeoWar
{
    /// <summary>
    /// A class for managing the updating and drawing of all the entities
    /// A static class is one that cannot be instantiated so only one of this 
    /// class can exist at any time during run time.
    /// </summary>
    static class EntityManager
    {
        // a list to store entity objects
        static List<Entity> entities = new List<Entity>();

        /// <summary>
        /// the isUpdating and addedEntities variables are used to handle the
        /// creation of new entities while the update method is running. If we try
        /// to update the entities list while iterating through it. we will get 
        /// an exception. 
        /// 
        /// So to get around this we will instead store any of the newly 
        /// generated entities into the addedEntities list first and then
        /// add them in to the entities list on the next update cycle rather than
        /// try to add them in while iterating through the list of entities.
        /// </summary>
        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();

        /// <summary>
        /// a property that returns the number of entities at any one time
        /// you could probably do this as a method as well like "getCount()"
        /// but this seems to be a nice way to do this, user can call it with
        /// EntityManager.Count
        /// </summary>
        public static int Count
        {
            get
            {
                return entities.Count;
            }
        }

        /// <summary>
        /// If this method is called while the we are iterating through the list
        /// of entities (like in the update method) then it will throw an exception
        /// so we only add to the entities list if we are not updating otherwise 
        /// we will add the entity in the addedEntities list instead
        /// </summary>
        /// <param name="entity"></param>
        public static void Add(Entity entity)
        {
            if (isUpdating == false)
            {
                entities.Add(entity);
            }
            else
            {
                addedEntities.Add(entity);
            }
        }

        /// <summary>
        /// The Update method runs through all the entities in the entities list
        /// and runs thier respective update methods.
        /// </summary>
        public static void Update()
        {
            // set this to true as soon as we start running just before running through
            // the entities list to run all the update methods
            isUpdating = true;

            // foreach can be used to read everything in a list but updating it 
            // while running through it should be done with a for loop only
            // we don't want to update it here so we are using the foreach
            foreach (Entity entity in entities)
            {
                entity.Update();
            }

            // set back to false after running all the update methods
            isUpdating = false;

            // now that all the update methods have finished running, we can add 
            // all the qued up, newly created entites to the entities list
            foreach (Entity entity in addedEntities)
            {
                entities.Add(entity);
            }
            // empty out the addedEntities list since they have now been added to the
            // production entities list
            addedEntities.Clear();

            // remove any expired entities
            // redefine the entities list to only the entities within the list that 
            // have an "IsExpired" attribute of "false" (Where uses the System.Linq library)
            entities = entities.Where(entity => entity.IsExpired = false).ToList();
        }

        /// <summary>
        /// method for running through all the draw methods of all the entities
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in entities)
            {
                entity.Draw(spriteBatch);
            }
        }
    }
}
