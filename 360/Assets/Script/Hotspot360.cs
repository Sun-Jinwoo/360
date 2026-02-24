using UnityEngine;

public class Hotspot360 : MonoBehaviour
{
    [Header("Qué mostrar al mirar")]
    public string descripcion = "Ir a la Oficina";

    [Header("A qué esfera ir al hacer clic")]
    public GameObject sphereActual;     // Sphere_Biblioteca
    public GameObject sphereDestino;    // Sphere_Oficina

    [Header("Ángulo de detección (grados)")]
    public float anguloActivacion = 20f;
}