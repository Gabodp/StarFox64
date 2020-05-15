using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public bool godMode;
    public GameObject CameraHolder;
    public CameraShake CameraShaker;
    public LevelLoader loader;

    private AudioManager.Sound backgroundMusic;//Se puede cambiar
    private int lifePoints;
    private float tiempo;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        AudioManager.Initialize();
    }
    // Start is called before the first frame update
    void Start()
    {
        lifePoints = 80;
        tiempo = 0;
        godMode = false;
        backgroundMusic = AudioManager.Sound.Background;
        AudioManager.PlayAsBackground(backgroundMusic);
    }

    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.G))
            godMode = !godMode;
    }

    public void SetLifePoints(int value)
    {
        if (godMode) return;

        this.lifePoints = Mathf.Clamp(this.lifePoints + value, 0, 100);
    }

    public void SetCameraZoom(float zoom, float duration)
    {
        CameraHolder.transform.DOLocalMove(new Vector3(0, 0, zoom), duration);
    }

    public void FieldOfView(float fov)
    {
        CameraHolder.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = fov;
    }

    public void ShakeCamera(float magnitude , float duration)
    {
        CameraShaker.ShakeCamera(magnitude, duration);
    }

    public void SetBackgroundMusic(AudioManager.Sound sound)
    {
        backgroundMusic = sound;
        AudioManager.PlayAsBackground(backgroundMusic);
    }
}
