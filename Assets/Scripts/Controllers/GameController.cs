
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public bool godMode;
    public GameObject bgMusicPrefab;

    private GameObject CameraHolder;
    private CameraShake CameraShaker;
    public LevelLoader loader;
    private HealthBar healthBar;
    private BoostBar boostBar;
    private GameObject bgMusicObject;

    private GameObject GamePlane;
    private AudioManager.Sound backgroundMusic;//Se puede cambiar
    private int lifePoints;
    private float posToChangeLevel;

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
            lifePoints = 100;
            backgroundMusic = AudioManager.Sound.Background;
            //AudioManager.PlayAsBackground(backgroundMusic);
            if(AudioManager.GetBGObject() == null)
            {
                bgMusicObject = GameObject.Instantiate(bgMusicPrefab) as GameObject;
                AudioManager.SetBGObject(bgMusicObject);
                AudioManager.SetAudioSourceBG(bgMusicObject.GetComponent<AudioSource>());
            }
                
        }
        GetLoader();
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

        if (SceneManager.GetActiveScene().buildIndex == 3)
            if (Input.GetKeyDown(KeyCode.Escape))
                loader.LoadMainMenu();
    }

    public void GetGameObjects()
    {
        GamePlane = GameObject.Find("GamePlane");
        CameraHolder = GamePlane.transform.Find("CameraParent").gameObject;
        CameraShaker = CameraHolder.GetComponentInChildren<CameraShake>();
        healthBar = GameObject.Find("Canvas").GetComponentInChildren<HealthBar>();
        boostBar = GameObject.Find("Canvas").GetComponentInChildren<BoostBar>();

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                posToChangeLevel = 3300f;
                break;
            case 2:
                posToChangeLevel = 2050f;
                break;
        }

    }

    public void GetLoader()
    {
        loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }
    public void SetLifePoints(int value)
    {
        if (godMode) return;

        this.lifePoints = Mathf.Clamp(this.lifePoints + value, 0, 100);
        healthBar.SetHealth(this.lifePoints);

        if (lifePoints == 0)
        {
            GameController.Instance.loader.RestarLevel();
            GamePlane.GetComponentInChildren<PlayerController>().StopPlaying();
        }
            
    }

    public void SetBoostPoints(float value)
    {
        boostBar.SetValue(value);
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

        /*GameAssets.SoundAudioClip sac = AudioManager.GetSoundAudioClip(sound);
        AudioSource source = bgMusicObject.GetComponent<AudioSource>();
        if (source.isPlaying)
            source.Stop();

        source.volume = sac.volume;
        source.pitch = sac.pitch;

        source.PlayOneShot(sac.audio);*/
    }

    public float GetPosToChangeLevel()
    {
        return posToChangeLevel;
    }

}
