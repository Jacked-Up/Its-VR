// This script was updated on 11/11/2021 by Jack Randolph.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItsVR {
    /// <summary>
    /// Commonly used VR related calculations.
    /// </summary>
    public static class ItsMath {
        /// <summary>
        /// Calculates the velocity of an object.
        /// </summary>
        /// <param name="thisFramePosition">The position of the object this frame.</param>
        /// <param name="lastFramePosition">The position of the object last frame.</param>
        /// <param name="overDeltaTime">If the velocity should be calculated over delta time.</param>
        /// <returns>The calculated velocity.</returns>
        public static Vector3 Velocity(Vector3 thisFramePosition, Vector3 lastFramePosition, bool overDeltaTime = true) {
            return thisFramePosition - lastFramePosition / (overDeltaTime ? Time.deltaTime : 1f);
        }

        /// <summary>
        /// Calculates the angular velocity of an object.
        /// </summary>
        /// <param name="thisFrameRotation">The rotation of the object this frame.</param>
        /// <param name="lastFrameRotation">The rotation of the object last frame.</param>
        /// <param name="overDeltaTime">If the angular velocity should be calculated over delta time.</param>
        /// <returns>The calculated angular velocity.</returns>
        public static Vector3 AngularVelocity(Quaternion thisFrameRotation, Quaternion lastFrameRotation, bool overDeltaTime = true) {
            var q = lastFrameRotation * Quaternion.Inverse(thisFrameRotation);
            
            if(Mathf.Abs(q.w) > 1023.5f / 1024.0f)
                return new Vector3(0,0,0);
            
            float gain;
            
            if(q.w < 0.0f) {
                var angle = Mathf.Acos(-q.w);
                gain = -2.0f * angle / (Mathf.Sin(angle)*Time.deltaTime);
            }
            else {
                var angle = Mathf.Acos(q.w);
                gain = 2.0f * angle / (Mathf.Sin(angle)*Time.deltaTime);
            }
            
            return -new Vector3(q.x * gain,q.y * gain,q.z * gain) / (overDeltaTime ? Time.deltaTime : 1f);
        }
        
        /// <summary>
        /// Returns the center point of all the vectors.
        /// </summary>
        /// <param name="vectors">A list of all the vectors to calculate the mid point of.</param>
        /// <returns>The middle vector between all of the vectors.</returns>
        public static Vector3 MiddlePoint(List<Vector3> vectors) {
            // Bail if no vectors were input.
            if (vectors.Count == 0)
                return Vector3.zero;

            var sum = Vector3.zero;
            sum = vectors.Aggregate(sum, (current, vector) => current + vector);
            return sum / vectors.Count;
        }
    }
}
