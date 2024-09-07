using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSkid : MonoBehaviour
{
    public float score; 
    public float totalScore;    // điểm

    public AudioSource SkidSound;
    public float SkidSoundMultiplyer; // âm thanh

    public ParticleSystem smokeParticles; 
    public float smokeIntensityMultiplier = 1.0f;  // khói của 

    private WheelCollider wheelCollider;
    private WheelHit wheelHitInfo;
    private Rigidbody rb;
    private Skidmarks skidmarksController;

  
    private const float SKID_FX_SPEED = 0.5f;
    private const float MAX_SKID_INTENSITY = 20.0f;
    private const float WHEEL_SLIP_MULTIPLIER = 10.0f;

    private int lastSkid = -1;
    private float lastFixedUpdateTime;

    private void Awake()
    {
        wheelCollider = GetComponent<WheelCollider>();
        lastFixedUpdateTime = Time.time;

       
        rb = GetComponentInParent<Rigidbody>();
        skidmarksController = FindObjectOfType<Skidmarks>();
    }

    private void FixedUpdate()
    {
        lastFixedUpdateTime = Time.time;
    }

    private void LateUpdate()
    {
        if (wheelCollider.GetGroundHit(out wheelHitInfo))
        {
            // kiểm tra trượt của bánh xe
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
            float skidTotal = Mathf.Abs(localVelocity.x);

            // kiểm tra vòng quay bánh xe
            float wheelAngularVelocity = wheelCollider.radius * ((2 * Mathf.PI * wheelCollider.rpm) / 60);
            float carForwardVel = Vector3.Dot(rb.velocity, transform.forward);
            float wheelSpin = Mathf.Abs(carForwardVel - wheelAngularVelocity) * WHEEL_SLIP_MULTIPLIER;
            wheelSpin = Mathf.Max(0, wheelSpin * (10 - Mathf.Abs(carForwardVel)));

            skidTotal += wheelSpin;

            // edit khói
            if (skidTotal >= SKID_FX_SPEED)
            {
                float intensity = Mathf.Clamp01(skidTotal / MAX_SKID_INTENSITY);
                Vector3 skidPoint = wheelHitInfo.point + (rb.velocity * (Time.time - lastFixedUpdateTime));

                smokeParticles.transform.position = skidPoint;
                smokeParticles.transform.rotation = Quaternion.LookRotation(wheelHitInfo.normal);
                var emission = smokeParticles.emission;
                emission.rateOverTime = intensity * smokeIntensityMultiplier;

                lastSkid = skidmarksController.AddSkidMark(skidPoint, wheelHitInfo.normal, intensity, lastSkid);

                SkidSound.volume = intensity / SkidSoundMultiplyer;

                score += intensity * Time.deltaTime;
                totalScore += score;

            }
            else
            {
                lastSkid = -1;
                var emission = smokeParticles.emission;
                emission.rateOverTime = 0f;
            }
        }
        else
        {
            lastSkid = -1;
            var emission = smokeParticles.emission;
            emission.rateOverTime = 0f;
        }
    }
}
