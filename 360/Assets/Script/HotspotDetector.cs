using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    private InputAction clickAction;

    private void Awake()
    {
        clickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        clickAction.performed += OnClick;
    }

    private void OnEnable() { clickAction.Enable(); }
    private void OnDisable() { clickAction.Disable(); }
    private void OnDestroy()
    {
        clickAction.performed -= OnClick;
        clickAction.Dispose();
    }

    private void Start()
    {
        if (hotspotPanel == null) Debug.LogError("? Falta HotspotPanel");
        if (hotspots == null || hotspots.Length == 0) Debug.LogWarning("?? No hay hotspots");
        if (hotspotPanel != null) hotspotPanel.SetActive(false);
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

    private void OnClick(InputAction.CallbackContext context)
    {
        if (hotspotActual == null) return;

        string escena = hotspotActual.escenaDestino;

        if (string.IsNullOrEmpty(escena))
        {
            Debug.LogError("? El campo 'Escena Destino' está vacío en el Hotspot");
            return;
        }

        Debug.Log($"?? Cargando escena: {escena}");
        SceneManager.LoadScene(escena);
    }
}