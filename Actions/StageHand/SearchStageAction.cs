using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Object;
using FullyGenericGame.Stages;

namespace FullyGenericGame.Actions.StageHand
{
    /// <summary>
    /// An action telling the game to search for a specific Stage.
    /// Author: James Yakura
    /// </summary>
    public class SearchStageAction:GameOnlyAction
    {
        #region Fields
        GameObject[] objectRecipients;
        CheckStageForMatch function;
        object criteria;
        #endregion
        #region Constructor
        public SearchStageAction(GameObject[] objectRecipients, CheckStageForMatch function, object criteria)
        {
            this.objectRecipients = objectRecipients;
            this.function = function;
            this.criteria = criteria;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the objects that recieve the search results.
        /// </summary>
        public GameObject[] ObjectRecipients { get { return objectRecipients; } }
        #endregion
        #region Methods
        /// <summary>
        /// Checks a speciic Stage for the given criteria.
        /// </summary>
        /// <param name="stage">The stage to check.</param>
        /// <returns>Whether or not the Stage matches the criteria.</returns>
        public bool Test(Stage stage)
        {
            return function(criteria, stage);
        }
        #endregion
    }
    /// <summary>
    /// Checks a Stage to see if it matches a given set of parameters.
    /// </summary>
    /// <param name="criteria">The parameters used by the function.</param>
    /// <param name="stage">The Stage to check.</param>
    /// <returns>Whether or not the Stage matches the listed criteria.</returns>
    public delegate bool CheckStageForMatch(object criteria, Stage stage);
}
