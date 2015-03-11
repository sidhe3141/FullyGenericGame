using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Input;
using MultithreadHandler;

namespace FullyGenericGame.ResourcesPackage
{
    /// <summary>
    /// A class used to pass information to InputWatchers.
    /// </summary>
    public class InputPackage:UpdatePackage
    {
        public InputPackage()
            : base()
        {
        }

        public InputPackage(InputPackage p)
            : base(p)
        {
        }

        public override Package Clone()
        {
            return new InputPackage(this);
        }
        /// <summary>
        /// Creates an InputPackage that is also a clone of an UpdatePackage.
        /// </summary>
        /// <param name="package">The UpdatePackage to be cloned.</param>
        /// <returns>A clone of the UpdatePackage that is also an InputPackage.</returns>
        public static InputPackage ForkIPFromUP(UpdatePackage package)
        {
            InputPackage output = new InputPackage();
            foreach (KeyValuePair<string, object> x in package.Data)
            {
                output.Data.Add(x.Key, x.Value);
            }
            return output;
        }
        /// <summary>
        /// Tests a specific InputWatcher.
        /// </summary>
        /// <param name="watcher">The watcher to test.</param>
        /// <returns>The watcher's Evaluate property.</returns>
        public bool Evaluate(InputWatcher watcher)
        {
            return watcher.Evaluate;
        }
    }
}
