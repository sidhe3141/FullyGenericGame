using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Object;

namespace FullyGenericGame.Actions.ObjectManipulation
{
    public class ObjectSearchCompleteAction:SingleItemAction
    {
        #region Fields
        ObjectSearchAction cause;
        GameObject[] results;
        #endregion
        #region Constructor
        /// <summary>
        /// Creates a new SearchCompleteAction.
        /// </summary>
        /// <param name="cause">The ObjectSearchAction that was the search query.</param>
        /// <param name="results">The search results.</param>
        public ObjectSearchCompleteAction(ObjectSearchAction cause, GameObject[] results)
        {
            this.cause = cause;
            this.results = results;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the search query.
        /// </summary>
        public ObjectSearchAction Cause { get { return cause; } }
        /// <summary>
        /// Gets the search results.
        /// </summary>
        public GameObject[] Results { get { return results; } }
        #endregion
    }
}
