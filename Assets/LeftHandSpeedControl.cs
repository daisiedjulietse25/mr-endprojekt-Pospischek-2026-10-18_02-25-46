
using UnityEngine;

public class LeftHandSpeedControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private RealisticSolarSystemController controller;

    [Header("Settings")]
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float deadZone = 2f;

    private Quaternion previousRotation;
    private bool initialized;

    private void Update()
    {
        if (leftHand == null || controller == null)
            return;

        if (!initialized)
        {
            previousRotation = leftHand.rotation;
            initialized = true;
            return;
        }

        // Rotation since the previous frame
        Quaternion deltaRotation =
            leftHand.rotation * Quaternion.Inverse(previousRotation);

        deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);

        if (angle > 180f)
            angle -= 360f;

        // Twist around the hand's forward axis
        float twist = angle * Vector3.Dot(axis, leftHand.forward);

        if (Mathf.Abs(twist) > deadZone)
        {
            controller.AdjustSimulationSpeed(twist * sensitivity);
        }

        previousRotation = leftHand.rotation;
    }
}
