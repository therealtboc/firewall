using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private KeyCode _pauseKey;
    [SerializeField] private GameObject _container;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    private bool _isPaused;

    private void Update()
    {
        if (Input.GetKeyDown(_pauseKey))
        {
            if (_isPaused)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }

    private void OnEnable()
    {
        _resumeButton.onClick.AddListener(ResumeGame);
        _restartButton.onClick.AddListener(RestartGame);
    }

    private void OnDisable()
    {
        _resumeButton.onClick.RemoveListener(ResumeGame);
        _restartButton.onClick.RemoveListener(RestartGame);
    }

    void Open()
    {
        _isPaused = true;
        _container.SetActive(true);
        Time.timeScale = 0;
    }

    void Close()
    {
        _isPaused = false;
        _container.SetActive(false);
        Time.timeScale = 1;
    }

    void ResumeGame()
    {
        Close();
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }
}
