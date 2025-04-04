// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
// Description: Defines the ValueUnavailableException, thrown when a value requested
//              by a validation rule is not available.
//

namespace System.Windows.Data
{
    ///<summary>Exception class thrown when a value requested by a validation rule is not available</summary>
    [Serializable]
    public class ValueUnavailableException : SystemException
    {
        #region Constructors

        ///<summary>
        /// Constructor
        ///</summary>
        public ValueUnavailableException() : base ()
        {
        }

        ///<summary>
        /// Constructor
        ///</summary>
        ///<param name="message">
        /// Exception message
        ///</param>
        public ValueUnavailableException(string message) : base (message)
        {
        }

        ///<summary>
        /// Constructor
        ///</summary>
        ///<param name="message">Exception message</param>
        ///<param name="innerException">exception occured</param>
        public ValueUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
#pragma warning disable SYSLIB0051 // Type or member is obsolete
        ///<summary>
        /// Constructor
        ///</summary>
        ///<param name="message">Exception message</param>
        ///<param name="innerException">exception occured</param>
        protected ValueUnavailableException(System.Runtime.Serialization.SerializationInfo info,
                                            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
#pragma warning restore SYSLIB0051 // Type or member is obsolete
        #endregion Constructors
    }
}
