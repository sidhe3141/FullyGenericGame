using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Stages;
using FullyGenericGame.Object;

namespace FullyGenericGame.ResourcesPackage
{
    public class LoaderPackage:Package
    {
        #region Fields
        Game.GenericGame game;
        Stage stage;
        #endregion
        #region Inherited
        public LoaderPackage()
            : base()
        {

        }

        public LoaderPackage(LoaderPackage p)
            : base(p)
        {
            game = p.game;
            stage = p.stage;
        }

        public override Package Clone()
        {
            return new LoaderPackage(this);
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets and sets the Game associated with the loader package, used for assigning delegates for preparing packages.
        /// </summary>
        public Game.GenericGame Game
        {
            get
            {
                return game;
            }
            set
            {
                game = value;
            }
        }
        /// <summary>
        /// Gets and sets the Stage associated with the LoaderPackage, used for assigning delegates for preparing packages.
        /// </summary>
        public Stage Stage
        {
            get
            {
                return stage;
            }
            set
            {
                stage = value;
            }
        }
        #endregion
        #region Methods
        public void Load(GameObject obj)
        {
            obj.Load(this);
        }
        #endregion
    }
}
