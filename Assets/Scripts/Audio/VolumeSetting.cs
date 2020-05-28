using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSetting : MonoBehaviour
{
    public AudioMixer audioM;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //Asignar bg music de la escena
        //audioSource.clip = AudioManager.GetAudioClip(AudioMa;

        float v;
        audioM.GetFloat("volume", out v);
        audioSource.volume = (v + 80f) * 0.01f;
    }

    void Update()
    {
        float v;
        audioM.GetFloat("volume", out v);
        audioSource.volume = (v + 80f) * 0.01f;
    }
}
