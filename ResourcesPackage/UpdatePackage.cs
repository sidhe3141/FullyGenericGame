using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Object;

namespace FullyGenericGame.ResourcesPackage
{
    /// <summary>
    /// A Package for passing resources to the Update method.
    /// See the Package documentation for more details.
    /// Author: James Yakura.
    /// </summary>
    public class UpdatePackage:Package
    {
        #region Constructor
        /// <summary>
        /// Creates a new UpdatePackage.
        /// </summary>
        public UpdatePackage():base()
        {
        }
        /// <summary>
        /// Clones an existing UpdatePackage.
        /// </summary>
        /// <param name="p">The UpdatePackage to clone.</param>
        public UpdatePackage(UpdatePackage p):base(p)
        {
        }
        #endregion
        #region Methods
        public override Package Clone()
        {
            return new UpdatePackage(this);
        }

        public void Update(GameObject obj)
        {
            obj.Update(this, Phase);
        }
        #endregion
    }
}
