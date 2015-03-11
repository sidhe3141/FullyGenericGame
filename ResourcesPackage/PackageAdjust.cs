using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FullyGenericGame.ResourcesPackage
{
    /// <summary>
    /// Methods for altering a generic Package.
    /// Author: James Yakura
    /// </summary>
    public static class PackageAdjust
    {
        /// <summary>
        /// Duplicates a Package.
        /// </summary>
        /// <typeparam name="T">The type of the package. Leave blank.</typeparam>
        /// <param name="source">The Package to be copied.</param>
        /// <returns>A clone of the Package, using the clone constructor defined in the package's class.</returns>
        public static T Copy<T>(this T source) where T : Package
        {
            return (T)source.Clone();
        }
    }
}
