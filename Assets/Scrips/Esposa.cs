using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esposa : MonoBehaviour
{
    public float fuerzaSalto = 10f;
    public float longitudRaycast = 0.1f;
    public LayerMask capaSuelo;

    private bool enSuelo;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    public Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Lógica para detectar el suelo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
        enSuelo = hit.collider != null;

        // Aquí el personaje no se moverá porque no hay código que gestione el input del jugador.
        // Si en el futuro quisieras que siguiera al jugador, aquí iría esa lógica.

        // Se actualiza la animación para reflejar si está en el suelo o no.
        if (animator != null)
        {
            animator.SetBool("enSuelo", enSuelo);
        }
    }

    // Dibuja el Raycast en el editor para facilitar la depuración
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}
