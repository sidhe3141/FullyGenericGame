using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FullyGenericGame.Phases
{
    /// <summary>
    /// A step in an update, draw, or other cycle.
    /// </summary>
    /// <remarks>
    /// This is not necessary, but is advised.
    /// All implementations of Phase, in order to save memory, should implement one of the following separately:
    /// <code>
    /// static T generic;
    /// 
    /// public static new T Generic()
    /// {
    ///  if(generic==null)
    ///  {
    ///   generic=new T();
    ///  }
    ///  return generic;
    /// }
    /// </code>
    /// or
    /// <code>
    /// static Dictionary&lt;U,Dictionary&lt;V, Dictionary&lt;...,T&gt;&gt;...&gt; generic;
    /// 
    /// public static new T Generic(U arg1, V arg2...)
    /// {
    ///  if(generic==null)
    ///  {
    ///   generic=new Dictionary&lt;U,Dictionary&lt;V,Dictionary&lt;...,T&gt;&gt;...&gt;();
    ///  }
    ///  if(!(generic.Keys.Contains(arg1)))
    ///  {
    ///   generic.Add(arg1,new Dictionary&lt;V,Dictionary&lt;W,Dictionary&lt;...,T&gt;&gt;...&gt;());
    ///  }
    ///  if(!(generic[arg1].Keys.Contains(arg2)))
    ///  {
    ///   generic[arg1].Add(arg2,new Dictionary&lt;W,Dictionary&lt;X,Dictionary&lt;...,T&gt;&gt;...&gt());
    ///  }
    ///  ...
    ///  if(!(generic[arg1][arg2]...[argN-1].Contains(argN)))
    ///  {
    ///   generic[arg1][arg2]...[argN-1].Add(argN,new T(arg1,arg2...));
    ///  }
    ///  return generic[arg1][arg2]...[argN];
    /// }
    /// </code>
    /// </remarks>
    public abstract class Phase
    {
        #region Fields
        #endregion
        #region Properties
        /// <summary>
        /// The hash code for this Phase. If two Phases have information differentiating them, append it to the end.
        /// </summary>
        public virtual string HashCode
        {
            get
            {
                return this.GetType().ToString();
            }
        }
        #endregion
        #region Methods

        public override bool Equals(object obj)
        {
            return (this.GetType() == obj.GetType());
        }

        public override int GetHashCode()
        {
            int sequence=0;
            for(int i=0;i<HashCode.Length;i++)
            {
                sequence+=(Convert.ToByte(HashCode[i]))*i;
                
            }
            return sequence;
        }     
        #endregion
    }
}
