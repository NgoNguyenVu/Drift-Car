using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAlignment : MonoBehaviour
{
    public GameObject wheelBase;
    public GameObject wheelGraphic;

    public WheelCollider wheelCol;

    public bool steerable;

    public float steeringAngle;

    RaycastHit hit;

    void Update()
    {
        alignMeshToCollider();
    }

    void alignMeshToCollider()
    {
        // kiểm tra vị trí bánh xe có tiếp xúc mặt đất hay ko 
        if (Physics.Raycast(wheelCol.transform.position, -wheelCol.transform.up, out hit, wheelCol.suspensionDistance + wheelCol.radius))     // suspensionDistance hệ thống treo bánh xe
        {
            wheelGraphic.transform.position = hit.point + wheelCol.transform.up * wheelCol.radius;                 // điểm va chạm + điểm hướng lên của bánh * với bán kính của bánh
        }
        else
        {
            wheelGraphic.transform.position = wheelCol.transform.position - (wheelCol.transform.up * wheelCol.suspensionDistance);
        }

        if (steerable)
        {
            wheelCol.steerAngle = steeringAngle; //nếu có đánh lái gán vào giá trị steeringAngle
        }
        wheelGraphic.transform.localRotation = Quaternion.Euler(0, wheelCol.steerAngle, 0);
        wheelGraphic.transform.Rotate(wheelCol.motorTorque / 60 * 360 * Time.deltaTime, 0, 0, Space.Self); //  sử dụng Space.Self để quay theo trục cục bộ của bánh đồ họa
    }
}
