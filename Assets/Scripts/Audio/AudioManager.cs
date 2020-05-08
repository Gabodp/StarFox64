using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{

    public enum Sound
    {
        PlayerLaser,
        EnemyLaser,
        DestroyExplosion,
    }

    //El diccionario puede cambiarse para que albergue una clase con config especifica de cada sonido
    private static Dictionary<Sound, float> soundTimerDictionary;
    private static GameObject u_AudioGameObject;
    private static AudioSource u_AudioSource;
    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        /* Aqui debajo inicializar los timers de todos los sonidos que los requieran.
           En otras palabras, todos los "cases" del switch dentro de canPlay().
           Deben haber minimo el mismo numero de inicializaciones que de cases.*/

        soundTimerDictionary[Sound.PlayerLaser] = 0.0f;
        soundTimerDictionary[Sound.EnemyLaser] = 0.0f;
    }

    public static void PlaySound(Sound sound, Vector3 pos, float radius)
    {
        if (canPlay(sound))
        {
            GameObject soundObject = new GameObject("Sound");
            soundObject.transform.position = pos;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.maxDistance = radius;
            audioSource.spatialBlend = 1f;
            //Falta regular el volumen
            audioSource.Play();

            Object.Destroy(soundObject, audioSource.clip.length);
        }
    }

    //Audio 2D, como el background
    public static void PlaySound(Sound sound)
    {
        if (canPlay(sound))
        {
             if(u_AudioGameObject == null)
            {
                u_AudioGameObject = new GameObject("Unique Sound Object");
                u_AudioSource = u_AudioGameObject.AddComponent<AudioSource>();
            }

            u_AudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.instance.soundsArray)
        {
            if(soundAudioClip.sound == sound)
            {
                return soundAudioClip.audio;
            }
        }

        //Aqui no deberia llegar nunca, a menos que no exista ese nobmre de audio, pero como
        //usamos Enums, no pasara
        return null;
    }


    //Se puede usar en caso de que querramos agregar restricciones de que tan frecuente debe sonar un sonido
    private static bool canPlay(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;

            case Sound.PlayerLaser:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimestamp = soundTimerDictionary[sound];
                    float laserMaxTimer = 0.05f;
                    if (lastTimestamp + laserMaxTimer < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                        return false;

                }
                else
                    return false;
        }
    }

}
