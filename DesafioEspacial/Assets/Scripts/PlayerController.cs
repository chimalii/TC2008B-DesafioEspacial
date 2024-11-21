using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f; // Velocidad de movimiento
    public Vector3 minBounds, maxBounds; // Límites del área de juego

    // Actualizar la posición de la nave
    void Update()
    {
        // Limitar movimiento de la nave en horizontal, eje z constante
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(horizontal, 0, 0) * speed * Time.deltaTime;
        transform.position += movement;

        // Limitar posición dentro del área de juego
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
            transform.position.y,
            transform.position.z
        );
    }
}