using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perro : MonoBehaviour
{
    public float fuerzaSalto = 10f; // Puedes ajustar este valor si quieres que el perro pueda saltar en el futuro
    public float longitudRaycast = 0.1f;
    public LayerMask capaSuelo;

    private bool enSuelo;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    public Animator animator; // Asegúrate de tener un Animator en el GameObject del perro
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Necesitamos el Rigidbody2D para las físicas
    }

    // Update is called once per frame
    void Update()
    {
        // Lógica para detectar el suelo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
        enSuelo = hit.collider != null;

        // Aquí el perro no se moverá automáticamente porque no hay código de movimiento.
        // Si quieres que el perro haga algo (como ladrar o mover la cola), deberías añadir esa lógica aquí.

        // Actualiza el parámetro "enSuelo" del Animator para las animaciones
        if (animator != null)
        {
            animator.SetBool("enSuelo", enSuelo); // Usa este parámetro para transiciones de animación
        }
    }

    // Dibuja el Raycast en el editor para facilitar la depuración
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}
