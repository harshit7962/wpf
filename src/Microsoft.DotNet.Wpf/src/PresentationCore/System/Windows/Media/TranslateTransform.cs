// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//                                             

using MS.Internal;

namespace System.Windows.Media
{
    #region TranslateTransform
    ///<summary>
    /// Create a translation transformation.
    ///</summary>
    public sealed partial class TranslateTransform : Transform
    {
        ///<summary>
        ///
        ///</summary>
        public TranslateTransform()
        {
        }

        ///<summary>
        /// Create a translation transformation.
        ///</summary>
        ///<param name="offsetX">Displacement amount in x direction.</param>
        ///<param name="offsetY">Displacement amount in y direction.</param>
        public TranslateTransform(
            double offsetX,
            double offsetY
            )
        {
            X = offsetX;
            Y = offsetY;
        }

        ///<summary>
        /// Return the current transformation value.
        ///</summary>
        public override Matrix Value
        {
            get 
            {
                ReadPreamble();
                
                Matrix matrix = Matrix.Identity;

                matrix.Translate(X, Y);

                return matrix;
            }
        }
        
        ///<summary>
        /// Returns true if transformation matches the identity transform.
        ///</summary>
        internal override bool IsIdentity
        {
            get 
            {
                return X == 0 && Y == 0 && CanFreeze;
            }
        }

        #region Internal Methods

        internal override void TransformRect(ref Rect rect)
        {
            if (!rect.IsEmpty)
            {
                rect.Offset(X, Y);
            }
        }

        /// <summary>
        /// MultiplyValueByMatrix - result is set equal to "this" * matrixToMultiplyBy.
        /// </summary>
        /// <param name="result"> The result is stored here. </param>
        /// <param name="matrixToMultiplyBy"> The multiplicand. </param>
        internal override void MultiplyValueByMatrix(ref Matrix result, ref Matrix matrixToMultiplyBy)
        {
            result = Matrix.Identity;

            // Set the translate + type
            result._offsetX = X;
            result._offsetY = Y;
            result._type = MatrixTypes.TRANSFORM_IS_TRANSLATION;

            MatrixUtil.MultiplyMatrix(ref result, ref matrixToMultiplyBy);
        }

        #endregion Internal Methods
    }
    #endregion
}
