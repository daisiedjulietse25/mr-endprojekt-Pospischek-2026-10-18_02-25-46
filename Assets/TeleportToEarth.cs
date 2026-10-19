using UnityEngine;

public class TeleportToEarth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform xrOrigin;
    [SerializeField] private Transform vrCamera;
    [SerializeField] private Transform earth;

    [Header("Teleport Settings")]
    [SerializeField] private float distanceFromEarth = 3f;

    public void Teleport()
    {
        if (xrOrigin == null || vrCamera == null || earth == null)
        {
            Debug.LogWarning("TeleportToEarth: References are missing!");
            return;
        }

        // Current position of the moving Earth
        Vector3 earthPosition = earth.position;

        // Direction from Earth toward the player's current position
        Vector3 direction = vrCamera.position - earthPosition;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            direction = Vector3.forward;

        direction.Normalize();

        // Position where the player's head should end up
        Vector3 targetHeadPosition =
            earthPosition + direction * distanceFromEarth;

        // Move XR Origin so that the VR camera reaches that position
        Vector3 offset = targetHeadPosition - vrCamera.position;
        xrOrigin.position += offset;
    }
}