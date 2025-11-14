using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;
    public Animator animator;
    private Rigidbody2D rb;
    private Vector2 movimiento;

    // NUEVAS VARIABLES para guardar la �ltima direcci�n
    private float lastHorizontal;
    private float lastVertical;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Inicializamos la �ltima direcci�n (por ejemplo, mirando hacia abajo)
        lastHorizontal = 0;
        lastVertical = -1;
    }

    void Update()
    {
        float movimientoX = Input.GetAxisRaw("Horizontal");
        float movimientoY = Input.GetAxisRaw("Vertical");

        movimiento = new Vector2(movimientoX, movimientoY).normalized;

        if (animator != null)
        {
            animator.SetFloat("Horizontal", movimiento.x);
            animator.SetFloat("Vertical", movimiento.y);
            animator.SetFloat("Speed", movimiento.sqrMagnitude);

            // --- L�GICA NUEVA ---
            // Si hay input de movimiento, actualizamos la �ltima direcci�n.
            if (movimientoX != 0 || movimientoY != 0)
            {
                lastHorizontal = movimientoX;
                lastVertical = movimientoY;
            }

            // Enviamos siempre la �ltima direcci�n al animator.
            animator.SetFloat("LastHorizontal", lastHorizontal);
            animator.SetFloat("LastVertical", lastVertical);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movimiento * velocidad;
    }
}