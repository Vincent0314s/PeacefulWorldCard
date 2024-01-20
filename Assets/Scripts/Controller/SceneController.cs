using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public static void LoadScene(int index) {
        SceneManager.LoadScene(index);
    }

    public static void LoadScene(int index,LoadSceneMode mode)
    {
        SceneManager.LoadScene(index,mode);
    }

    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void LoadScene(string name, LoadSceneMode mode)
    {
        SceneManager.LoadScene(name,mode);
    }

    //public static void LoadScene(SceneInTheBuild scene)
    //{
    //    SceneManager.LoadScene(scene.ToString());
    //}

    public static void NextScene() {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nextSceneIndex += 1;
        LoadScene(nextSceneIndex);
    }

    public static void PreviousScene() {
        int previousSceneIndex = SceneManager.GetActiveScene().buildIndex;
        previousSceneIndex -= 1;
        LoadScene(previousSceneIndex);
    }

    public static int GetCurrentSceneIndex() {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public static void QuitGame() {
        Application.Quit();
    }

}
