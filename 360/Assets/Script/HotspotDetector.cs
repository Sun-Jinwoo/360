using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class HotspotDetector : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject hotspotPanel;
    public TMP_Text labelDescripcion;
    public TMP_Text labelClick;

    [Header("Hotspots en la escena")]
    public Hotspot360[] hotspots;

    private Hotspot360 hotspotActual = null;

    // InputAction directo para el clic
    private InputAction clickAction;

    private void Awake()
    {
        // Crea la acción de clic directamente en código
        clickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        clickAction.performed += OnClick;
    }

    private void OnEnable()
    {
        clickAction.Enable();
    }

    private void OnDisable()
    {
        clickAction.Disable();
    }

    private void OnDestroy()
    {
        clickAction.performed -= OnClick;
        clickAction.Dispose();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Debug.Log($"??? Clic detectado | HotspotActual: {(hotspotActual != null ? hotspotActual.name : "NINGUNO")}");

        if (hotspotActual != null)
        {
            CambiarEsfera();
        }
        else
        {
            Debug.LogWarning("?? Clic pero no hay hotspot activo — ¿estás mirando al punto?");
        }
    }

    private void Start()
    {
        if (hotspotPanel == null) Debug.LogError("? Falta HotspotPanel");
        if (labelDescripcion == null) Debug.LogError("? Falta LabelDescripcion");
        if (labelClick == null) Debug.LogError("? Falta LabelClick");
        if (hotspots == null || hotspots.Length == 0) Debug.LogWarning("?? No hay hotspots");

        if (hotspotPanel != null)
            hotspotPanel.SetActive(false);
    }

    private void Update()
    {
        DetectarHotspot();
    }

    private void DetectarHotspot()
    {
        if (hotspots == null || hotspots.Length == 0) return;

        Hotspot360 encontrado = null;

        foreach (Hotspot360 h in hotspots)
        {
            if (h == null) continue;

            Vector3 direccion = (h.transform.position - transform.position).normalized;
            float angulo = Vector3.Angle(transform.forward, direccion);

            Debug.Log($"Hotspot: {h.name} | Ángulo: {angulo:F1}° | Activación: {h.anguloActivacion}°");

            if (angulo < h.anguloActivacion)
            {
                encontrado = h;
                break;
            }
        }

        hotspotActual = encontrado;

        if (hotspotPanel == null) return;

        if (hotspotActual != null)
        {
            hotspotPanel.SetActive(true);
            if (labelDescripcion != null)
                labelDescripcion.text = hotspotActual.descripcion;
            if (labelClick != null)
                labelClick.text = "[ Clic para continuar ]";
        }
        else
        {
            hotspotPanel.SetActive(false);
        }
    }

    private void CambiarEsfera()
    {
        Debug.Log($"?? Cambiando esfera...");
        Debug.Log($"   SphereActual:  {(hotspotActual.sphereActual != null ? hotspotActual.sphereActual.name : "NO ASIGNADA ?")}");
        Debug.Log($"   SphereDestino: {(hotspotActual.sphereDestino != null ? hotspotActual.sphereDestino.name : "NO ASIGNADA ?")}");

        if (hotspotActual.sphereActual != null)
            hotspotActual.sphereActual.SetActive(false);

        if (hotspotActual.sphereDestino != null)
            hotspotActual.sphereDestino.SetActive(true);

        if (hotspotPanel != null)
            hotspotPanel.SetActive(false);

        hotspotActual = null;
    }
}