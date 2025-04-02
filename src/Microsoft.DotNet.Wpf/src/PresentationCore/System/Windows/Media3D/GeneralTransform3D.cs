// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
// 
//
// Description: Declaration of the GeneralTransform3D class.
//

using System.Windows.Media.Animation;
using MS.Internal.PresentationCore;

namespace System.Windows.Media.Media3D
{
    /// <summary>
    /// GeneralTransform3D class provides services to transform points and rects
    /// </summary>
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public abstract partial class GeneralTransform3D : Animatable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal GeneralTransform3D()
        {
        }
        
        /// <summary>
        /// Transform a point
        /// </summary>
        /// <param name="inPoint">Input point</param>
        /// <param name="result">Output point</param>
        /// <returns>True if the point was transformed successfuly, false otherwise</returns>
        public abstract bool TryTransform(Point3D inPoint, out Point3D result);
                
        /// <summary>
        /// Transform a point
        /// 
        /// If the transformation does not succeed, this will throw an InvalidOperationException.
        /// If you don't want to try/catch, call TryTransform instead and check the boolean it
        /// returns.
        ///
        /// </summary>
        /// <param name="point">Input point</param>
        /// <returns>The transformed point</returns>
        public Point3D Transform(Point3D point)
        {
            Point3D transformedPoint;

            if (!TryTransform(point, out transformedPoint))
            {
                throw new InvalidOperationException(SR.Format(SR.GeneralTransform_TransformFailed, null));
            }

            return transformedPoint;
        }
        
        /// <summary>
        /// Transforms the bounding box to the smallest axis aligned bounding box
        /// that contains all the points in the original bounding box
        /// </summary>
        /// <param name="rect">Bounding box</param>
        /// <returns>The transformed bounding box</returns>
        public abstract Rect3D TransformBounds(Rect3D rect);


        /// <summary>
        /// Returns the inverse transform if it has an inverse, null otherwise
        /// </summary>        
        public abstract GeneralTransform3D Inverse { get; }

        /// <summary>
        /// Returns a best effort affine transform
        /// </summary>
        internal abstract Transform3D AffineTransform
        {
            get;
        }
    }
}

