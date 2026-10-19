using UnityEngine;

public class RealisticPlanet : MonoBehaviour
{
    [Header("Values from assignment")]
    public float radius;
    public float distanceToSun;
    public float orbitalPeriodYears = 1f;
    public Transform sun;

    [Header("Mesh sizing")]
    [Tooltip("For a Unity sphere of diameter 1, set to 2 so radius corresponds to world units.")]
    public float diameterMultiplier = 2f;

    private float angleDegrees;

    public void Initialize()
    {
        transform.localScale = Vector3.one * (radius * diameterMultiplier);
        angleDegrees = 0f;
        UpdatePosition();
    }

    public void AdvanceDays(float days)
    {
        if (sun == null || distanceToSun <= 0f || orbitalPeriodYears <= 0f) return;
        angleDegrees = Mathf.Repeat(angleDegrees + 360f * days / (365f * orbitalPeriodYears), 360f);
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (sun == null) return;
        float radians = angleDegrees * Mathf.Deg2Rad;
        transform.localPosition = sun.localPosition + new Vector3(
            Mathf.Cos(radians) * distanceToSun, 0f,
            Mathf.Sin(radians) * distanceToSun);
    }
}
