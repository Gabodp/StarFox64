
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    //Default values
    private float amplitude = 1.5f;    
    
    private readonly float frequencyOfShakes = 2.0f;         

    private float elapsedTime = 0f;


    public CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin vCamNoise;

    private void Start()
    {

        if (vCam != null)
            vCamNoise = vCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }


    void Update()
    {


        if (vCam != null && vCamNoise != null)
        {
            if (elapsedTime > 0)
            {

                vCamNoise.m_AmplitudeGain = amplitude;
                vCamNoise.m_FrequencyGain = frequencyOfShakes;


                elapsedTime -= Time.deltaTime;
            }
            else
            {

                vCamNoise.m_AmplitudeGain = 0f;
                elapsedTime = 0f;
            }
        }
    }

    public void ShakeCamera(float magnitude, float duration)
    {
        amplitude = magnitude;
        elapsedTime = duration;
        
    }
}
