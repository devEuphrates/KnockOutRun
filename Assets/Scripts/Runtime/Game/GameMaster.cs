using Euphrates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    [SerializeReference] IntSO _currentLevel;

    [Header("Triggers and Channels")]
    [SerializeReference] SaveChannelSO _save;
    [SerializeReference] TriggerChannelSO _setLevel;
    [SerializeReference] TriggerChannelSO _levelGenerated;
    [SerializeReference] TriggerChannelSO _nextLevel;

    [Header("UI"), Space]
    [SerializeReference] GameObject _loadingScreen;

    private void OnEnable()
    {
        _levelGenerated.AddListener(LevelReady);
        _nextLevel.AddListener(LoadNexLevel);
    }

    private void OnDisable()
    {
        _levelGenerated.RemoveListener(LevelReady);
        _nextLevel.RemoveListener(LoadNexLevel);
    }

    private void Start() => LoadLevel(null);


    #region Load / Unload Cycle
    void LoadLevel(AsyncOperation _)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        load.completed += SceneLoaded;
    }

    void SceneLoaded(AsyncOperation op)
    {
        // Trigger event that sets the level enviroment
        _setLevel.Invoke();
    }

    void LevelReady()
    {
        // Disable loading screen
        _loadingScreen.SetActive(false);
    }

    void LoadNexLevel()
    {
        // Enable loading screen
        _loadingScreen.SetActive(true);

        // Increment current level and start loading processs
        _currentLevel.Value++;
        AsyncOperation op = SceneManager.UnloadSceneAsync(1);
        op.completed += LoadLevel;
    }
    #endregion
}
