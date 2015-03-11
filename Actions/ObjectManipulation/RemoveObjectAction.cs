using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Object;

namespace FullyGenericGame.Actions.ObjectManipulation
{
    public class RemoveObjectAction:ObjectManipulationAction
    {
        #region Fields
        GameObject subtraction;
        #endregion
        #region Constructor
        public RemoveObjectAction(GameObject target)
        {
            subtraction = target;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the GameObject to be removed.
        /// </summary>
        public GameObject Subtraction { get { return subtraction; } }
        #endregion
    }
}
