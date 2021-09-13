using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonLoadScene : MonoBehaviour
{
    public void LoadScene(int buildIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
    }

    public void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.GetSceneByBuildIndex(index) != null)
        {
            LoadScene(index);
        }
        else
        {
            LoadScene(0);

        }
    }


}
