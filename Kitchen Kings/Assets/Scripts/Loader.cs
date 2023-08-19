using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static string scene_to_load;
    public static void Load(string scene)
    {
        scene_to_load = scene;
        SceneManager.LoadScene("LoadingScene");
    
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(scene_to_load);
    }
}
