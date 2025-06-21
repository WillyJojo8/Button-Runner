using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ScoreManager.Instance.IncreaseCoinPoints(); // Suma 5 puntos
            AudioManager.Instance.PlaySound("Coin");
            Destroy(gameObject);
        }
    }
}