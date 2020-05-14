using System.Collections;
using System;
using UnityEngine;

public class SwitchMusic : MonoBehaviour
{
    public AudioManager.Sound[] sounds;

    IEnumerator ChangeBackgroundMusic()
    {
        foreach(AudioManager.Sound sound in sounds)
        {
            GameAssets.SoundAudioClip s_object = Array.Find(GameAssets.Instance.soundsArray, soundObject => soundObject.sound == sound);
            GameController.Instance.SetBackgroundMusic(sound);

            yield return new WaitForSeconds(s_object.audio.length);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(ChangeBackgroundMusic());
        }
    }
}
