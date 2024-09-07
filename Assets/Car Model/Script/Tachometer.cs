using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tachometer : MonoBehaviour
{
    public WheelController wheelController;
    public RectTransform needle;
    public float startPoint = 190f;   // điểm đầu của kim đồng hồ
    public float endPoint = -12f;     // điểm cuối của kim đồng hồ

    private float minRPM = 0f;
    private float maxRPM = 9000f;  // giới hạn RPM

    void Update()
    {
        float normalizedRPM = Mathf.InverseLerp(minRPM, maxRPM, wheelController.GetEngineRPM());
        float targetRotation = Mathf.Lerp(startPoint, endPoint, normalizedRPM);

        needle.rotation = Quaternion.Euler(0f, 0f, targetRotation);
    }
}
