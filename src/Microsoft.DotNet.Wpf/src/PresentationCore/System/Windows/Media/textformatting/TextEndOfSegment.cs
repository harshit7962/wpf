// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
//
//
//  Contents:  Text modification API
//
//  Spec:      http://avalon/text/DesignDocsAndSpecs/Text%20Formatting%20API.doc
//
//

namespace System.Windows.Media.TextFormatting
{
    /// <summary>
    /// Specialized text run used to mark the end of a segment, i.e., to end
    /// the scope affected by a preceding TextModifier run.
    /// </summary>
    public class TextEndOfSegment : TextRun
    {
        private int _length;

        #region Constructors

        /// <summary>
        /// Construct an end of segment run
        /// </summary>
        /// <param name="length">number of characters</param>
        public TextEndOfSegment(int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

            _length = length;
        }

        #endregion


        /// <summary>
        /// Reference to character buffer
        /// </summary>
        public sealed override CharacterBufferReference CharacterBufferReference
        {
            get { return new CharacterBufferReference(); }
        }


        /// <summary>
        /// Character length
        /// </summary>
        public sealed override int Length
        {
            get { return _length; }
        }


        /// <summary>
        /// A set of properties shared by every characters in the run
        /// </summary>
        public sealed override TextRunProperties Properties
        {
            get { return null; }
        }
    }
}
