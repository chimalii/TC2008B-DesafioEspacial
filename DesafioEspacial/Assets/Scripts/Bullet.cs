using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Velocidad de la bala
    private bool isVisible = false; // Para rastrear si la bala está visible
    private BulletCounterUI bulletCounter;

    // Inicializar el contador de balas
    void Start()
    {
        bulletCounter = FindAnyObjectByType<BulletCounterUI>();
    }

    // Actualizar el estado de la bala
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Verificar si la bala ha rebasado los límites
        if (transform.position.z <= -12f || Mathf.Abs(transform.position.x) >= 9f)
        {
            if (bulletCounter != null && isVisible)
            {
                bulletCounter.DecrementBulletsVisible();
                isVisible = false;
            }

            // Destruir la bala
            Destroy(gameObject);
        }
    }

    // Detectar colisiones con el jugador y destruir la bala
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (bulletCounter != null && isVisible)
            {
                bulletCounter.DecrementBulletsVisible();
                isVisible = false;
            }

            // Destruir la bala
            Destroy(gameObject);
        }
    }

    // Detectar si la bala se ha vuelto visible
    void OnBecameVisible()
    {
        if (bulletCounter != null && !isVisible)
        {
            bulletCounter.IncrementBulletsVisible();
            isVisible = true;
        }
    }

    // Detectar si la bala se ha vuelto invisible
    void OnBecameInvisible()
    {
        if (bulletCounter != null && isVisible)
        {
            bulletCounter.DecrementBulletsVisible();
            isVisible = false;
        }

        // Destruir la bala si sale de la pantalla
        Destroy(gameObject);
    }
}
