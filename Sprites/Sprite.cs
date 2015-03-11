using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Phases;
using FullyGenericGame.ResourcesPackage;
using FullyGenericGame.Stages;

namespace FullyGenericGame.Sprites
{
    /// <summary>
    /// An object that handles display code.
    /// </summary>
    public abstract class Sprite
    {

        #region Properties
        /// <summary>
        /// Gets the array of phases involved in drawing the sprite.
        /// </summary>
        public abstract List<Phase> DrawPhases
        {
            get;
        }
        /// <summary>
        /// Gets the required information types for drawing the sprite.
        /// </summary>
        public abstract Dictionary<Phase,List<PackagePreparation>> Requirements
        {
            get;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Draws the sprite.
        /// </summary>
        /// <param name="resources">The resources needed to draw the sprite.</param>
        /// <param name="phase">The phase of drawing, for sprites that require more than one phase to draw.</param>
        public abstract void Draw(DisplayPackage resources, Phase phase);
        /// <summary>
        /// Loads the sprite.
        /// </summary>
        /// <param name="package">Resources and information needed to load the sprite.</param>
        public abstract void Load(LoaderPackage package);
        #endregion
    }
}
