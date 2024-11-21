using TMPro;
using UnityEngine;

public class BulletCounterUI : MonoBehaviour
{
    public TextMeshProUGUI bulletCountText;
    private int bulletsVisible = 0; // Contador de balas visibles

    // Inicializar el contador de balas
    void Start()
    {
        UpdateBulletCountText();
    }

    // Incrementar el contador de balas visibles
    public void IncrementBulletsVisible(int count = 1)
    {
        bulletsVisible += count;
        UpdateBulletCountText();
    }

    // Decrementar el contador de balas visibles
    public void DecrementBulletsVisible(int count = 1)
    {
        bulletsVisible -= count;

        // Seguridad: evitar valores negativos en el contador
        if (bulletsVisible < 0)
        {
            bulletsVisible = 0;
        }

        UpdateBulletCountText();
    }

    // Actualizar el texto del contador de balas solo si ha habido un cambio
    private void UpdateBulletCountText()
    {
        bulletCountText.text = "Visible Bullets: " + bulletsVisible;
    }
}