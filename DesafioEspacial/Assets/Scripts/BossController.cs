using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform[] firePoints; // Puntos de disparo
    public GameObject bulletPrefab; // Prefab de la bala
    public float fireRate = 0.5f; // Tiempo entre disparos
    private float nextFireTime;

    private int currentPattern = 0; // Patrón actual
    private float patternDuration = 10f; // Duración de cada patrón
    private float patternTimer; // Temporizador para cambiar de patrón

    public float bulletSpeed = 10f; // Velocidad de la bala
    private float spiralAngle = 0f; // Ángulo de la espiral

    // Inicializar el temporizador del patrón
    void Start() 
    {
        patternTimer = patternDuration;
    }

    // Actualizar el temporizador del patrón y disparar
    void Update()
    {
        patternTimer -= Time.deltaTime;
        if (patternTimer <= 0)
        {
            SwitchPattern();
            patternTimer = patternDuration;
        }

        if (Time.time >= nextFireTime)
        {
            FirePattern();
            nextFireTime = Time.time + fireRate;
        }
    }

    // Disparar según el patrón actual seleccionado
    void FirePattern()
    {
        switch (currentPattern)
        {
            case 0:
                FireSpiral();
                SpreadShot();
                break;
            case 1:
                SemiCircularShot();
                break;
            case 2:
                CosineWaveShot();
                break;
        }
    }

    // Disparar desde un solo punto
    void SingleShot(Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * bulletSpeed;
        FindAnyObjectByType<BulletCounterUI>().IncrementBulletsVisible();
    }

    // Disparar un patrón lineal desde varios puntos
    void SpreadShot()
    {
        foreach (var firePoint in firePoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            FindAnyObjectByType<BulletCounterUI>().IncrementBulletsVisible();
        }
    }

    // Disparar un patrón semicircular desde varios puntos con un punto adicional
    void SemiCircularShot()
    {
        int bullets = 10;
        float spreadAngle = 180f;
        float angleStep = spreadAngle / (bullets - 1); // Separación angular entre balas
        float startAngle = -spreadAngle / 2; // Ángulo inicial para cubrir el sector del jugador

        for (int i = 0; i < firePoints.Length; i++)
        {
            Transform firePoint = firePoints[i];

            if (i < 2) // Los primeros dos firePoints realizan el disparo semicircular
            {
                for (int j = 0; j < bullets; j++)
                {
                    float angle = startAngle + (angleStep * j);
                    Quaternion rotation = Quaternion.Euler(0, angle, 0);
                    Vector3 bulletDirection = rotation * Vector3.back;

                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(bulletDirection));
                    Rigidbody rb = bullet.GetComponent<Rigidbody>();

                    rb.linearVelocity = bulletDirection * bulletSpeed;
                    FindAnyObjectByType<BulletCounterUI>().IncrementBulletsVisible();
                }
            }
            else // El tercer firePoint realiza un disparo único
            {
                SingleShot(firePoint);
            }
        }
    }

    // Disparar un patrón espiral desde varios puntos
    void FireSpiral()
    {
        float angleStep = 30f; // Separación angular entre balas
        float startAngle = -45f; // Ángulo inicial para cubrir el sector del jugador
        float endAngle = 45f; // Ángulo final para cubrir el sector del jugador
        float sectorAngle = endAngle - startAngle; // Ángulo total del sector

        for (int i = 0; i < firePoints.Length; i++)
        {
            Transform firePoint = firePoints[i];
            float angle = startAngle + (spiralAngle % sectorAngle); // Ajustar el ángulo dentro del sector
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 bulletDirection = rotation * Vector3.back; // Disparar hacia el eje -z

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(bulletDirection));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.linearVelocity = bulletDirection * bulletSpeed; 
            FindAnyObjectByType<BulletCounterUI>().IncrementBulletsVisible();
        }

        spiralAngle += angleStep; // Incrementa el ángulo para la próxima iteración
    }

    void CosineWaveShot()
    {
        int bullets = 10;
        float spreadAngle = 180f;
        float angleStep = spreadAngle / (bullets - 1);
        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < firePoints.Length; i++)
        {
            Transform firePoint = firePoints[i];
            float offset = i * (2 * Mathf.PI / firePoints.Length); // Desfase para cada firePoint

            for (int j = 0; j < bullets; j++)
            {
                float angle = startAngle + (angleStep * j);
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                Vector3 bulletDirection = rotation * Vector3.back; // Disparar hacia el eje -z

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(bulletDirection));
                Rigidbody rb = bullet.GetComponent<Rigidbody>();

                rb.velocity = bulletDirection * bulletSpeed + new Vector3(0, Mathf.Cos(Time.time * 5f + offset) * 2f, 0); // Añadir movimiento cosenoidal con desfase
                FindAnyObjectByType<BulletCounterUI>().IncrementBulletsVisible();
            }
        }
    }

    // Cambiar al siguiente patrón
    void SwitchPattern()
    {
        currentPattern = (currentPattern + 1) % 3; 
    }
}