using UnityEngine;
using TMPro;

public class PlanetDetailMode : MonoBehaviour
{
    [SerializeField] private GameObject solarSystem;

    [Header("Welcome Screen")]
    [SerializeField] private TMP_Text welcomeText;

    [Header("Planet Detail Modes")]
    [SerializeField] private GameObject mercuryDetailMode;
    [SerializeField] private GameObject venusDetailMode;
    [SerializeField] private GameObject earthDetailMode;
    [SerializeField] private GameObject marsDetailMode;
    [SerializeField] private GameObject jupiterDetailMode;
    [SerializeField] private GameObject saturnDetailMode;
    [SerializeField] private GameObject uranusDetailMode;
    [SerializeField] private GameObject neptuneDetailMode;
    [SerializeField] private GameObject sunDetailMode;

    [Header("Audio")]
    [SerializeField] private PlanetTypeAudio planetTypeAudio;
    [SerializeField] private AudioSource solarSystemAudio;
    [SerializeField] private AudioSource comparisonAudio;
    [SerializeField] private AudioSource orbitsAudio;


    [Header("Positioning")]
    [SerializeField] private Transform vrCamera;
    [SerializeField] private float distance = 2.5f;
    [SerializeField] private float comparisonDistance = 3.5f;
    [SerializeField] private float comparisonHeightOffset = -2.0f;

    [Header("Comparison")]
    [SerializeField] private Transform comparisonRoot;
    [SerializeField] private Transform leftPosition;
    [SerializeField] private Transform rightPosition;
    [SerializeField] private GameObject divider;
    [SerializeField] private GameObject comparisonBackCanvas;
    [SerializeField] private GameObject[] detailButtons;

    private GameObject activeDetailMode;
    private GameObject firstPlanet;
    private GameObject secondPlanet;
    private bool selectingSecondPlanet;
    private bool isComparing;

    private void Start()
    {
        CloseAllDetailModes();
        SetComparisonUI(false);
        SetDetailButtons(true);
        PlayBackgroundAudio(solarSystemAudio);
    }

    private void CloseAllDetailModes()
    {
        SetDetailModeActive(mercuryDetailMode, false);
        SetDetailModeActive(venusDetailMode, false);
        SetDetailModeActive(earthDetailMode, false);
        SetDetailModeActive(marsDetailMode, false);
        SetDetailModeActive(jupiterDetailMode, false);
        SetDetailModeActive(saturnDetailMode, false);
        SetDetailModeActive(uranusDetailMode, false);
        SetDetailModeActive(neptuneDetailMode, false);
        SetDetailModeActive(sunDetailMode, false);
    }

    private void SetDetailModeActive(GameObject detailMode, bool active)
    {
        if (detailMode != null)
            detailMode.SetActive(active);
    }

    private GameObject GetDetailMode(string planetName)
    {
        switch (planetName.ToLowerInvariant())
        {
            case "mercury": return mercuryDetailMode;
            case "venus": return venusDetailMode;
            case "earth": return earthDetailMode;
            case "mars": return marsDetailMode;
            case "jupiter": return jupiterDetailMode;
            case "saturn": return saturnDetailMode;
            case "uranus": return uranusDetailMode;
            case "neptune": return neptuneDetailMode;
            case "sun": return sunDetailMode;
            default:
                Debug.LogWarning("Unknown planet: " + planetName);
                return null;
        }
    }

    public void OpenDetails(string planetName)
    {
        if (isComparing || string.IsNullOrWhiteSpace(planetName))
            return;

        GameObject selectedPlanet = GetDetailMode(planetName);
        if (selectedPlanet == null)
        {
            Debug.LogWarning("Detail mode missing for: " + planetName);
            return;
        }

        if (selectingSecondPlanet)
        {
            if (selectedPlanet == firstPlanet)
            {
                Debug.LogWarning("Choose a different planet for comparison.");
                return;
            }

            secondPlanet = selectedPlanet;
            OpenComparison();
            return;
        }

        CloseAllDetailModes();
        SetDetailButtons(true);
        SetComparisonUI(false);

        GetViewPose(out Vector3 center, out Quaternion rotation);
        selectedPlanet.transform.SetPositionAndRotation(center, rotation);

        solarSystem.SetActive(false);
        selectedPlanet.SetActive(true);
        activeDetailMode = selectedPlanet;
        firstPlanet = selectedPlanet;
        secondPlanet = null;

        StopAllAudio();
        PlayPlanetAudio(planetName);

    }

    
    public void StartComparison()
    {
        if (activeDetailMode == null || isComparing || selectingSecondPlanet)
            return;

        firstPlanet = activeDetailMode;
        selectingSecondPlanet = true;
        welcomeText.text = "Select your 2nd planet for comparison";
        PlayBackgroundAudio(solarSystemAudio);
        CloseAllDetailModes();
        activeDetailMode = null;
        solarSystem.SetActive(true);
    }

    private void OpenComparison()
    {
        if (firstPlanet == null || secondPlanet == null ||
            comparisonRoot == null || leftPosition == null || rightPosition == null)
        {
            Debug.LogError("Comparison references are missing on PlanetDetailMode.");
            secondPlanet = null;
            return;
        }

        GetViewPose(out Vector3 center, out Quaternion rotation, comparisonDistance);
        center.y += comparisonHeightOffset;
        comparisonRoot.SetPositionAndRotation(center, rotation);

        CloseAllDetailModes();
        PlayBackgroundAudio(comparisonAudio);
        solarSystem.SetActive(false);

        firstPlanet.transform.SetPositionAndRotation(leftPosition.position, rotation);
        secondPlanet.transform.SetPositionAndRotation(rightPosition.position, rotation);

        firstPlanet.SetActive(true);
        secondPlanet.SetActive(true);

        SetDetailButtons(false);
        SetComparisonUI(true);

        activeDetailMode = null;
        selectingSecondPlanet = false;
        isComparing = true;
    }

    private void GetViewPose(
    out Vector3 center,
    out Quaternion rotation,
    float customDistance = -1f)
    {
        Vector3 forward = vrCamera.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude < 0.0001f)
            forward = Vector3.forward;

        forward.Normalize();

        float actualDistance = customDistance > 0f
            ? customDistance
            : distance;

        center = vrCamera.position + forward * actualDistance;
        rotation = Quaternion.LookRotation(forward, Vector3.up);
    }

    private void SetComparisonUI(bool visible)
    {
        if (divider != null)
            divider.SetActive(visible);
        if (comparisonBackCanvas != null)
            comparisonBackCanvas.SetActive(visible);
    }

    private void SetDetailButtons(bool visible)
    {
        if (detailButtons == null)
            return;

        foreach (GameObject button in detailButtons)
        {
            if (button != null)
                button.SetActive(visible);
        }
    }

    private void PlayPlanetAudio(string planetName)
    {
        if (planetTypeAudio == null || planetTypeAudio.audioSource == null)
            return;

        switch (planetName.ToLowerInvariant())
        {
            case "mercury":
            case "venus":
            case "earth":
            case "mars":
                planetTypeAudio.PlayPlanetSound(PlanetTypeAudio.PlanetType.Rocky);
                break;
            case "jupiter":
            case "saturn":
                planetTypeAudio.PlayPlanetSound(PlanetTypeAudio.PlanetType.Gas);
                break;
            case "uranus":
            case "neptune":
                planetTypeAudio.PlayPlanetSound(PlanetTypeAudio.PlanetType.Ice);
                break;
            case "sun":
                planetTypeAudio.PlayPlanetSound(PlanetTypeAudio.PlanetType.Sun);
                break;
        }
    }
    
    public void CloseDetails()
    {
        CloseAllDetailModes();
        PlayBackgroundAudio(solarSystemAudio);
        activeDetailMode = null;
        firstPlanet = null;
        secondPlanet = null;
        selectingSecondPlanet = false;
        isComparing = false;
        SetDetailButtons(true);
        SetComparisonUI(false);
        solarSystem.SetActive(true);
        welcomeText.text = "Select a planet in Planetly"; 
    }

    public void StopAllAudio()
    {
        if (solarSystemAudio != null)
            solarSystemAudio.Stop();

        if (comparisonAudio != null)
            comparisonAudio.Stop();

        if (orbitsAudio != null)
            orbitsAudio.Stop();

        if (planetTypeAudio != null &&
            planetTypeAudio.audioSource != null)
        {
            planetTypeAudio.audioSource.Stop();
        }
    }

    private void PlayBackgroundAudio(AudioSource source)
    {
        StopAllAudio();

        if (source != null && source.clip != null)
        {
            source.loop = true;
            source.Play();
        }
    }
}
