using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Stages;
using FullyGenericGame.Actions;
using FullyGenericGame.Actions.StageHand;
using FullyGenericGame.Input;
using FullyGenericGame.ResourcesPackage;
using FullyGenericGame.Object;

namespace FullyGenericGame.Game
{
    public abstract class GenericGame
    {
        #region Fields
        List<Stage> allStages;
        Stack<Stage> activeStages;
        #region Event Handling
        List<GameAction> actions;
        #endregion
        #region Input
        Dictionary<InputWatcher, KeyValuePair<RespondToInput,object>> inputs;
        #endregion
        #endregion
        #region Constructor
        public GenericGame()
        {
            allStages = new List<Stage>();
            activeStages = new Stack<Stage>();
            #region Event Handling
            actions = new List<GameAction>();
            #endregion
            #region Input
            inputs = new Dictionary<InputWatcher,KeyValuePair< RespondToInput,object>>();
            #endregion
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the list of all stages that can be affected by GameWideActions.
        /// </summary>
        public List<Stage> AllStages
        {
            get { return allStages; }
        }
        /// <summary>
        /// Gets the stack of all stages that are currently displayed or ready to display.
        /// </summary>
        public Stack<Stage> ActiveStages
        {
            get { return activeStages; }
        }
        #region Event Handling
        /// <summary>
        /// Gets the list of all actions the game must take.
        /// </summary>
        public List<GameAction> Actions
        {
            get { return actions; }
        }
        #endregion
        #region Input
        public Dictionary<InputWatcher, KeyValuePair<RespondToInput,object>> Inputs
        {
            get { return inputs; }
        }
        #endregion
        #endregion
        #region Methods
        #region Stage Manipulation
        /// <summary>
        /// Adds a stage to the game.
        /// </summary>
        /// <param name="stage">The stage to be added.</param>
        public void AddStage(Stage stage)
        {
            if (!allStages.Contains(stage))
            {
                allStages.Add(stage);
            }
            stage.Load(CreateLoaderPackage(stage));
        }
        /// <summary>
        /// Puts a stage on the top of the active stack.
        /// </summary>
        /// <param name="stage">The stage to be opened.</param>
        public void OpenStage(Stage stage)
        {
            AddStage(stage);
            activeStages.Push(stage);
        }
        /// <summary>
        /// Removes the topmost stage from the active stack.
        /// </summary>
        public void CloseStage()
        {
            activeStages.Pop();
        }
        /// <summary>
        /// Removes the topmost stage from the game entirely.
        /// </summary>
        public void RemoveStage()
        {
            allStages.Remove(activeStages.Pop());
        }
        #endregion
        #region Event Handling
        /// <summary>
        /// Handles an action on the game level.
        /// </summary>
        /// <param name="action">The action to be handled.</param>
        public void HandleAction(GameLevelAction action)
        {
            if (action is GameOnlyAction)
            {
                HandleOwnAction(action);
            }
            if (action is GameWideAction)
            {
                foreach (Stage x in allStages)
                {
                    x.HandleAction((GameWideAction)action);
                }
                HandleOwnAction(action);
            }
        }
        /// <summary>
        /// Handles actions that affect the game itself.
        /// </summary>
        /// <param name="action">The action to be processed.</param>
        public virtual void HandleOwnAction(GameLevelAction action)
        {
            #region Stage Manipulation
            if (action is StageManipulationAction)
            {
                //If the action opens a new stage, open a stage.
                if (action is OpenStageAction)
                {
                    OpenStage(((OpenStageAction)action).Target);
                }
                //If the action closes a stage, close the topmost stage.
                else if (action is CloseStageAction)
                {
                    CloseStage();
                }
                //If the action kills a stage, remove the topmost stage from the game entirely.
                else if (action is KillStageAction)
                {
                    RemoveStage();
                }
                //If the action transitions a stage, close the topmost stage and add the new one.
                else if (action is TransitionStageAction)
                {
                    if (((TransitionStageAction)action).Kill) { RemoveStage(); }
                    else { CloseStage(); }

                    OpenStage(((TransitionStageAction)action).NewStage);
                }
                //If the action searches for a stage, run the search and pass the topmost stage the result.
                else if (action is SearchStageAction)
                {
                    SearchStageAction castedAction = (SearchStageAction)action;
                    List<Stage> results = new List<Stage>();
                    foreach (Stage x in allStages)
                    {
                        if (castedAction.Test(x))
                        {
                            results.Add(x);
                        }
                    }
                    StageSearchCompleteAction sendResult = new StageSearchCompleteAction(castedAction, results.ToArray());
                    foreach (GameObject x in castedAction.ObjectRecipients)
                    {
                        x.RaiseAction(sendResult);
                    }
                }
            }
            #endregion
        }
        /// <summary>
        /// Gathers and processes all actions in the game.
        /// </summary>
        public void GatherActions()
        {
            //Have the current Stage poll its own actions.
                activeStages.Peek().GetAllActions();
                //Have each Stage execute its own Stage- and GameObject-level actions.
                activeStages.Peek().HandleOwnActions();
                //Poll Stages for Game-level actions.
                for (int i=0; i<activeStages.Peek().Actions.Count;i++)
                {
                    if (activeStages.Peek().Actions[i] is GameLevelAction)
                    {
                        actions.Add(activeStages.Peek().Actions[i]);
                    }
                }
                ActiveStages.Peek().Actions.Clear();
            //Execute all Game-level actions.
            foreach (GameLevelAction y in actions)
            {
                HandleAction(y);
            }
            //Wipe actions list.
            actions.Clear();
        }
        #endregion
        #region Update and Draw
        /// <summary>
        /// Creates a package containing all resources necessary for stages to update.
        /// </summary>
        /// <returns></returns>
        public abstract UpdatePackage AssembleUpdatePackage();
        /// <summary>
        /// Updates the currently-active stage.
        /// </summary>
        public virtual void Update()
        {
            activeStages.Peek().Update(AssembleUpdatePackage());
            GatherActions();
        }
        /// <summary>
        /// Creates a package containing all resources necessary for stages to draw.
        /// </summary>
        /// <returns></returns>
        public abstract DisplayPackage AssembleDrawPackage();
        /// <summary>
        /// Draws the currently-active stage.
        /// </summary>
        public virtual void Draw()
        {
            activeStages.Peek().Draw(AssembleDrawPackage());
        }
        #endregion
        #region Input
        /// <summary>
        /// Call once per cycle. Gets information needed to prepare input packages.
        /// </summary>
        public abstract InputPackage GatherInputs();
        /// <summary>
        /// Handles any game inputs.
        /// </summary>
        /// <param name="package">External resources needed for testing inputs.</param>
        public void FireInputs(InputPackage package)
        {
            foreach (InputWatcher x in inputs.Keys)
            {
                InputPackage input = PackageAdjust.Copy(package);
                foreach (KeyValuePair< PackagePreparation,object> y in x.Requirements)
                {
                    y.Key(input, y.Value);
                }
                x.PrimeForEvaluation(input);
                if (x.Evaluate)
                {
                    inputs[x].Key(inputs[x].Value);
                }
            }
        }
        /// <summary>
        /// Gathers and fires game inputs.
        /// </summary>
        public void HandleInputs()
        {
            FireInputs(GatherInputs());
        }
        #endregion
        #region Load and Save
        /// <summary>
        /// Creates a loader package for a given stage, containing all information and resources that it needs in order to load.
        /// </summary>
        /// <param name="stage">The stage for which the loader package is created.</param>
        /// <returns>A new LoaderPackage.</returns>
        public virtual LoaderPackage CreateLoaderPackage(Stage stage)
        {
            LoaderPackage output = new LoaderPackage();
            output.Game = this;
            return output;
        }
        #endregion
        #endregion
        #region Delegates
        #region Input

        #endregion
        #endregion
    }
    /// <summary>
    /// Handles responses to input.
    /// </summary>
    public delegate void RespondToInput(object obj);
}
