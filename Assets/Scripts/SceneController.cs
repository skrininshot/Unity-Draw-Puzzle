using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{ 
    private Animator _animator;
    private int _nextSceneBuildIndex;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void BackToMenu()
    {
        FadeIn(0);
    }

    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (SceneManager.sceneCountInBuildSettings >= nextSceneIndex)
            FadeIn(nextSceneIndex);
    }

    public void RestartLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        FadeIn(currentScene);
    }

    private void FadeIn(int sceneBuildIndex)
    {
        _nextSceneBuildIndex = sceneBuildIndex;
        _animator.SetTrigger("FadeIn");
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene(_nextSceneBuildIndex);
    }
}