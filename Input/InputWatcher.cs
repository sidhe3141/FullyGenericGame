using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.ResourcesPackage;
using FullyGenericGame.Stages;

namespace FullyGenericGame.Input
{
    /// <summary>
    /// The type of change in a condition to watch for.
    /// </summary>
    public enum InputWatcherType
    {
        /// <summary>
        /// Fires if the basic condition is met.
        /// </summary>
        True,
        /// <summary>
        /// Fires if the basic condition is not met.
        /// </summary>
        False,
        /// <summary>
        /// Fires if the basic condition is met, and was not met last cycle.
        /// </summary>
        Begin,
        /// <summary>
        /// Fires if the basic condition is not met, and was met last cycle.
        /// </summary>
        End,
        /// <summary>
        /// Fires if the basic condition's truth-value has changed since last cycle.
        /// </summary>
        Change,
        /// <summary>
        /// Fires if the basic condition's truth-value has not changed since last cycle.
        /// </summary>
        Stay
    }
    /// <summary>
    /// An object that checks for a condition being met.
    /// Author: James Yakura
    /// </summary>
    public abstract class InputWatcher
    {
        #region Fields
        bool trueLastCycle;
        InputWatcherType type;
        #endregion
        #region Constructor
        public InputWatcher(InputWatcherType type)
        {
            this.type = type;
            trueLastCycle = Creation;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Checks the underlying event.
        /// </summary>
        public abstract bool BasicEvent
        {
            get;
        }
        /// <summary>
        /// Whether or not the basic event is true when the item is created.
        /// </summary>
        public abstract bool Creation
        {
            get;
        }
        /// <summary>
        /// Gets the processed event for the True type.
        /// </summary>
        public bool TrueEvent
        {
            get
            {
                return BasicEvent;
            }
        }
        /// <summary>
        /// Gets the processed event for the False type.
        /// </summary>
        public bool FalseEvent
        {
            get
            {
                return !BasicEvent;
            }
        }
        /// <summary>
        /// Gets the processed event for the Begin type.
        /// </summary>
        public bool BeginEvent
        {
            get
            {
                return BasicEvent & !trueLastCycle;
            }
        }
        /// <summary>
        /// Gets the processed event for the End type.
        /// </summary>
        public bool EndEvent
        {
            get
            {
                return !BasicEvent & trueLastCycle;
            }
        }
        /// <summary>
        /// Gets the processed event for the Change type.
        /// </summary>
        public bool ChangeEvent
        {
            get
            {
                return BasicEvent != trueLastCycle;
            }
        }
        /// <summary>
        /// Gets the processed event for the Stay type.
        /// </summary>
        public bool StayEvent
        {
            get
            {
                return BasicEvent == trueLastCycle;
            }
        }
        /// <summary>
        /// Gets the processed event.
        /// </summary>
        public bool ProcessedEvent
        {
            get
            {
                switch (type)
                {
                    case InputWatcherType.Begin:
                        return BeginEvent;
                    case InputWatcherType.Change:
                        return ChangeEvent;
                    case InputWatcherType.End:
                        return EndEvent;
                    case InputWatcherType.False:
                        return FalseEvent;
                    case InputWatcherType.Stay:
                        return FalseEvent;
                    case InputWatcherType.True:
                        return TrueEvent;
                    default:
                        return false;
                }
            }
        }
        /// <summary>
        /// Returns the processed event and advances by one cycle.
        /// </summary>
        public bool Evaluate
        {
            get
            {
                bool value = ProcessedEvent;
                trueLastCycle = BasicEvent;
                return value;
            }
        }
        /// <summary>
        /// Gets modifications that need to be made to an UpdatePackage in order to prime for evaluation.
        /// </summary>
        public abstract KeyValuePair<PackagePreparation,object>[] Requirements
        {
            get;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Passes the InputWatcher information and resources needed to evaluate its basic event.
        /// </summary>
        /// <param name="requirements">Information and resources needed to perform the evaluation.</param>
        public abstract void PrimeForEvaluation(UpdatePackage requirements);
        /// <summary>
        /// Loads information needed for evaluating the input, including assigning update requirements.
        /// </summary>
        /// <param name="package"></param>
        public abstract void Load(LoaderPackage package);

      
       #endregion
    }
}
