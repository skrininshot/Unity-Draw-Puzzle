using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{ 
    [SerializeField] private GameObject _victoryPopup;
    [SerializeField] private GameObject _gameOverPopup;
    private Animator _animator;
    private Level _level;
    private int _nextSceneBuildIndex;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        GetComponent<UnityEngine.UI.Image>().enabled = true;
    }

    private void Start()
    {
        _level = Level.singleton;

        if (_level is not null)
        {
            _level.onVictory += Victory;
            _level.onGameOver += GameOver;
        }

        if (!_nextSceneBuildIndex.Equals(0))
            SaveSystem.lastPlayedLevel = _nextSceneBuildIndex;
    }

    private void OnDisable()
    {
        if (_level is not null)
        {
            _level.onVictory -= Victory;
            _level.onGameOver -= GameOver;
        }
    }

    public void BackToMenu()
    {
        FadeInScene(0);
    }

    private void GameOver(GameOverReason reason = 0)
    {
        _gameOverPopup.SetActive(true);
    }

    private void Victory()
    {
        _victoryPopup.SetActive(true);
        int passedLevels = SaveSystem.passedLevels;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (passedLevels < nextSceneIndex)
            SaveSystem.passedLevels = nextSceneIndex;
    }

    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int passedLevels = SaveSystem.passedLevels;

        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex && passedLevels >= nextSceneIndex)
            FadeInScene(nextSceneIndex);
    }

    public void RestartLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        FadeInScene(currentScene);
    }

    private void FadeInScene(int sceneBuildIndex)
    {
        _nextSceneBuildIndex = sceneBuildIndex;
        _animator.SetTrigger("FadeIn");
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene(_nextSceneBuildIndex);
    }

    public void LastPlayedLevel()
    {
        FadeInScene(SaveSystem.lastPlayedLevel);
    }

    public void MaxPassedLevel()
    {
        FadeInScene(SaveSystem.passedLevels);
    }

    public void PreviousLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;

        if (nextSceneIndex > 0)
            FadeInScene(nextSceneIndex);
    }

    public void ClearData()
    {
        SaveSystem.ClearData();
        RestartLevel();
    }
}