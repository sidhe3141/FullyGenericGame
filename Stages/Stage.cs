using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Game;
using FullyGenericGame.Object;
using FullyGenericGame.Phases;
using FullyGenericGame.Actions;
using FullyGenericGame.Actions.ObjectManipulation;
using FullyGenericGame.ResourcesPackage;
using FullyGenericGame.Input;

namespace FullyGenericGame.Stages
{
    /// <summary>
    /// An area within the game, such as a mission, level, or menu.
    /// Author: James Yakura
    /// </summary>
    public abstract class Stage
    {
        #region Fields
        List<GameObject> objects;
        List<GameAction> actions;
        #region Update and Draw
        List<Phase> cachedUpdatePhases;
       Dictionary<Phase, List<GameObject>> cachedDrawLists;
       LoaderPackage loader;
        #endregion
       #region Inputs
       Dictionary<InputWatcher, KeyValuePair< RespondToInput,object>[]> inputs;
       #endregion
        #endregion
       #region Constructor
       public Stage()
        {
            objects = new List<GameObject>();
            actions = new List<GameAction>();
            List<Phase> throwaway1=UpdatePhases;
            Dictionary<Phase, List<GameObject>> throwaway2=DrawLists;
            inputs = new Dictionary<InputWatcher, KeyValuePair<RespondToInput,object>[]>();
        }
        #endregion
        #region Properties
        /// <summary>
        /// All objects on the stage.
        /// </summary>
        public List<GameObject> Objects
        {
            get { return objects; }
        }
        /// <summary>
        /// All actions to be processed by the stage.
        /// </summary>
        public List<GameAction> Actions
        {
            get { return actions; }
        }
        #region Update and Draw
        /// <summary>
        /// Gets the list of phases to be run through during update.
        /// </summary>
        public virtual List<Phase> UpdatePhases
        {
            get
            {
                //Assemble phases list in cachedUpdatePhases if none exists
                if (cachedUpdatePhases == null)
                {
                    cachedUpdatePhases = new List<Phase>();
                    foreach (GameObject x in objects)
                    {
                        //Add each Phase in an object's update phases that is not already in cachedUpdatePhases.
                        foreach (Phase y in x.UpdatePhases)
                        {
                            bool match = false;
                            foreach (Phase z in cachedUpdatePhases)
                            {
                                if (y.Equals(z))
                                {
                                    match = true;
                                    break;
                                }
                            }
                            if (!match)
                            {
                                cachedUpdatePhases.Add(y);
                            }
                        }
                    }
                }
                return cachedUpdatePhases;
            }
        }
        /// <summary>
        /// User-defined logic assigning game objects on the stage to draw lists and associating those draw lists with draw phases.
        /// </summary>
        public abstract Dictionary<Phase, List<GameObject>> ProcessDrawLists
        {
            get;
        }
        /// <summary>
        /// Gets the list of GameObjects to draw associated with each phase of drawing.
        /// </summary>
        public virtual Dictionary<Phase, List<GameObject>> DrawLists
        {
            get
            {
                if (cachedDrawLists == null)
                {
                    cachedDrawLists = ProcessDrawLists;

                }
                return cachedDrawLists;
            }
        }
        /// <summary>
        /// Gets the modifications that need to be made to update packages before passing them to the Stage.
        /// </summary>
        public abstract List<PackagePreparation> UpdateRequirements
        {
            get;
        }
        /// <summary>
        /// Gets the modifications that need to be made to draw packages before passing them to the Stage.
        /// </summary>
        public abstract List<PackagePreparation> DrawRequirements
        {
            get;
        }
        #endregion
        #region Inputs

        #endregion
        #endregion
        #region Methods
        #region Object Handling
        /// <summary>
        /// Adds an object to the stage.
        /// </summary>
        /// <param name="obj">The object to add.</param>
        public virtual void AddObject(GameObject obj)
        {
            objects.Add(obj);
            //Add the object to draw lists and update phases.
            //Add the object to the update phases lists.
            foreach (Phase x in obj.UpdatePhases)
            {
                bool ContainsThisPhase = false;
                for (int i = cachedUpdatePhases.Count-1; i > -1; i--)
                {
                    if (cachedUpdatePhases[i].Equals(x))
                    {
                        ContainsThisPhase = true;
                    }
                }
                if (!ContainsThisPhase)
                {
                    cachedUpdatePhases.Add(x);
                }
            }
            //Add the object to draw lists.
            AddToDrawLists(obj);
            obj.Load(PrepareLoaderPackage(obj,loader));
        }
        /// <summary>
        /// Adds an object to all appropriate draw lists.
        /// </summary>
        /// <param name="obj">The object to add to draw lists.</param>
        public virtual void AddToDrawLists(GameObject obj)
        {
            List<Phase> lists = AssignDrawLists(obj);
            foreach (Phase x in lists)
            {
                cachedDrawLists[x].Add(obj);
            }
        }
        /// <summary>
        /// Decides what draw lists a GameObject belongs on.
        /// </summary>
        /// <param name="obj">The object to be checked.</param>
        /// <returns>All Phases associated with the correct draw lists.</returns>
        public List<Phase> AssignDrawLists(GameObject obj)
        {
            List<Phase> output=new List<Phase>();
            foreach (Phase x in cachedDrawLists.Keys)
            {
                if (CheckDrawList(obj, x))
                {
                    output.Add(x);
                }
            }
            return output;
        }
        /// <summary>
        /// Checks whether or not a GameObject belongs on a given draw list.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <param name="list">The Phase associated with the draw list.</param>
        /// <returns>Whether or not the object belongs on this draw list.</returns>
        public abstract bool CheckDrawList(GameObject obj, Phase list);
        /// <summary>
        /// Removes an object from the stage.
        /// </summary>
        /// <param name="obj">The object to remove.</param>
        public virtual void RemoveObject(GameObject obj)
        {
            objects.Remove(obj);
            foreach(Phase x in cachedDrawLists.Keys)
            {
                if (cachedDrawLists[x].Contains(obj))
                {
                    cachedDrawLists[x].Remove(obj);
                }
            }
            List<Phase> throwaway=cachedUpdatePhases;
        }
        /// <summary>
        /// Adds a GameObject to the draw list associated with a specific Phase.
        /// </summary>
        /// <param name="obj">The object to add.</param>
        /// <param name="phase">The phase associated with the draw list.</param>
        public virtual void AddToDrawList(GameObject obj, Phase phase)
        {
            foreach (Phase x in cachedDrawLists.Keys)
            {
                if (x.Equals(phase))
                {
                    cachedDrawLists[x].Add(obj);
                }
            }
        }
        #endregion
        #region Action Handling
        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public virtual void HandleAction(StageLevelAction action)
        {
            if (action is StageOnlyAction)
            {
                HandleOwnAction(action);
            }
            if (action is ObjectLevelAction)
            {
                HandleOwnAction(action);
                foreach (GameObject x in objects)
                {
                    x.HandleAction((ObjectLevelAction)action);
                }
            }
        }
        /// <summary>
        /// Logic for handling Actions that affect the Stage itself.
        /// </summary>
        /// <param name="action">The action passed to the Stage.</param>
        public virtual void HandleOwnAction(StageLevelAction action)
        {
            #region Object Manipulation
            if (action is ObjectManipulationAction)
            {
                //If an object is to be added, add it.
                if (action is AddObjectAction)
                {
                    AddObject(((AddObjectAction)action).Addition);
                }
                //If an object is to be removed, remove it.
                else if (action is RemoveObjectAction)
                {
                    RemoveObject(((RemoveObjectAction)action).Subtraction);
                }
                //If the action involves searching for another object, run the search and send the recipients the result.
                else if (action is ObjectSearchAction)
                {
                    ObjectSearchAction castedAction=(ObjectSearchAction)action;
                    List<GameObject> results = new List<GameObject>();
                    foreach (GameObject x in objects)
                    {
                        if(castedAction.Compare(x))
                        {
                            results.Add(x);
                        }
                    }
                    foreach (GameObject x in castedAction.Recipients)
                    {
                        x.RaiseAction(new ObjectSearchCompleteAction(castedAction, results.ToArray<GameObject>()));
                    }
                }
            }
            #endregion
        }
        /// <summary>
        /// Gets all actions from objects in the Stage.
        /// </summary>
        public void GetAllActions()
        {
            //Iterate through the objects list.
            foreach (GameObject x in objects)
            {
                //Execute all SingleItemActions.
                x.HandleOwnActions();
                //Iterate through the actions list for the GameObject.
                foreach (GameAction y in x.Actions)
                {
                    //If the Action is not a SingleItemAction, add it to the stage's actions list.
                    if (!(y is SingleItemAction))
                    {
                        actions.Add(y);
                    }
                }
                //Clear the object's actions list.
                x.Actions.Clear();
            }
        }
        /// <summary>
        /// Executes all StageOnlyActions and StageWideActions on the Stage.
        /// </summary>
        public void HandleOwnActions()
        {
            foreach (GameAction x in actions)
            {
                if (x is StageLevelAction & !( x is GameLevelAction))
                {
                    if (x is StageOnlyAction)
                    {
                        HandleAction((StageOnlyAction)x);
                    }
                    else if (x is StageWideAction)
                    {
                        HandleAction((StageWideAction)x);
                    }
                }
            }
        }

        public void RaiseAction(GameAction action)
        {
            actions.Add(action);
        }
        #endregion
        #region Update and Draw
        /// <summary>
        /// Creates an update package for a GameObject, containing all information and resources needed to update it.
        /// </summary>
        /// <param name="obj">The GameObject to be updated.</param>
        /// <param name="phase">The phase of updating to be carried out.</param>
        /// <param name="external">External resources and information needed to assemble the update package.</param>
        /// <returns>A package containing all needed update resources.</returns>
        public virtual UpdatePackage AssembleUpdatePackage(GameObject obj, Phase phase, UpdatePackage external)
        {
            UpdatePackage result = CreatePackage<UpdatePackage>(external, phase);
            //Execute PackagePreparations.
            foreach (KeyValuePair<PackagePreparation, object> x in obj.UpdateRequirements[phase])
            {
                x.Key(result, x.Value);
            }
            return result;
        }
        /// <summary>
        /// Updates the stage and all objects on it.
        /// </summary>
        /// <param name="resources">All information and external resources needed to update the stage.</param>
        public virtual void Update(UpdatePackage resources)
        {
            List<Phase> phases = UpdatePhases;
            //DO NOT MULTITHREAD THIS!
            //Iterate through all phases.
            foreach (Phase x in phases)
            {
                //Duplicate the update package. If the phase is an InputPhase, fork the update package into an InputPackage.

                // Execute each phase on each object that has that phase.
                ExecutePhase(x,resources);
            }
            //Handle actions.
            GetAllActions();
        }
        /// <summary>
        /// Executes a phase of updating.
        /// </summary>
        /// <param name="phase">The phase to be executed.</param>
        public virtual void ExecutePhase(Phase phase, UpdatePackage resources)
        {
            //If the phase is an InputPhase, poll the object's inputs.
            if (phase is InputPhase)
            {

            }
            //TODO: Multithread this.
            foreach (GameObject y in objects)
            {
                //  Check for posession of the phase.
                bool hasPhase = false;
                foreach (Phase z in y.UpdatePhases)
                {
                    if (z.Equals(phase))
                    {
                        hasPhase = true;
                        break;
                    }
                }
                if (hasPhase)
                {
                    //  If the phase exists on the object:
                    //   Assemble the object's update package for that phase.
                    //   Update the object.
                    y.Update(AssembleUpdatePackage(y, phase, resources), phase);
                }
            }
        }
        /// <summary>
        /// Creates a display package for a GameObject, containing all information and resources needed to draw it.
        /// </summary>
        /// <param name="obj">The GameObject to be drawn.</param>
        /// <param name="phase">The phase of drawing to be carried out.</param>
        /// <param name="external">External resources and information needed to assemble the draw package.</param>
        /// <returns>The display package.</returns>
        public virtual DisplayPackage AssembleDrawPackage(GameObject obj, Phase phase, DisplayPackage external)
        {
            DisplayPackage package = CreatePackage<DisplayPackage>(external, phase);
            //Execute all necessary computations.
            foreach (PackagePreparation x in obj.ObjectSprite.Requirements[phase])
            {
                x(external, obj);
            }
            return package;
        }
        /// <summary>
        /// Draws the stage and all objects on it.
        /// </summary>
        /// <param name="resources">Information and external resources needed to draw the stage.</param>
        public virtual void Draw(DisplayPackage resources)
        {
            Dictionary<Phase,List<GameObject>> lists=DrawLists;
            //DO NOT MULTITHREAD THIS!
            foreach (Phase x in lists.Keys)
            {
                //TODO: Multithread this.
                foreach (GameObject y in lists[x])
                {
                    y.Draw(AssembleDrawPackage(y, x, resources), x);
                }
            }
        }
        /// <summary>
        /// Creates a resources package.
        /// </summary>
        /// <param name="external">External resources and information used to create the package.</param>
        /// <param name="phase">The phase for which the package is being prepared.</param>
        /// <returns>The package.</returns>
        public virtual T CreatePackage<T>(T external, Phase phase) where T:Package
        {
            T output = PackageAdjust.Copy(external);
            return output;
        }


        #endregion
        #region Load and Save
        /// <summary>
        /// Loads the Stage in preparation for play, including creating and loading all GameObjects in its initial state.
        /// </summary>
        public virtual void Load(LoaderPackage package)
        {
            AssignPackagePreparationDelegates(package.Game);
            package.Stage = this;
            loader = package;
        }
        /// <summary>
        /// Assigns any PackagePreparations that need to be executed at the Game level, such as those associated with Stage-level inputs.
        /// </summary>
        /// <param name="game">The Game to be used for executing these inputs.</param>
        public virtual void AssignPackagePreparationDelegates(GenericGame game)
        {
        }
        /// <summary>
        /// Prepares a LoaderPackage to be passed to a GameObject.
        /// </summary>
        /// <param name="obj">The GameObject to which the LoaderPackage is to be passed.</param>
        /// <param name="external">External resources and information needed to prepare the package.</param>
        /// <returns>A prepared loader package.</returns>
        public virtual LoaderPackage PrepareLoaderPackage(GameObject obj, LoaderPackage external)
        {
            LoaderPackage output = PackageAdjust.Copy(external);
            return output;
        }
        #endregion
        #region Inputs
        /// <summary>
        /// Assigns the stage's inputs, using the LoaderPackage specified.
        /// </summary>
        /// <param name="loader">A LoaderPackage used for assigning inputs.</param>
        public virtual void AssignInputs(LoaderPackage loader)
        {
        }
        /// <summary>
        /// Adds a new InputWatcher.
        /// </summary>
        /// <param name="input">The InputWatcher to be loaded.</param>
        /// <param name="response">The actions to be executed when the InputWatcher is triggered.</param>
        public void AddInput(InputWatcher input, KeyValuePair<RespondToInput,object>[] response)
        {
            inputs.Add(input, response);
            input.Load(loader);
        }
        /// <summary>
        /// Removes an InputWatcher.
        /// </summary>
        /// <param name="input">The InputWatcher to remove.</param>
        public void RemoveInput(InputWatcher input)
        {
            inputs.Remove(input);
        }
        /// <summary>
        /// Checks all inputs on the Stage.
        /// </summary>
        /// <param name="package">The InputPackage to be used for evaluating the inputs.</param>
        public void PollInputs(InputPackage package)
        {
            //TODO: Multithread this.
            //Poll each input-watcher.
            foreach (KeyValuePair<InputWatcher, KeyValuePair<RespondToInput, object>[]> x in inputs)
            {
                //TODO: Multithread this.
                //Prepare a clone of the input package.
                InputPackage input = PackageAdjust.Copy(package);
                foreach (KeyValuePair<PackagePreparation, object> y in x.Key.Requirements)
                {
                    y.Key(input,y.Value);
                }
                x.Key.PrimeForEvaluation(input);
                if (x.Key.Evaluate)
                {
                    //TODO: Multithread this.
                    //Execute each RespondToInput function.
                    foreach (KeyValuePair<RespondToInput, object> y in x.Value)
                    {
                        y.Key(y.Value);
                    }
                }
            }
        }
        #endregion
        #endregion
    }
    /// <summary>
    /// Makes a modification to a resource package to prepare it for use.
    /// </summary>
    /// <param name="package">The Package being modified.</param>
    /// <param name="arguments">Information dealing with the specifics of the modification. Note that when this is called by a Sprite,
    /// this is automatically the associated GameObject.</param>
    public delegate void PackagePreparation(Package package, object arguments);
        
}
