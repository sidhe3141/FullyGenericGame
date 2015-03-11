using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Object;
using FullyGenericGame.Phases;

namespace FullyGenericGame.ResourcesPackage
{
    /// <summary>
    /// A class that bundles together resources needed for drawing.
    /// See the Package documentation for more details.
    /// Author: James Yakura
    /// </summary>
    public class DisplayPackage:Package
    {
        /// <summary>
        /// Creates a new DisplayPackage.
        /// </summary>
        public DisplayPackage():base()
        {
        }

        /// <summary>
        /// Clones a display package.
        /// </summary>
        /// <param name="package">The package to be cloned.</param>
        public DisplayPackage(DisplayPackage package):base(package)
        {

        }

        public override Package Clone()
        {
            return new DisplayPackage(this);
        }

        public void Draw(GameObject obj)
        {
            obj.Draw(this, Phase);
        }
    }
}
