﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
//
// This file was generated, please do not edit it directly.
//
// Please see MilCodeGen.html for more information.
//

using System.Windows.Media.Composition;
// These types are aliased to match the unamanaged names used in interop

namespace System.Windows.Media.Media3D
{
    sealed partial class PerspectiveCamera : ProjectionCamera
    {
        //------------------------------------------------------
        //
        //  Public Methods
        //
        //------------------------------------------------------

        #region Public Methods

        /// <summary>
        ///     Shadows inherited Clone() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new PerspectiveCamera Clone()
        {
            return (PerspectiveCamera)base.Clone();
        }

        /// <summary>
        ///     Shadows inherited CloneCurrentValue() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new PerspectiveCamera CloneCurrentValue()
        {
            return (PerspectiveCamera)base.CloneCurrentValue();
        }




        #endregion Public Methods

        //------------------------------------------------------
        //
        //  Public Properties
        //
        //------------------------------------------------------

        private static void FieldOfViewPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PerspectiveCamera target = ((PerspectiveCamera)d);


            target.PropertyChanged(FieldOfViewProperty);
        }


        #region Public Properties

        /// <summary>
        ///     FieldOfView - double.  Default value is (double)45.0.
        /// </summary>
        public double FieldOfView
        {
            get
            {
                return (double)GetValue(FieldOfViewProperty);
            }
            set
            {
                SetValueInternal(FieldOfViewProperty, value);
            }
        }

        #endregion Public Properties

        //------------------------------------------------------
        //
        //  Protected Methods
        //
        //------------------------------------------------------

        #region Protected Methods

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new PerspectiveCamera();
        }



        #endregion ProtectedMethods

        //------------------------------------------------------
        //
        //  Internal Methods
        //
        //------------------------------------------------------

        #region Internal Methods

        internal override void UpdateResource(DUCE.Channel channel, bool skipOnChannelCheck)
        {
            // If we're told we can skip the channel check, then we must be on channel
            Debug.Assert(!skipOnChannelCheck || _duceResource.IsOnChannel(channel));

            if (skipOnChannelCheck || _duceResource.IsOnChannel(channel))
            {
                base.UpdateResource(channel, skipOnChannelCheck);

                // Read values of properties into local variables
                Transform3D vTransform = Transform;

                // Obtain handles for properties that implement DUCE.IResource
                DUCE.ResourceHandle hTransform;
                if (vTransform == null ||
                    Object.ReferenceEquals(vTransform, Transform3D.Identity)
                    )
                {
                    hTransform = DUCE.ResourceHandle.Null;
                }
                else
                {
                    hTransform = ((DUCE.IResource)vTransform).GetHandle(channel);
                }

                // Obtain handles for animated properties
                DUCE.ResourceHandle hNearPlaneDistanceAnimations = GetAnimationResourceHandle(NearPlaneDistanceProperty, channel);
                DUCE.ResourceHandle hFarPlaneDistanceAnimations = GetAnimationResourceHandle(FarPlaneDistanceProperty, channel);
                DUCE.ResourceHandle hPositionAnimations = GetAnimationResourceHandle(PositionProperty, channel);
                DUCE.ResourceHandle hLookDirectionAnimations = GetAnimationResourceHandle(LookDirectionProperty, channel);
                DUCE.ResourceHandle hUpDirectionAnimations = GetAnimationResourceHandle(UpDirectionProperty, channel);
                DUCE.ResourceHandle hFieldOfViewAnimations = GetAnimationResourceHandle(FieldOfViewProperty, channel);

                // Pack & send command packet
                DUCE.MILCMD_PERSPECTIVECAMERA data;
                unsafe
                {
                    data.Type = MILCMD.MilCmdPerspectiveCamera;
                    data.Handle = _duceResource.GetHandle(channel);
                    data.htransform = hTransform;
                    if (hNearPlaneDistanceAnimations.IsNull)
                    {
                        data.nearPlaneDistance = NearPlaneDistance;
                    }
                    data.hNearPlaneDistanceAnimations = hNearPlaneDistanceAnimations;
                    if (hFarPlaneDistanceAnimations.IsNull)
                    {
                        data.farPlaneDistance = FarPlaneDistance;
                    }
                    data.hFarPlaneDistanceAnimations = hFarPlaneDistanceAnimations;
                    if (hPositionAnimations.IsNull)
                    {
                        data.position = CompositionResourceManager.Point3DToMilPoint3F(Position);
                    }
                    data.hPositionAnimations = hPositionAnimations;
                    if (hLookDirectionAnimations.IsNull)
                    {
                        data.lookDirection = CompositionResourceManager.Vector3DToMilPoint3F(LookDirection);
                    }
                    data.hLookDirectionAnimations = hLookDirectionAnimations;
                    if (hUpDirectionAnimations.IsNull)
                    {
                        data.upDirection = CompositionResourceManager.Vector3DToMilPoint3F(UpDirection);
                    }
                    data.hUpDirectionAnimations = hUpDirectionAnimations;
                    if (hFieldOfViewAnimations.IsNull)
                    {
                        data.fieldOfView = FieldOfView;
                    }
                    data.hFieldOfViewAnimations = hFieldOfViewAnimations;

                    // Send packed command structure
                    channel.SendCommand(
                        (byte*)&data,
                        sizeof(DUCE.MILCMD_PERSPECTIVECAMERA));
                }
            }
        }
        internal override DUCE.ResourceHandle AddRefOnChannelCore(DUCE.Channel channel)
        {
            if (_duceResource.CreateOrAddRefOnChannel(this, channel, System.Windows.Media.Composition.DUCE.ResourceType.TYPE_PERSPECTIVECAMERA))
            {
                Transform3D vTransform = Transform;
                if (vTransform != null)
                    ((DUCE.IResource)vTransform).AddRefOnChannel(channel);

                AddRefOnChannelAnimations(channel);


                UpdateResource(channel, true /* skip "on channel" check - we already know that we're on channel */ );
            }

            return _duceResource.GetHandle(channel);
        }
        internal override void ReleaseOnChannelCore(DUCE.Channel channel)
        {
            Debug.Assert(_duceResource.IsOnChannel(channel));

            if (_duceResource.ReleaseOnChannel(channel))
            {
                Transform3D vTransform = Transform;
                if (vTransform != null)
                    ((DUCE.IResource)vTransform).ReleaseOnChannel(channel);

                ReleaseOnChannelAnimations(channel);
            }
        }
        internal override DUCE.ResourceHandle GetHandleCore(DUCE.Channel channel)
        {
            // Note that we are in a lock here already.
            return _duceResource.GetHandle(channel);
        }
        internal override int GetChannelCountCore()
        {
            // must already be in composition lock here
            return _duceResource.GetChannelCount();
        }
        internal override DUCE.Channel GetChannelCore(int index)
        {
            // Note that we are in a lock here already.
            return _duceResource.GetChannel(index);
        }


        #endregion Internal Methods

        //------------------------------------------------------
        //
        //  Internal Properties
        //
        //------------------------------------------------------

        #region Internal Properties





        #endregion Internal Properties

        //------------------------------------------------------
        //
        //  Dependency Properties
        //
        //------------------------------------------------------

        #region Dependency Properties

        /// <summary>
        ///     The DependencyProperty for the PerspectiveCamera.FieldOfView property.
        /// </summary>
        public static readonly DependencyProperty FieldOfViewProperty;

        #endregion Dependency Properties

        //------------------------------------------------------
        //
        //  Internal Fields
        //
        //------------------------------------------------------

        #region Internal Fields



        internal System.Windows.Media.Composition.DUCE.MultiChannelResource _duceResource = new System.Windows.Media.Composition.DUCE.MultiChannelResource();

        internal const double c_FieldOfView = (double)45.0;

        #endregion Internal Fields



        #region Constructors

        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        static PerspectiveCamera()
        {
            // We check our static default fields which are of type Freezable
            // to make sure that they are not mutable, otherwise we will throw
            // if these get touched by more than one thread in the lifetime
            // of your app.



            // Initializations
            Type typeofThis = typeof(PerspectiveCamera);
            FieldOfViewProperty =
                  RegisterProperty("FieldOfView",
                                   typeof(double),
                                   typeofThis,
                                   (double)45.0,
                                   new PropertyChangedCallback(FieldOfViewPropertyChanged),
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
        }

        #endregion Constructors
    }
}
