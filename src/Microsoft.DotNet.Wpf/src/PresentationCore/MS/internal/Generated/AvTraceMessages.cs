﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// NOTE: This file was generated by $(WpfCodeGenDir)AvTrace\AvTraceMessages.tt.
// Any manual updates to this file will overwritten.

namespace MS.Internal
{
    static internal partial class TraceRoutedEvent
    {
        static private AvTrace _avTrace = new AvTrace(
                delegate() { return PresentationTraceSources.RoutedEventSource; },
                delegate() { PresentationTraceSources._RoutedEventSource = null; }
                );

		static AvTraceDetails _RaiseEvent;
		static public AvTraceDetails RaiseEvent
        {
            get
            {
                if ( _RaiseEvent == null )
                {
                    _RaiseEvent = new AvTraceDetails(1, new string[] { "Raise RoutedEvent" } );
                }

                return _RaiseEvent;
            }
        }

		static AvTraceDetails _ReRaiseEventAs;
		static public AvTraceDetails ReRaiseEventAs
        {
            get
            {
                if ( _ReRaiseEventAs == null )
                {
                    _ReRaiseEventAs = new AvTraceDetails(2, new string[] { "Raise RoutedEvent" } );
                }

                return _ReRaiseEventAs;
            }
        }

		static AvTraceDetails _HandleEvent;
		static public AvTraceDetails HandleEvent
        {
            get
            {
                if ( _HandleEvent == null )
                {
                    _HandleEvent = new AvTraceDetails(3, new string[] { "RoutedEvent has set Handled" } );
                }

                return _HandleEvent;
            }
        }

		static AvTraceDetails _InvokeHandlers;
		static public AvTraceDetails InvokeHandlers
        {
            get
            {
                if ( _InvokeHandlers == null )
                {
                    _InvokeHandlers = new AvTraceDetails(4, new string[] { "InvokeHandlers" } );
                }

                return _InvokeHandlers;
            }
        }

        /// <summary> Send a single trace output </summary>
        static public void Trace( TraceEventType type, AvTraceDetails traceDetails, params object[] parameters )
        {
            _avTrace.Trace( type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters );
        }

        /// <summary> These help delay allocation of object array </summary>
        static public void Trace( TraceEventType type, AvTraceDetails traceDetails )
        {
            _avTrace.Trace( type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, Array.Empty<object>() );
        }
        static public void Trace( TraceEventType type, AvTraceDetails traceDetails, object p1 )
        {
            _avTrace.Trace( type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1 } );
        }
        static public void Trace( TraceEventType type, AvTraceDetails traceDetails, object p1, object p2 )
        {
            _avTrace.Trace( type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1, p2 } );
        }
        static public void Trace( TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3 )
        {
            _avTrace.Trace( type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1, p2, p3 } );
        }

        /// <summary> Send a singleton "activity" trace (really, this sends the same trace as both a Start and a Stop) </summary>
        static public void TraceActivityItem( AvTraceDetails traceDetails, params Object[] parameters )
        {
            _avTrace.TraceStartStop( traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters );
        }

        /// <summary> These help delay allocation of object array </summary>
        static public void TraceActivityItem( AvTraceDetails traceDetails )
        {
            _avTrace.TraceStartStop( traceDetails.Id, traceDetails.Message, traceDetails.Labels, Array.Empty<object>() );
        }
        static public void TraceActivityItem( AvTraceDetails traceDetails, object p1 )
        {
            _avTrace.TraceStartStop( traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1 } );
        }
        static public void TraceActivityItem( AvTraceDetails traceDetails, object p1, object p2 )
        {
            _avTrace.TraceStartStop( traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1, p2 } );
        }
        static public void TraceActivityItem( AvTraceDetails traceDetails, object p1, object p2, object p3 )
        {
            _avTrace.TraceStartStop( traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1, p2, p3 } );
        }

        static public bool IsEnabled
        {
            get { return _avTrace != null && _avTrace.IsEnabled; }
        }

        /// <summary> Is there a Tracesource?  (See comment on AvTrace.IsEnabledOverride.) </summary>
        static public bool IsEnabledOverride
        {
            get { return _avTrace.IsEnabledOverride; }
        }

        /// <summary> Re-read the configuration for this trace source </summary>
        static public void Refresh()
        {
            _avTrace.Refresh();
        }
	}
    static internal partial class TraceAnimation
    {
        static private AvTrace _avTrace = new AvTrace(
                delegate() { return PresentationTraceSources.AnimationSource; },
                delegate() { PresentationTraceSources._AnimationSource = null; }
                );

		static AvTraceDetails _StoryboardBegin;
		static public AvTraceDetails StoryboardBegin
        {
            get
            {
                if ( _StoryboardBegin == null )
                {
                    _StoryboardBegin = new AvTraceDetails(1, new string[] { "Storyboard has begun" } );
                }

                return _StoryboardBegin;
            }
        }

		static AvTraceDetails _StoryboardPause;
		static public AvTraceDetails StoryboardPause
        {
            get
            {
                if ( _StoryboardPause == null )
                {
                    _StoryboardPause = new AvTraceDetails(2, new string[] { "Storyboard has been paused" } );
                }

                return _StoryboardPause;
            }
        }

		static AvTraceDetails _StoryboardRemove;
		static public AvTraceDetails StoryboardRemove
        {
            get
            {
                if ( _StoryboardRemove == null )
                {
                    _StoryboardRemove = new AvTraceDetails(3, new string[] { "Storyboard has been removed" } );
                }

                return _StoryboardRemove;
            }
        }

		static AvTraceDetails _StoryboardResume;
		static public AvTraceDetails StoryboardResume
        {
            get
            {
                if ( _StoryboardResume == null )
                {
                    _StoryboardResume = new AvTraceDetails(4, new string[] { "Storyboard has been resumed" } );
                }

                return _StoryboardResume;
            }
        }

		static AvTraceDetails _StoryboardStop;
		static public AvTraceDetails StoryboardStop
        {
            get
            {
                if ( _StoryboardStop == null )
                {
                    _StoryboardStop = new AvTraceDetails(5, new string[] { "Storyboard has been stopped" } );
                }

                return _StoryboardStop;
            }
        }

		static AvTraceDetails _StoryboardNotApplied;
		static public AvTraceDetails StoryboardNotApplied
        {
            get
            {
                if ( _StoryboardNotApplied == null )
                {
                    _StoryboardNotApplied = new AvTraceDetails(6, new string[] { "Unable to perform action because the specified Storyboard was never applied to this object for interactive control." } );
                }

                return _StoryboardNotApplied;
            }
        }

		static AvTraceDetails _AnimateStorageValidationFailed;
		static public AvTraceDetails AnimateStorageValidationFailed
        {
            get
            {
                if ( _AnimateStorageValidationFailed == null )
                {
                    _AnimateStorageValidationFailed = new AvTraceDetails(7, new string[] { "Animated property failed validation. Animated value not set." } );
                }

                return _AnimateStorageValidationFailed;
            }
        }

		static AvTraceDetails _AnimateStorageValidationNoLongerFailing;
		static public AvTraceDetails AnimateStorageValidationNoLongerFailing
        {
            get
            {
                if ( _AnimateStorageValidationNoLongerFailing == null )
                {
                    _AnimateStorageValidationNoLongerFailing = new AvTraceDetails(8, new string[] { "Animated property no longer failing validation." } );
                }

                return _AnimateStorageValidationNoLongerFailing;
            }
        }

        /// <summary> Send a single trace output </summary>
        static public void Trace( TraceEventType type, AvTraceDetails traceDetails, params object[] parameters )
        {
            _avTrace.Trace( type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters );
        }

        /// <summary> These help delay allocation of object array </summary>
        static public void Trace( TraceEventType type, AvTraceDetails traceDetails )
        {
            _avTrace.Trace( type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, Array.Empty<object>() );
        }
        static public void Trace( TraceEventType type, AvTraceDetails traceDetails, object p1 )
        {
            _avTrace.Trace( type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1 } );
        }
        static public void Trace( TraceEventType type, AvTraceDetails traceDetails, object p1, object p2 )
        {
            _avTrace.Trace( type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1, p2 } );
        }
        static public void Trace( TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3 )
        {
            _avTrace.Trace( type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1, p2, p3 } );
        }

        /// <summary> Send a singleton "activity" trace (really, this sends the same trace as both a Start and a Stop) </summary>
        static public void TraceActivityItem( AvTraceDetails traceDetails, params Object[] parameters )
        {
            _avTrace.TraceStartStop( traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters );
        }

        /// <summary> These help delay allocation of object array </summary>
        static public void TraceActivityItem( AvTraceDetails traceDetails )
        {
            _avTrace.TraceStartStop( traceDetails.Id, traceDetails.Message, traceDetails.Labels, Array.Empty<object>() );
        }
        static public void TraceActivityItem( AvTraceDetails traceDetails, object p1 )
        {
            _avTrace.TraceStartStop( traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1 } );
        }
        static public void TraceActivityItem( AvTraceDetails traceDetails, object p1, object p2 )
        {
            _avTrace.TraceStartStop( traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1, p2 } );
        }
        static public void TraceActivityItem( AvTraceDetails traceDetails, object p1, object p2, object p3 )
        {
            _avTrace.TraceStartStop( traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[] { p1, p2, p3 } );
        }

        static public bool IsEnabled
        {
            get { return _avTrace != null && _avTrace.IsEnabled; }
        }

        /// <summary> Is there a Tracesource?  (See comment on AvTrace.IsEnabledOverride.) </summary>
        static public bool IsEnabledOverride
        {
            get { return _avTrace.IsEnabledOverride; }
        }

        /// <summary> Re-read the configuration for this trace source </summary>
        static public void Refresh()
        {
            _avTrace.Refresh();
        }
	}
}
