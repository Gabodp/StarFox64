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
        RingCollected,
        Background,
        Interruption,
        WarningAlarm,
        Phase2Giorno,
        TurretLaunchMissile,
        TurretTargeting,
    }

    //El diccionario puede cambiarse para que albergue una clase con config especifica de cada sonido
    private static Dictionary<Sound, float> soundTimerDictionary;
    private static GameObject u_AudioGameObject;
    private static AudioSource u_AudioSource;

    private static GameObject backgroundAudioObject;
    private static AudioSource backgroundAudioSource;

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>
        {
            /* Aqui debajo inicializar los timers de todos los sonidos que los requieran.
               En otras palabras, todos los "cases" del switch dentro de canPlay().
               Deben haber minimo el mismo numero de inicializaciones que de cases.*/

            [Sound.PlayerLaser] = 0.0f,
            [Sound.EnemyLaser] = 0.0f
        };
    }

    public static void PlaySound(Sound sound, Vector3 pos, float radius)
    {
        if (CanPlay(sound))
        {
            GameObject soundObject = new GameObject("Sound");
            soundObject.transform.position = pos;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            GameAssets.SoundAudioClip AudioClipObject = GetAudioClipObject(sound);
            audioSource.clip = AudioClipObject.audio;
            audioSource.volume = AudioClipObject.volume;
            audioSource.pitch = AudioClipObject.pitch;
            audioSource.maxDistance = radius;
            audioSource.spatialBlend = 1f;
            //Falta regular el volumen
            audioSource.Play();

            Object.Destroy(soundObject, audioSource.clip.length);
        }
    }

    public static void PlayAsBackground(Sound sound)
    {
        if(backgroundAudioObject == null)
        {
            backgroundAudioObject = new GameObject("BackGround Music");
            backgroundAudioSource = backgroundAudioObject.AddComponent<AudioSource>();
        }

        if (backgroundAudioSource.isPlaying)
            backgroundAudioSource.Stop();

        backgroundAudioSource.PlayOneShot(GetAudioClip(sound, true));
    }

    //Audio 2D, como el background
    public static void PlaySound(Sound sound)
    {
        if (CanPlay(sound))
        {

            if (u_AudioGameObject == null)
            {
                u_AudioGameObject = new GameObject("Unique Sound Object");
                u_AudioSource = u_AudioGameObject.AddComponent<AudioSource>();
            }

            u_AudioSource.PlayOneShot(GetAudioClip(sound,false));

        }
    }

    private static AudioClip GetAudioClip(Sound sound, bool background)
    {
        foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.Instance.soundsArray)
        {
            if(soundAudioClip.sound == sound)
            {
                if (background)
                {
                    backgroundAudioSource.volume = soundAudioClip.volume;
                    backgroundAudioSource.pitch = soundAudioClip.pitch;
                }
                else
                {
                    u_AudioSource.volume = soundAudioClip.volume;
                    u_AudioSource.pitch = soundAudioClip.pitch;
                }


                return soundAudioClip.audio;
            }
        }

        //Aqui no deberia llegar nunca, a menos que no exista ese nobmre de audio, pero como
        //usamos Enums, no pasara
        return null;
    }

    private static GameAssets.SoundAudioClip GetAudioClipObject(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.Instance.soundsArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip;
            }
        }
        return null;
    }

    //Se puede usar en caso de que querramos agregar restricciones de que tan frecuente debe sonar un sonido
    private static bool CanPlay(Sound sound)
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
