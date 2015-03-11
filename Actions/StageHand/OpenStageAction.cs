using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Stages;

namespace FullyGenericGame.Actions.StageHand
{
    /// <summary>
    /// An action indicating that a new stage is to be added on top of the current one, such as a menu or minigame.
    /// Note: If more than one of these is fired on a single cycle, more than one stage will be added to the active stack. Unless opening alerts
    /// and suchlike (and sometimes even then), <i>make sure</i> that you do not do this!
    /// Author: James Yakura.
    /// </summary>
    public class OpenStageAction:StageManipulationAction
    {
        #region Fields
        Stage stage;
        #endregion
        #region Constructor
        /// <summary>
        /// Creates an action signifying that a new Stage is to be opened.
        /// </summary>
        /// <param name="stage">The Stage to open.</param>
        public OpenStageAction(Stage stage)
        {
            this.stage = stage;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the Stage to be opened.
        /// </summary>
        public Stage Target { get { return stage; } }
        #endregion
    }
}
