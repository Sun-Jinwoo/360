using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [Header("Audio Source")]
    private AudioSource audioSource;

    [Header("Clips de pasos")]
    public AudioClip[] footstepClips;       // Clips para superficie normal
    public AudioClip[] runFootstepClips;    // Clips para cuando corre (opcional)

    [Header("Configuración")]
    public float walkStepInterval = 0.5f;   // Tiempo entre pasos caminando
    public float runStepInterval = 0.28f;   // Tiempo entre pasos corriendo
    [Range(0f, 1f)] public float volume = 0.7f;

    private float stepTimer = 0f;
    private MovePlayer movePlayer;
    private CharacterController character;

    private void Awake()
    {
        movePlayer = GetComponent<MovePlayer>();
        character = GetComponent<CharacterController>();

        // Crear AudioSource en el propio personaje
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;      // 3D total
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 20f;
        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        // Solo reproducir si está en el suelo y moviéndose
        bool isMoving = movePlayer.IsMoving;
        bool isRunning = movePlayer.IsRunning;
        bool isGrounded = character.isGrounded;

        if (!isMoving || !isGrounded)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer += Time.deltaTime;
        float interval = isRunning ? runStepInterval : walkStepInterval;

        if (stepTimer >= interval)
        {
            PlayFootstep(isRunning);
            stepTimer = 0f;
        }
    }

    private void PlayFootstep(bool running)
    {
        AudioClip[] clips = (running && runFootstepClips.Length > 0)
            ? runFootstepClips
            : footstepClips;

        if (clips.Length == 0) return;

        // Elegir clip aleatorio para que no suene repetitivo
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip, volume);
    }

    // Método público para que zonas de audio puedan cambiar el AudioSource
    public AudioSource GetAudioSource() => audioSource;
}