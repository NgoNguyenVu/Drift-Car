using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CarControl : MonoBehaviour
{
    public enum Axel     //liet ke cac bien do minh tu dinh nghia
    {
        Front,
        Rear
    }
    [Serializable]        //chuyen doi object ve dang trung gian
    public struct Wheel            //khai bao thanh phan co nhieu du lieu khac nhau
    {
        public GameObject wheelModel;       //tham chieu den GO
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public float MaxAcceleration = 30f;       //gia toc toi da
    public float brakeAccleration = 50f;      //gia toc khi phanh

    public float turnSensitivity = 1f;        //do nhay lai
    public float maxSteerAngle = 30f;         //toi da banh lai'

    public Vector3 CenterCar;


    public List<Wheel> wheels;     
    float moveInput;   
    float steerInput;     

    private Rigidbody carRb;


    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = CenterCar;      //dung centerofmass thuoc tinh cua lop rigidbody dung de thiet lap trong tam cua vat

    }
    
    void Update()  
    {
        GetInputs();     //lay tt tu keyboard (readline)
        AnimationWheel();
    }

    
    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }
    
   void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");     //gia tri move truyen vao di thang    W and S  
        steerInput = Input.GetAxis("Horizontal");     //gia tri steer de di ngang     A and D
    }
   
    void Move()                          //di chuyen
    {
        foreach(var Wheel in wheels)
        {
            Wheel.wheelCollider.motorTorque = moveInput * 100 * MaxAcceleration * Time.deltaTime;      //motorTorque thuoc tinh cua wheelCollider (momen xoan N/M)
        }
    }

    void Steer()
    {
        foreach(var Wheel in wheels )
        {
            if(Wheel.axel == Axel.Front)
            {
                var maxsteerAngle = steerInput * turnSensitivity * maxSteerAngle;      //tinh goc' lai' cua xe 
                Wheel.wheelCollider.steerAngle = Mathf.Lerp(Wheel.wheelCollider.steerAngle, maxsteerAngle, 0.6f);    // dung math.lerp de cap nhap goc lai banh xe (gia tri hien tai cua goc lai w.w.SA, gia tri moi cua goc lai SA) (ham lerp dung de tao hieu ung di chuyen)   
            }
        }
    }

    void Brake()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            foreach(var Wheel in wheels )
            {
                Wheel.wheelCollider.brakeTorque = 300 * brakeAccleration * Time.deltaTime;    //brakeTorque thuoc tinh cua wheelcollider 
            }
        }
        else
        {
            foreach(var Wheel in wheels )
            {
                Wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }


    void AnimationWheel()
    {
        foreach(var Wheel in wheels )
        {
            Quaternion rot;  
            Vector3 pos;
            Wheel.wheelCollider.GetWorldPose(out pos, out rot);       //GetWorldPose() (vong quay banh xe trong khong gian)
            Wheel.wheelModel.transform.position = pos;     // vi tri
            Wheel.wheelModel.transform.rotation = rot;      // goc quay
 
        }
    }


    
}    
