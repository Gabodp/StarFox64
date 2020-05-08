using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSetting : MonoBehaviour
{
    public AudioMixer audioM;
    public AudioSource audioS;

    // Start is called before the first frame update
    void Start()
    {
        float v;
        audioM.GetFloat("volume", out v);
        audioS.volume = (v + 80f) * 0.01f;
    }

    void Update()
    {
        float v;
        audioM.GetFloat("volume", out v);
        audioS.volume = (v + 80f) * 0.01f;
    }
}
