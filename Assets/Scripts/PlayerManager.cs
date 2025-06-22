using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("Referencias")]
    public Animator animator;
    public InvencibilidadUI invencibilidadUI; // Asignar en el inspector

    [Header("Estado del jugador")]
    public bool invencible = false;
    public bool enemyCollision = false;
    public bool grounded;

    private float startY;

    // Invencibilidad controlada por tiempo
    private float tiempoInvencibilidad = 0f;
    private bool invencibilidadActiva = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        startY = transform.position.y;

        if (invencibilidadUI == null)
        {
            invencibilidadUI = FindObjectOfType<InvencibilidadUI>();
        }
    }

    void Update()
    {
        grounded = transform.position.y == startY;

        if (invencibilidadActiva)
        {
            tiempoInvencibilidad -= Time.unscaledDeltaTime;

            if (tiempoInvencibilidad <= 0f)
            {
                FinalizarInvencibilidad();
            }
        }
    }

    public void SetAnimation(string name)
    {
        animator.Play(name);

        if (name == "PlayerJump")
        {
            AudioManager.Instance?.PlaySound("Jump");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            if (!invencible)
            {
                enemyCollision = true;
                GetComponent<BoxCollider2D>().enabled = false;
                AudioManager.Instance?.StopMusic();
                AudioManager.Instance?.PlaySound("Die");
            }
        }
        else if (collider.CompareTag("Points"))
        {
            ScoreManager.Instance?.IncreasePoints();
            AudioManager.Instance?.PlaySound("Point");
        }
    }

    public void ActivarInvencibilidad(float duracion)
    {
        tiempoInvencibilidad = duracion;
        invencibilidadActiva = true;
        invencible = true;

        GetComponent<SpriteRenderer>().color = Color.yellow;
        AudioManager.Instance?.PlaySound("PowerUp");

        if (invencibilidadUI != null)
        {
            invencibilidadUI.ActivarBarra(duracion);
        }
    }

    private void FinalizarInvencibilidad()
    {
        invencibilidadActiva = false;
        invencible = false;

        GetComponent<SpriteRenderer>().color = Color.white;

        // Puedes notificar al UI si quieres que se oculte inmediatamente
        // invencibilidadUI?.ForzarFinalizacion(); // si decides agregar ese método
    }
}
