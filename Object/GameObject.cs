using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Phases;
using FullyGenericGame.Actions;
using FullyGenericGame.Input;
using FullyGenericGame.ResourcesPackage;
using FullyGenericGame.Sprites;
using FullyGenericGame.Stages;

namespace FullyGenericGame.Object
{
    /// <summary>
    /// An object that exists within a game.
    /// </summary>
    public abstract class GameObject
    {
        #region Fields
        #region Event Handling
        List<GameAction> actions;
        #endregion
        #region Updating and Drawing
        Sprite sprite;
        List<Phase> cachedDrawPhases;
        #endregion
        #region Loading
        LoaderPackage loader;
        #endregion
        #endregion
        #region Constructor
        public GameObject()
        {
            actions = new List<GameAction>();
        }
        #endregion
        #region Properties
        #region Event Handling
        /// <summary>
        /// Gets the Actions raised by this object.
        /// </summary>
        public List<GameAction> Actions
        {
            get { return actions; }
        }
        #endregion
        #region Updating and Drawing
        /// <summary>
        /// Gets and sets the object's display sprite.
        /// </summary>
        public Sprite ObjectSprite
        {
            get
            {
                cachedDrawPhases = null;
                return sprite;

            }
            set
            {
                sprite = value;
                sprite.Load(loader);
                cachedDrawPhases = null;
            }
        }
        /// <summary>
        /// Gets the phases the object runs through while updating.
        /// </summary>
        public abstract Phase[] UpdatePhases
        {
            get;
        }
        /// <summary>
        /// Gets the phases the object runs through while drawing.
        /// </summary>
        public List<Phase> DrawPhases
        {
            get
            {
                if(cachedDrawPhases==null)
                {
                    cachedDrawPhases = sprite.DrawPhases;
                }
                return cachedDrawPhases;
            }
        }

        public abstract Dictionary<Phase, KeyValuePair<PackagePreparation, object>[]> UpdateRequirements
        {
            get;
        }
        #endregion
        #endregion
        #region Methods
        #region Action Handling
        /// <summary>
        /// Executes actions.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public abstract void HandleAction(ObjectLevelAction action);
        /// <summary>
        /// Raises an action.
        /// </summary>
        /// <param name="action">The action to be raised.</param>
        public void RaiseAction(GameAction action)
        {
            actions.Add(action);
        }
        /// <summary>
        /// Executes all Object-level actions.
        /// </summary>
        public void HandleOwnActions()
        {
            for (int i = 0; i < actions.Count;i++ )
            {
                if (!(actions[i] is StageLevelAction) & !(actions[i] is GameLevelAction))
                    HandleAction((SingleItemAction)actions[i]);
            }
        }
        #endregion
        #region Update and Draw
        /// <summary>
        /// Executes update logic.
        /// </summary>
        /// <param name="resources">The resources used in updating.</param>
        /// <param name="phase">The phase of updating currently being executed, for objects or games that update in more than one pass.</param>
        public abstract void Update(UpdatePackage resources, Phase phase);
        /// <summary>
        /// Draws the object.
        /// </summary>
        /// <param name="resources">Resources to be passed to the sprite.</param>
        /// <param name="phase">The phase of drawing currently being executed, for objects or games that draw in more than one pass.</param>
        public void Draw(DisplayPackage resources, Phase phase)
        {
            sprite.Draw(resources, phase);
        }
        #endregion
        #region Load and Save
        /// <summary>
        /// Loads external resources for the GameObject.
        /// </summary>
        /// <param name="package">Resources and information needed to load the sprite.</param>
        /// <remarks>The following items should be initialized before calling this method:
        /// ObjectSprite</remarks>
        public virtual void Load(LoaderPackage package)
        {
            sprite.Load(package);
            PrepareRequirements(package);
        }
        /// <summary>
        /// Gathers the object's update and draw requirements.
        /// </summary>
        /// <param name="package">The LoaderPackage containing the game and stage necessary for creating the update and draw requirements.</param>
        public virtual void PrepareRequirements(LoaderPackage package)
        {

        }
        #endregion
        #endregion
    }
}
