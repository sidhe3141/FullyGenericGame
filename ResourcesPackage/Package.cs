using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullyGenericGame.Phases;

namespace FullyGenericGame.ResourcesPackage
{
    /// <summary>
    /// A package of resources and information used for passing these around when specific requirements are arbitrary.
    /// All subclasses should individually implement the following, so that PackageAdjust.Copy works properly:
    /// 
    /// <code>   
    ///  public T(args):base(args)
    ///  {
    ///   //this is the constructor
    ///  }
    ///  
    ///  public T(T p):base(p)
    ///  {
    ///   //this creates a clone
    ///  }
    ///  
    ///  public override Package Clone()
    ///  {
    ///   return new T(this);
    ///  }</code>

    /// Author:James Yakura
    /// </summary>
    public class Package
    {
        #region Fields
        Dictionary<string, object> data;

        Phase phase;
        #endregion
        #region Constructor
        public Package()
        {
            data = new Dictionary<string, object>();
        }

        public Package(Package p):this()
        {
            foreach (KeyValuePair<string, object> x in p.data)
            {
                data.Add(x.Key, x.Value);
            }
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the data associated with the Package.
        /// </summary>
        public Dictionary<string, object> Data
        {
            get
            {
                return data;
            }
        }

        public Phase Phase
        {
            get
            {
                return phase;
            }
            set
            {
                phase = value;
            }
        }
        #endregion
        #region Methods
        public virtual Package Clone()
        {
            return new Package(this);
        }

        
        #endregion
    }
}
