using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Variable pública para que puedas arrastrar tu personaje a ella en el Inspector.
    public Transform target;

    // Velocidad con la que la cámara se pondrá al día. Un valor más pequeño será más lento y suave.
    public float smoothSpeed = 0.125f;

    // El desfase de la cámara con respecto al objetivo (para mantener la distancia en Z).
    public Vector3 offset;

    // LateUpdate se ejecuta después de que todos los Update() hayan sido llamados.
    // Es el mejor lugar para el código de seguimiento de cámara, para asegurar que el objetivo
    // ya ha completado su movimiento de ese frame.
    void LateUpdate()
    {
        // La posición a la que la cámara quiere moverse.
        Vector3 desiredPosition = target.position + offset;

        // Interpola suavemente desde la posición actual de la cámara a la posición deseada.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Aplica la nueva posición a la cámara.
        transform.position = smoothedPosition;
    }
}