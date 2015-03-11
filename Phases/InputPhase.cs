using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FullyGenericGame.Phases
{
    /// <summary>
    /// The Phase of the Update cycle used for handling user input and AI.
    /// </summary>
    public class InputPhase:Phase
    {
        #region Fields
        static InputPhase generic;
        #endregion
        #region Methods
        /// <summary>
        /// A factory that produces the same InputPhase each time it is called. Shadow this with "new" in subclasses.
        /// </summary>
        /// <returns>A generic InputPhase.</returns>
        public static InputPhase Generic()
        {
            if (generic == null)
            {
                generic = new InputPhase();
            }
            return generic;
        }
        #endregion
    }
}
