using UnityEngine;

public class CaveAudioZone : MonoBehaviour
{
    [Header("Configuración de Reverb")]
    public AudioReverbPreset reverbPreset = AudioReverbPreset.Cave;

    [Header("Sonido dentro vs fuera")]
    [Range(0f, 1f)] public float spatialBlendInside = 0.3f;   // Más 2D adentro para que se escuche
    [Range(0f, 1f)] public float spatialBlendOutside = 1f;    // Full 3D afuera
    public float maxDistanceInside = 50f;   // Aumentado para que llegue a la cámara
    public float maxDistanceOutside = 20f;

    // Contador para evitar el parpadeo del trigger
    private int overlapCount = 0;

    private AudioReverbFilter reverbFilter;
    private AudioSource footstepSource;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        overlapCount++;

        if (overlapCount == 1)
        {
            footstepSource = other.GetComponent<FootstepAudio>()?.GetAudioSource();

            if (footstepSource != null)
            {
                // COMENTADO PARA DEBUG:
                // reverbFilter = footstepSource.gameObject.GetComponent<AudioReverbFilter>();
                // if (reverbFilter == null)
                //     reverbFilter = footstepSource.gameObject.AddComponent<AudioReverbFilter>();
                // reverbFilter.reverbPreset = reverbPreset;
                // reverbFilter.enabled = true;

                footstepSource.maxDistance = maxDistanceInside;
                footstepSource.spatialBlend = 0f; // Full 2D para probar
            }

            Debug.Log("Entraste - footstepSource: " + (footstepSource != null ? "OK" : "NULL"));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        overlapCount--;
        if (overlapCount < 0) overlapCount = 0;

        // Solo quitar efectos cuando realmente salió del todo
        if (overlapCount == 0)
        {
            if (reverbFilter != null)
                reverbFilter.enabled = false;

            if (footstepSource != null)
            {
                footstepSource.maxDistance = maxDistanceOutside;
                footstepSource.spatialBlend = spatialBlendOutside;
            }

            Debug.Log("Saliste de la zona cerrada");
        }
    }
}