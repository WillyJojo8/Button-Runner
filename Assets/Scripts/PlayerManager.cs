using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public Animator animator;
    public bool invencible = false;
    public bool enemyCollision = false, grounded;

    private float startY;
    private Coroutine invencibleCoroutine;

    [Header("UI")]
    public InvencibilidadUI invencibilidadUI; // Asignar en el inspector

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
        if (invencibleCoroutine != null)
        {
            StopCoroutine(invencibleCoroutine);
        }

        invencibleCoroutine = StartCoroutine(InvencibleTemporal(duracion));

        if (invencibilidadUI != null)
            invencibilidadUI.ActivarBarra(duracion);
    }

    IEnumerator InvencibleTemporal(float duracion)
    {
        invencible = true;
        GetComponent<SpriteRenderer>().color = Color.yellow;
        AudioManager.Instance?.PlaySound("PowerUp");

        yield return new WaitForSecondsRealtime(duracion);

        invencible = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
