using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public void print()
    {
        print("amil");
    }

    int sceneIndex = 1;

    public void loadLevel()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
