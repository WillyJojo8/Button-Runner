using UnityEngine;
using UnityEngine.UI;

public class InvencibilidadUI : MonoBehaviour
{
    public Image fillImage;
    private float duracionTotal;
    private float tiempoRestante;
    private bool activa = false;

    void Start()
    {
        gameObject.SetActive(false); // Ocultar al inicio
    }

    void Update()
    {
        if (activa)
        {
            tiempoRestante -= Time.unscaledDeltaTime;

            if (tiempoRestante <= 0f)
            {
                fillImage.fillAmount = 0f;
                activa = false;
                gameObject.SetActive(false); // Ocultar cuando termina
            }
            else
            {
                fillImage.fillAmount = tiempoRestante / duracionTotal;
            }
        }
    }

    public void ActivarBarra(float duracion)
    {
        duracionTotal = duracion;
        tiempoRestante = duracion;
        fillImage.fillAmount = 1f;
        activa = true;
        gameObject.SetActive(true); // Mostrar la barra
    }
}
