using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class ButtonRestart : MonoBehaviour
{
    private Button button = null;
    void Awake()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(RestartLevel);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

}
