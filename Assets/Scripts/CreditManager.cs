using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CreditManager : MonoBehaviour
{


    public Animation creditAnimation;
    public AudioSource musicSource;
    public string nextSceneName = "Main";

    void Update()
    {
        // Aceleración (ya lo tienes)
        bool accelerating = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);
        float speed = accelerating ? 4f : 1f;

        if (creditAnimation != null && creditAnimation.clip != null)
        {
            var clipName = creditAnimation.clip.name;
            creditAnimation[clipName].speed = speed;

            if (!creditAnimation.IsPlaying(clipName) || Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }

        if (musicSource != null)
            musicSource.pitch = accelerating ? 2f : 1f;
    }

   
}
