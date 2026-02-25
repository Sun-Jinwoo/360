using UnityEngine;

public class Hotspot360 : MonoBehaviour
{
    [Header("Qué mostrar al mirar")]
    public string descripcion = "Esta es una oficina moderna " +
        "de planta abierta con piso de madera clara, " +
        "iluminación empotrada en techo blanco, zona de sofás de " +
        "cuero negro a los lados y área de mesas al fondo. Estilo corporativo minimalista.";

    [Header("Nombre exacto de la escena destino")]
    public string escenaDestino = "Escena_Oficina";

    [Header("Ángulo de detección")]
    public float anguloActivacion = 20f;
}