using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Object;

namespace FullyGenericGame.Actions.ObjectManipulation
{
    public class ObjectSearchAction:ObjectManipulationAction
    {
        #region Fields
        GameObject[] recipients;
        CheckGameObject function;
        object parameter;
        object purpose;
        #endregion
        #region Constructor
        /// <summary>
        /// Creates a new ObjectSearchAction.
        /// </summary>
        /// <param name="recipients">The objects to receive the results of the search via a SearchCompleteAction.</param>
        /// <param name="function">The function to be used to determine whether or not a GameObject fits the criteria.</param>
        /// <param name="parameter">The search criteria.</param>
        /// <param name="purpose">An object describing the purpose of the search.</param>
        public ObjectSearchAction(GameObject[] recipients, CheckGameObject function, object parameter, object purpose):base()
        {
            this.recipients = recipients;
            this.function = function;
            this.parameter = parameter;
            this.purpose = purpose;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the GameObjects that recieve the results of the search.
        /// </summary>
        public GameObject[] Recipients { get { return recipients; } }
        /// <summary>
        /// Gets an object describing the purpose of the search.
        /// </summary>
        public object Purpose{get{return purpose;}}
        #endregion
        #region Methods
        public bool Compare(GameObject obj)
        {
            return function(obj, parameter);
        }
        #endregion
    }

    public delegate bool CheckGameObject(GameObject input, object parameter);
}
