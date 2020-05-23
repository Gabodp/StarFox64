using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public bool godMode;
    private GameObject CameraHolder;
    private CameraShake CameraShaker;
    public LevelLoader loader;
    private HealthBar healthBar;

    private GameObject GamePlane;
    private AudioManager.Sound backgroundMusic;//Se puede cambiar
    private int lifePoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        AudioManager.Initialize();
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        Debug.Log("Onsceneloaded called");
        if(scene.buildIndex == 1 || scene.buildIndex == 2)
        {
            GetGameObjects();
            backgroundMusic = AudioManager.Sound.Background;
            AudioManager.PlayAsBackground(backgroundMusic);
        }
    }

    void Start()
    {
        lifePoints = 100;
        healthBar.SetHealth(this.lifePoints);
        godMode = false;
        
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            godMode = !godMode;
        if (godMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
                loader.LoadNextLevel();
            if (Input.GetKeyDown(KeyCode.Alpha1))
                loader.LoadPreviousLevel();
        }
    }

    public void GetGameObjects()
    {
        print("Consiguio los objetos");
        GamePlane = GameObject.Find("GamePlane");
        loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        CameraHolder = GamePlane.transform.Find("CameraParent").gameObject;
        CameraShaker = CameraHolder.GetComponentInChildren<CameraShake>();
        healthBar = GameObject.Find("Canvas").GetComponentInChildren<HealthBar>();
    }

    public void SetLifePoints(int value)
    {
        if (godMode) return;

        this.lifePoints = Mathf.Clamp(this.lifePoints + value, 0, 100);
        healthBar.SetHealth(this.lifePoints);
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
