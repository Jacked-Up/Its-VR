// This script was updated on 02/08/2022 by Jack Randolph.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItsVR.Core {
    /// <summary>
    /// Commonly used VR related calculations.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    public static class ItsMath {
        /// <summary>
        /// Calculates the velocity of an object.
        /// </summary>
        /// <param name="thisFramePosition">The position of the object this frame.</param>
        /// <param name="lastFramePosition">The position of the object last frame.</param>
        /// <returns>The calculated velocity.</returns>
        public static Vector3 Velocity(Vector3 thisFramePosition, Vector3 lastFramePosition) {
            return thisFramePosition - lastFramePosition;
        }

        /// <summary>
        /// Calculates the angular velocity of an object.
        /// </summary>
        /// <param name="thisFrameRotation">The rotation of the object this frame.</param>
        /// <param name="lastFrameRotation">The rotation of the object last frame.</param>
        /// <returns>The calculated angular velocity.</returns>
        public static Vector3 AngularVelocity(Quaternion thisFrameRotation, Quaternion lastFrameRotation) {
            var difference = thisFrameRotation * Quaternion.Inverse(lastFrameRotation);
            difference.ToAngleAxis(out var degrees, out var axis);
            var degreesDifference = degrees * axis;

            return degreesDifference * Mathf.Deg2Rad;
        }
        
        /// <summary>
        /// Returns the center point of all the vectors.
        /// </summary>
        /// <param name="allVectors">A list of all the vectors to calculate the mid point of.</param>
        /// <returns>The middle vector between all of the vectors.</returns>
        public static Vector3 MiddlePoint(List<Vector3> allVectors) {
            if (allVectors.Count == 0)
                return Vector3.zero;

            var sum = Vector3.zero;
            sum = allVectors.Aggregate(sum, (current, vector) => current + vector);
            return sum / allVectors.Count;
        }
    }
}
