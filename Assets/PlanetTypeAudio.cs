
using UnityEngine;

public class PlanetTypeAudio : MonoBehaviour
{
    public enum PlanetType
    {
        Rocky,
        Gas,
        Sun,
        Ice
    }

    public AudioSource audioSource;

    public AudioClip rockyPlanetSound;
    public AudioClip gasPlanetSound;
    public AudioClip icePlanetSound;
    public AudioClip sunPlanetSound;

    public void PlayPlanetSound(PlanetType type)
    {
        AudioClip selectedSound = null;

        switch (type)
        {
            case PlanetType.Rocky:
                selectedSound = rockyPlanetSound;
                break;

            case PlanetType.Gas:
                selectedSound = gasPlanetSound;
                break;

            case PlanetType.Ice:
                selectedSound = icePlanetSound;
                break;

            case PlanetType.Sun:
                selectedSound = sunPlanetSound;
                break;
        }

        if (audioSource == null || selectedSound == null)
            return;

        audioSource.Stop();
        audioSource.clip = selectedSound;
        audioSource.loop = true;
        audioSource.Play();
    }
}
