using System.Collections;
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

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void RestarLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex >= num_scenes) return;

        StartCoroutine(LoadLevel(nextSceneIndex));
    }

    public void LoadPreviousLevel()
    {
        int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (previousSceneIndex < 0) return;

        StartCoroutine(LoadLevel(previousSceneIndex));
    }

    IEnumerator LoadLevel(int level_index)
    {
        transition_anim.SetTrigger("Start");

        yield return new WaitForSeconds(transition_time);

        SceneManager.LoadScene(level_index);
    }
}
