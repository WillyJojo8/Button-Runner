using UnityEngine;

public class MoverHaciaIzquierda : MonoBehaviour
{
    public float speed = 2.5f;

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < -10.5f)
            Destroy(gameObject);
    }
}