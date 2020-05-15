using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{

    public Animator transition_anim;
    public float transition_time;

    private int num_scenes;

    private void Start()
    {
        num_scenes = SceneManager.sceneCountInBuildSettings;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadNextLevel();
        }
            
    }
    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex >= num_scenes) return;

        StartCoroutine(LoadLevel(nextSceneIndex));
    }

    IEnumerator LoadLevel(int level_index)
    {
        transition_anim.SetTrigger("Start");

        yield return new WaitForSeconds(transition_time);

        SceneManager.LoadScene(level_index);
    }
}
