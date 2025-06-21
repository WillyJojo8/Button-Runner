using UnityEngine;

public class PowerBox : MonoBehaviour
{
    public float duracionInvencible = 3f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerManager.Instance.ActivarInvencibilidad(duracionInvencible);
            Destroy(gameObject);
        }
    }
}