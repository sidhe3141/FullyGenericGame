using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Stages;

namespace FullyGenericGame.Actions.StageHand
{
    /// <summary>
    /// An action indicating that the current stage is to be exchanged with a new one.
    /// </summary>
    public class TransitionStageAction:StageManipulationAction
    {
        #region Fields
        bool kill;
        Stage newStage;
        #endregion
        #region Constructor
        /// <summary>
        /// Creates a new TransitionStageAction.
        /// </summary>
        /// <param name="kill">Whether or not to completely purge the current stage.</param>
        /// <param name="newStage">The new stage to add.</param>
        public TransitionStageAction(bool kill, Stage newStage)
        {
            this.kill = kill;
            this.newStage = newStage;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets whether or not to completely purge the current stage.
        /// </summary>
        public bool Kill { get { return kill; } }
        /// <summary>
        /// Gets the stage that replaces the current one.
        /// </summary>
        public Stage NewStage { get { return newStage; } }
        #endregion
    }
}
