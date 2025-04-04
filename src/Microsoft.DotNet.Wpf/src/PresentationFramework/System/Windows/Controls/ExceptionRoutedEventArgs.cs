// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Windows
{
    #region ExceptionRoutedEventArgs

    /// <summary>
    /// Holds the error event arguments for media failed events from a MediaElement.
    /// Since other MediaFailed events from the MediaClock or MediaTimeline are not
    /// routed events, this is separated out into a different class.
    /// </summary>
    public sealed class ExceptionRoutedEventArgs : RoutedEventArgs
    {
        internal
        ExceptionRoutedEventArgs(
            RoutedEvent     routedEvent,
            object          sender,
            Exception       errorException
            ) : base(routedEvent, sender)
        {
            ArgumentNullException.ThrowIfNull(errorException);

            _errorException = errorException;
        }

        /// <summary>
        /// The exception that describes the media failure.
        /// </summary>
        public Exception ErrorException
        {
            get
            {
                return _errorException;
            }
        }

        private Exception _errorException;
    };

    #endregion
} // namespace System.Windows
