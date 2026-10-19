using TMPro;
using UnityEngine;

public class RealisticSolarSystemController : MonoBehaviour
{
    [Header("Modes")]
    [SerializeField] private GameObject normalSolarSystem;
    [SerializeField] private GameObject realisticSolarSystem;
    [SerializeField] private GameObject realisticUI;
    [SerializeField] private Transform vrCamera;

    [Header("Orbit Simulation")]
    [SerializeField] private RealisticPlanet[] planets;
    [SerializeField] private float earthDaysPerSecond = 10f;
    [SerializeField] private TMP_Text dayLabel;
    [SerializeField] private TMP_Text playPauseLabel;
    [SerializeField] private TMP_Text speedLabel;

    [Header("VR Overview")]
    [SerializeField] private Transform overviewRoot;
    [SerializeField] private float displayScale = 10f; 
    [SerializeField] private float overviewDistance = 30f; 
    [SerializeField] private float overviewHeight = 0f; 

    [Header("Audio")]
    [SerializeField] private PlanetDetailMode planetDetailMode;
    [SerializeField] private AudioSource orbitsAudio;
    [SerializeField] private AudioSource solarSystemAudio;

    private bool running;
    private bool realisticModeActive;
    private float elapsedEarthDays;

    private void Start()
    {
        realisticSolarSystem.SetActive(false);
        if (realisticUI != null) realisticUI.SetActive(false);
        UpdateUI();
    }

    private void Update()
    {
        if (!realisticModeActive || !running) return;
        float days = Time.deltaTime * earthDaysPerSecond;
        elapsedEarthDays += days;
        foreach (RealisticPlanet planet in planets)
            if (planet != null) planet.AdvanceDays(days);
        UpdateUI();
    }

    public void OpenRealisticMode()
    {
        

        if (vrCamera == null || realisticSolarSystem == null || normalSolarSystem == null) return;

        // Scale and position the whole realistic system for VR viewing.
        // Distances and sizes remain proportional inside the system.
        // overviewRoot must be the parent of the Sun and all planets, not the UI.
        if (overviewRoot != null)
        {
            Vector3 forward = vrCamera.forward;
            forward.y = 0f;
            if (forward.sqrMagnitude < 0.0001f) forward = Vector3.forward;
            forward.Normalize();
            overviewRoot.localScale = Vector3.one * displayScale;
            overviewRoot.SetPositionAndRotation(
                vrCamera.position + forward * overviewDistance + Vector3.up * overviewHeight,
                Quaternion.LookRotation(forward, Vector3.up) * Quaternion.Euler(0f, 0f, 0f)); // 35f, 0f, 0fso it is tilted down a bit for better viewing of the orbits
        }

        

        normalSolarSystem.SetActive(false);
        realisticSolarSystem.SetActive(true);
        if (realisticUI != null) realisticUI.SetActive(true);

        elapsedEarthDays = 0f;
        foreach (RealisticPlanet planet in planets)
            if (planet != null) planet.Initialize();

        realisticModeActive = true;
        running = true;
        UpdateUI();

        planetDetailMode.StopAllAudio();

        if (orbitsAudio != null)
        {
            orbitsAudio.loop = true;
            orbitsAudio.Play();
        }
    }

    public void TogglePause()
    {
        if (!realisticModeActive) return;
        running = !running;
        UpdateUI();
    }

    public void CloseRealisticMode()
    {
        running = false;
        realisticModeActive = false;
        realisticSolarSystem.SetActive(false);
        if (realisticUI != null) realisticUI.SetActive(false);
        normalSolarSystem.SetActive(true);
        if (orbitsAudio != null)
            orbitsAudio.Stop();

        if (solarSystemAudio != null)
        {
            solarSystemAudio.loop = true;
            solarSystemAudio.Play();
        }
    }

    public void AdjustSimulationSpeed(float change)
    {
        if (!realisticModeActive) return;

        earthDaysPerSecond = Mathf.Clamp(
            earthDaysPerSecond + change,
            0f,
            1000f
        );

        UpdateUI();
    }

    private void UpdateUI()
    {
        int day = Mathf.FloorToInt(Mathf.Repeat(elapsedEarthDays, 365f)) + 1;
        if (dayLabel != null) dayLabel.text = $"Day {day} / 365";
        if (playPauseLabel != null) playPauseLabel.text = running ? "Pause" : "Play";
        if (speedLabel != null)
            speedLabel.text = $"Speed: {earthDaysPerSecond:F1} Earth days/s";
    }
}
