using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    private GroundPieceController[] allGroundpieces;



    void Start()
    {
        SetupNewLevel();
    }

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        } else if (singleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void SetupNewLevel()
    {
        allGroundpieces = FindObjectsOfType<GroundPieceController>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }

    public void CheckComplete()
    {
        bool isFinished = true;

        for (int i = 0; i < allGroundpieces.Length; i++)
        {
            if (allGroundpieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }

            if (isFinished)
            {
                NextLevel();
            }
        }
    }

    private void NextLevel()
    {
        int sceneIndex = 0;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            sceneIndex = 0;
        }
        sceneIndex++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + sceneIndex);
    }
}
