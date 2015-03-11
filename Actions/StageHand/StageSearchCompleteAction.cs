using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Stages;

namespace FullyGenericGame.Actions.StageHand
{
    /// <summary>
    /// An action used to pass a stage-search result to an object.
    /// Author: James Yakura
    /// </summary>
    public class StageSearchCompleteAction:SingleItemAction
    {
        #region Fields
        SearchStageAction cause;
        Stage[] results;
        #endregion
        #region Constructor
        /// <summary>
        /// Creates a new StageSearchCompleteAction.
        /// </summary>
        /// <param name="cause">The SearchStageAction for which this Action contains results.</param>
        /// <param name="results">The search results.</param>
        public StageSearchCompleteAction(SearchStageAction cause, Stage[] results)
        {
            this.cause = cause;
            this.results = results;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the SearchStageAction associated with the search results.
        /// </summary>
        public SearchStageAction Cause { get { return cause; } }
        /// <summary>
        /// Gets the search results.
        /// </summary>
        public Stage[] Results { get { return results; } }
        #endregion
    }
}
