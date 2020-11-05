using System;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Represents a participant (i.e., another device in a collaborative session).
    /// </summary>
    /// <remarks>
    /// Generated by the <see cref="ARParticipantManager"/> to represent another participant in the session.
    /// </remarks>
    // [DefaultExecutionOrder(ARUpdateOrder.k_Plane)]
    [DisallowMultipleComponent]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@3.1/api/UnityEngine.XR.ARFoundation.ARParticipant.html")]
    public sealed class ARParticipant : ARTrackable<XRParticipant, ARParticipant>
    {
        /// <summary>
        /// Get a native pointer associated with this participant.
        /// </summary>
        /// <remarks>
        /// The data pointed to by this member is implementation defined.
        /// The lifetime of the pointed to object is also
        /// implementation defined, but should be valid at least until the next
        /// <see cref="ARSession"/> update.
        /// </remarks>
        public IntPtr nativePtr { get { return sessionRelativeData.nativePtr; } }

        /// <summary>
        /// Get this participant's session identifier.
        /// </summary>
        public Guid sessionId { get { return sessionRelativeData.sessionId; } }
    }
}
