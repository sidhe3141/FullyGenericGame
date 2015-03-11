using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Object;

namespace FullyGenericGame.Actions.ObjectManipulation
{
    public class AddObjectAction:ObjectManipulationAction
    {
        #region Fields
        GameObject addition;
        #endregion
        #region Constructor
        public AddObjectAction(GameObject target):base()
        {
            addition = target;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the GameObject to be added.
        /// </summary>
        public GameObject Addition { get { return addition; } }
        #endregion
    }
}
