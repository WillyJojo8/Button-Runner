using UnityEngine;
using UnityEngine.UI;

public class InvencibilidadUI : MonoBehaviour
{
    public static InvencibilidadUI Instance { get; private set; }

    public Image fillImage;
    private float duracionTotal;
    private float tiempoRestante;
    private bool activa = false;
    private ParticleSystem particleSystem;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        particleSystem = GameObject.Find("Progress Bar Particles").GetComponent<ParticleSystem>();
    }

    void Start()
    {
        gameObject.SetActive(false); 
    }

    void Update()
    {
        if (activa)
        {
            tiempoRestante -= Time.unscaledDeltaTime;

            // Solo reproducir si no está ya en marcha
            if (particleSystem != null && !particleSystem.isPlaying)
            {
                particleSystem.Play();
            }

            if (tiempoRestante <= 0f)
            {
                fillImage.fillAmount = 0f;
                activa = false;
                gameObject.SetActive(false);

                // Al terminar, detener y resetear
                if (particleSystem != null && particleSystem.isPlaying)
                {
                    particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
            else
            {
                fillImage.fillAmount = tiempoRestante / duracionTotal;

                // Mover partículas en sincronía con la barra
                float width = fillImage.rectTransform.rect.width;
                Vector3 newPosition = particleSystem.transform.localPosition;
                newPosition.x = (width * fillImage.fillAmount) - (width / 2f);
                particleSystem.transform.localPosition = newPosition;
            }
        }
    }

    public void ActivarBarra(float duracion)
    {
        duracionTotal = duracion;
        tiempoRestante = duracion;
        fillImage.fillAmount = 1f;
        activa = true;
        gameObject.SetActive(true);

        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }

    public void PauseBar()
    {
        activa = false;

        if (particleSystem != null && particleSystem.isPlaying)
        {
            particleSystem.Pause();
        }
    }

    public void ResumeBar()
    {
        activa = true;

        if (particleSystem != null && particleSystem.isPaused)
        {
            particleSystem.Play();
        }
    }


    public bool BarraActiva()
    {
        return activa;
    }
}
