using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FullyGenericGame.Input
{
    /// <summary>
    /// A class that observes a changeable InputWatcher, usable for tracking a global input binding.
    /// Author: James Yakura
    /// </summary>
    /// <remarks>Note that the InputWatcher watched must be primed and evaluated separately.
    /// It is handled this way so that an InputWatcher observed by multiple IWWs does not re-evaluate for each one.</remarks>
    public class InputWatcherWatcher:InputWatcher
    {
        #region Fields
        InputWatcher input;
        #endregion
        #region Constructor
        /// <summary>
        /// Creates a new InputWatcherWatcher that passes the targeted IW's state directly.
        /// </summary>
        /// <param name="input">The InputWatcher to be observed.</param>
        public InputWatcherWatcher(InputWatcher input):base(InputWatcherType.True)
        {
            this.input = input;
        }
        /// <summary>
        /// Creates a new InputWatcherWatcher associated with a specific state or state-change in the targeted IW.
        /// </summary>
        /// <param name="input">The InputWatcher to be observed.</param>
        /// <param name="type">The state or state-change in the observed InputWatcher that triggers the IWW.</param>
        public InputWatcherWatcher(InputWatcher input, InputWatcherType type)
            : base(type)
        {
            this.input = input;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets and sets the InputWatcher to be observed.
        /// </summary>
        public InputWatcher Input
        {
            get { return input; }
            set { input = value; }
        }

        public override bool BasicEvent
        {
            get { return input.ProcessedEvent; }
        }

        public override bool Creation
        {
            get { return input.ProcessedEvent; }
        }

        public override KeyValuePair<Stages.PackagePreparation, object>[] Requirements
        {
            get { return new KeyValuePair<Stages.PackagePreparation, object>[0]; }
        }
        #endregion
        #region Methods

        public override void Load(ResourcesPackage.LoaderPackage package)
        {
        }

        public override void PrimeForEvaluation(ResourcesPackage.UpdatePackage requirements)
        {
        }
        #endregion
    }
}
