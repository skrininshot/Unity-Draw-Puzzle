using UnityEngine;

public class LevelsCounterText : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Text _text;
    
    private void Awake()
    {
        _text = GetComponent<UnityEngine.UI.Text>();
    }

    private void Start()
    {
        SetLevelsText();
    }

    private void SetLevelsText()
    {
        int passedLevels = SaveSystem.passedLevels;
        int maxLevelsCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1;
        _text.text = $"Levels: {passedLevels} / {maxLevelsCount}";
    }
}
