using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    public AudioSource EngineHighOn;
    public SpeedMeter Speed;

    public AudioSource GearChangeSound;
    public AudioSource startAudioSource;
    public AudioSource engineStart;

    private bool isEngineRunning = false;


    public float PitchOffSet1;
    public float PitchOffSet2;
    public float PitchOffSet3;
    public float PitchOffSet4;
    public float PitchOffSet5;
    public float PitchOffSet6;

    void Start()
    {

    }

    void Update()
    {
        PitchControl();
        GearChange();
        EngineVolume();
        if (Input.GetKeyDown(KeyCode.F))
    {
        if (!isEngineRunning)
        {
            // start nổ máy
            startAudioSource.PlayDelayed(0f); // delay start

                // Start tiếng IDLE
            engineStart.PlayDelayed(1.2f); // delay start 1.2

                isEngineRunning = true;
        }
        else
        {
                // Stop tiếng IDLE
                engineStart.Stop();

            isEngineRunning = false;
        }
    }
    }


    public void EngineVolume()
    {

        if (Input.GetAxis("Vertical") == 1)
        {
            EngineHighOn.volume += Time.deltaTime;
        }
        else
        {
            if (EngineHighOn.volume > 0.1f)
            {
                EngineHighOn.volume -= Time.deltaTime;
            }
        }

    }


    public void GearChange()
    {
       int currentGear = GetComponent<WheelController>().currentGear;

    if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q))
    {
        if (!GearChangeSound.isPlaying)
        {
            GearChangeSound.Play();
        }

        // Phát âm thanh exhaust fire
        // Tên AudioSource cho âm thanh exhaust fire là "ExhaustFireSound" (thay đổi tên nếu cần thiết)
       /* AudioSource exhaustFireSound = GameObject.Find("ExhaustFireSound").GetComponent<AudioSource>();
        if (!exhaustFireSound.isPlaying)
        {
            exhaustFireSound.Play();
        } */
    }
    }

    public void PitchControl()
    {
        if (Speed.Speed > 0 & Speed.Speed < 30)
        {
            EngineHighOn.pitch = Speed.Speed * PitchOffSet1;
        }

        if (Speed.Speed > 30 & Speed.Speed < 60)
        {
            EngineHighOn.pitch = Speed.Speed * PitchOffSet2;
        }

        if (Speed.Speed > 60 & Speed.Speed < 90)
        {
            EngineHighOn.pitch = Speed.Speed * PitchOffSet3;
        }

        if (Speed.Speed > 90 & Speed.Speed < 120)
        {
            EngineHighOn.pitch = Speed.Speed * PitchOffSet4;
        }

        if (Speed.Speed > 120 & Speed.Speed < 150)
        {
            EngineHighOn.pitch = Speed.Speed * PitchOffSet5;
        }

        if (Speed.Speed > 150)
        {
            EngineHighOn.pitch = Speed.Speed * PitchOffSet6;
        }
    }


}
