using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelController : MonoBehaviour
{
    public WheelAlignment[] steerableWheels;

    public float BreakPower;  // thắng
    public GameObject brakeLight; // đèn của thắng
    public Color brakeLightColor; // set giá trị màu đèn của thắng

    public float Horizontal; 
    public float Vertical;

    // giá trị của hệ thống xe
    public float wheelRotateSpeed;
    public float wheelSteeringAngle;

    // Tốc độ của xe
    public float wheelAcceleration;
   

    private bool isEngineOn = false;
    public int currentGear = 0;
    public int[] gearRatios = { 0, 40, 100, 180, 240, 300, 350 }; // xe có 7 cấp số
    public Text gearText;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartEngine();    // nhấn F để khởi động xe
        }

        if (isEngineOn)    
        {
            wheelControl();
            Brake();
            AnimationWheel();
            GearShifting();


            if (currentGear == 0)
            {
                gearText.text = "N"; // khi xe đang ở số 0 thì hiện N 
            }
            else
            {
                gearText.text = currentGear.ToString();
            }
        }
    }

    void StartEngine()
    {
        isEngineOn = true;

        // xe đã khởi động nhưng giá trị wheel vẫn là 0
        foreach (var wheel in steerableWheels)
        {
            wheel.wheelCol.motorTorque = 0;
        }
    }

    // Applies steering and motor torque
    void wheelControl()
    {
        
        if (isEngineOn)
        {
            Horizontal = Input.GetAxis("Horizontal");  // đánh lái
            Vertical = Input.GetAxis("Vertical");  // tới lùi

            // giới hạn RPM theo gear cũng là tốc độ đầu ra cuối cùng
            float maxRPM = wheelAcceleration * gearRatios[currentGear] / 100f;
            foreach (var wheel in steerableWheels)
            {
                float currentRPM = wheel.wheelCol.motorTorque;    // RPM hiện tại 
                float desiredRPM = Mathf.Clamp(currentRPM, -maxRPM, maxRPM);   // giới hạn rpm từ khoảng - rpm đến rpm
                float torqueDiff = desiredRPM - currentRPM;                      // sự chênh lêch giữa mong muốn và hiện tại
                wheel.wheelCol.motorTorque += torqueDiff * Time.deltaTime * wheelAcceleration;  // motorTorque là mô men xoắn
            }

            if (Vertical > 0.1)
            {
                foreach (var wheel in steerableWheels)
                {
                    wheel.wheelCol.motorTorque = Mathf.Clamp(wheel.wheelCol.motorTorque + Time.deltaTime * wheelAcceleration, -maxRPM, maxRPM);  // giá trị từ 0.1 đến 1 thì đi tiến
                }
            }
            else if (Vertical < -0.1)
            {
                foreach (var wheel in steerableWheels)
                {
                    wheel.wheelCol.motorTorque = Mathf.Clamp(wheel.wheelCol.motorTorque - Time.deltaTime * wheelAcceleration, -maxRPM, maxRPM); // giá trị từ -0.1 đến -1 thì đi lùi
                }
            }
            else
            {
                foreach (var wheel in steerableWheels)
                {
                    wheel.wheelCol.motorTorque = Mathf.MoveTowards(wheel.wheelCol.motorTorque, 0, Time.deltaTime * wheelAcceleration);      // nếu ko có giá trị nào thì sẽ giảm tốc
                }
            }

            if (Horizontal > 0.1)
            {
                foreach (var wheel in steerableWheels)
                {
                    wheel.steeringAngle = Mathf.LerpAngle(wheel.steeringAngle, wheelSteeringAngle, Time.deltaTime * wheelRotateSpeed);  // giá trị từ 0.1 đến 1 thì bánh lái qua phải
                }
            }
            else if (Horizontal < -0.1)
            {
                foreach (var wheel in steerableWheels)
                {
                    wheel.steeringAngle = Mathf.LerpAngle(wheel.steeringAngle, -wheelSteeringAngle, Time.deltaTime * wheelRotateSpeed);     // giá trị từ -0.1 đến -1 thì bánh lái qua trái
                }
            }
            else
            {
                foreach (var wheel in steerableWheels)
                {
                    wheel.steeringAngle = Mathf.LerpAngle(wheel.steeringAngle, 0, Time.deltaTime * wheelRotateSpeed);        // nếu ko có giá trị nào thì sẽ trả bánh lái về
                }
            }
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var wheel in steerableWheels)
            {
                wheel.wheelCol.brakeTorque = 1000 * BreakPower * Time.deltaTime; // braketorque momen xoắn thắng
            }

          
            if (brakeLight != null)
            {
                var brakeLightRenderer = brakeLight.GetComponent<Renderer>();
                if (brakeLightRenderer != null)
                {
                    brakeLightRenderer.material.color = brakeLightColor;    // nêu thắng mà có giá trị thì sẽ chuyển brakeLightColor theo bản màu
                }
            }
        }
        else
        {
            foreach (var wheel in steerableWheels)
            {
                wheel.wheelCol.brakeTorque = 0;
            }

            
            if (brakeLight != null)
            {
                var brakeLightRenderer = brakeLight.GetComponent<Renderer>();
                if (brakeLightRenderer != null)
                {
                    brakeLightRenderer.material.color = Color.white;    // còn không thì set màu trắng
                }
            }
        }
    }

    void AnimationWheel()   
    {
        foreach (var wheel in steerableWheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCol.GetWorldPose(out pos, out rot); //GetWorldPose() (vong quay banh xe trong khong gian)
            wheel.wheelGraphic.transform.position = pos;    // vi tri
            wheel.wheelGraphic.transform.rotation = rot;    // goc quay
        }
    }

    void GearShifting()
    {
        if (currentGear == 0) 
        {
            // xe không có giá trị để chạy
            foreach (var wheel in steerableWheels)
            {
                wheel.wheelCol.motorTorque = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentGear < gearRatios.Length - 1)
            {
                currentGear++;                  // tăng số 
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q)) 
        {
            if (currentGear > 0)
            {
                currentGear--;                // giảm số
            }
        }
    }

    public float GetEngineRPM()
    {
        float maxWheelRPM = 0f;
        foreach (var wheel in steerableWheels)
        {
            maxWheelRPM = Mathf.Max(maxWheelRPM, Mathf.Abs(wheel.wheelCol.motorTorque) );   // hàm max tìm giá trị lớn nhất
        }

        float currentGearRatio = gearRatios[currentGear] / 100f;
        float engineRPM = maxWheelRPM * currentGearRatio;             

        return engineRPM;
    }

}
