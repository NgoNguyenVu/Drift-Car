using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private WheelSkid wheelSkid;

    private void Start()
    {
        wheelSkid = GetComponent<WheelSkid>();
    }

    private void Update()
    {
        scoreText.text = "Score: " + wheelSkid.totalScore.ToString("F0");
    }
}
