using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public Text pointsText, maxPointsText;

    [SerializeField] private int points = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateMaxPoints();
    }

    public void IncreasePoints(int cantidad = 1)
    {
        points += cantidad;
        pointsText.text = points.ToString();
        UpdateMaxPoints();
    }

    public void IncreaseCoinPoints()
    {
        IncreasePoints(5); // Añade 5 puntos por moneda
    }

    public void UpdateMaxPoints()
    {
        int maxPoints = PlayerPrefs.GetInt("Max", 0);

        if (points >= maxPoints)
        {
            maxPoints = points;
            PlayerPrefs.SetInt("Max", maxPoints);
        }

        maxPointsText.text = "BEST: " + maxPoints.ToString();
    }
}