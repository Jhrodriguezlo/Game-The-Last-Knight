using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHARACTER_CONTROLLER : MonoBehaviour
{
    public float velocidad;
    public float saltoAltura = 8f;
    public float sensibilidadSalto = 1f;

    private Rigidbody2D rb; 
    private bool isFacingRight = true; 
    private int saltosDisponibles = 1000; 
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; 
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        ProcessMovement();
    }

    void ProcessMovement()
    {
        float inputMovement = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(inputMovement * velocidad, rb.velocity.y);

       if(inputMovement != 0f)
    {
        animator.SetBool("isRunning", true);
    }
    else
    {
        animator.SetBool("isRunning", false);
    }

        ManageOrientation(inputMovement);

        // Agregar detección de salto con control de saltos disponibles
        if (Input.GetButtonDown("Jump") && saltosDisponibles > 0 && rb.velocity.y == 0)
        {
            float saltoReal = saltoAltura * sensibilidadSalto;
            rb.velocity = new Vector2(rb.velocity.x, saltoReal);
            saltosDisponibles--;
        }
    }

    void ManageOrientation(float inputMovement)
    {
        if ((isFacingRight && inputMovement < 0) || (!isFacingRight && inputMovement > 0))
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Suelo")
        {
            saltosDisponibles = 2; // Restablecer saltos al tocar el suelo
            transform.rotation = Quaternion.Euler(0, 0, 0); // Fijar la rotación
        }
    }
}
