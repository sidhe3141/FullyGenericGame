using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Phases;
using FullyGenericGame.Stages;
using FullyGenericGame.ResourcesPackage;

namespace FullyGenericGame.Sprites
{
    /// <summary>
    /// A sprite that displays multiple other sprites, such as a particle effect, health box, or vector graphic.
    /// Author: James Yakura
    /// </summary>
    public class MultiSprite:Sprite
    {
        #region Fields
        List<Sprite> sprites;
        LoaderPackage loader;
        #endregion
        #region Constructor
        public MultiSprite()
        {
            sprites = new List<Sprite>();
        }
        #endregion
        #region Properties

        public override List<Phases.Phase> DrawPhases
        {
            get
            {
                List<Phase> output = new List<Phase>();
                foreach (Sprite x in sprites)
                {
                    foreach (Phase y in x.DrawPhases)
                    {
                        if (!output.Contains(y))
                        {
                            output.Add(y);
                        } 
                    }
                }
                return output;
            }
        }

        public override Dictionary<Phase, List<PackagePreparation>> Requirements
        {
            get
            {
                Dictionary<Phase, List<PackagePreparation>> output=new Dictionary<Phase,List<PackagePreparation>>();
                foreach (Phase x in DrawPhases)
                {
                    output.Add(x, new List<PackagePreparation>());
                }
                foreach (Sprite x in sprites)
                {
                    foreach (KeyValuePair<Phase, List<PackagePreparation>> y in x.Requirements)
                    {
                        foreach (PackagePreparation z in y.Value)
                        {
                            output[y.Key].Add(z);
                        }
                    }
                }

                return output;
            }
        }
        /// <summary>
        /// Gets the list of sprites being drawn. Do not use this to add unloaded sprites, as they are not loaded by this method.
        /// </summary>
        public List<Sprite> Sprites
        {
            get
            {
                return sprites;
            }
        }
        #endregion
        #region Methods
        public override void Draw(ResourcesPackage.DisplayPackage resources, Phase phase)
        {
            //TODO: Multithread this.
            foreach (Sprite x in sprites)
            {
                x.Draw(resources, phase);
            }
        }

        public override void Load(LoaderPackage package)
        {
            loader = package;
        }
        /// <summary>
        /// Adds a new sprite to the MultiSprite.
        /// </summary>
        /// <param name="sprite">The sprite to be added.</param>
        public void AddSprite(Sprite sprite)
        {
            sprites.Add(sprite);
            sprite.Load(loader);
        }
        #endregion
    }
}
