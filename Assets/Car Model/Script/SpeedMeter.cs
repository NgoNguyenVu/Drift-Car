using UnityEngine;
using UnityEngine.UI;

public class SpeedMeter : MonoBehaviour
{
    public Rigidbody targetRigidbody;  
    public Text speedometerText;  
    public float Speed;
    private void Update()
    {
        if (targetRigidbody != null && speedometerText != null)
        {
            Speed = targetRigidbody.velocity.magnitude * 3.6f;  // chuyển đổi từ mile về km/h

            speedometerText.text = Speed.ToString("0") + " km/h";
        }
    }
}
