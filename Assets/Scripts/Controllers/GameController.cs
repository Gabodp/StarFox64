using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

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

    }

    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;
    }

    public void setLifePoints(int value)
    {
        this.lifePoints = Mathf.Clamp(this.lifePoints + value, 0, 100);
    }

}
