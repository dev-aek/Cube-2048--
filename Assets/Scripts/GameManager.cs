using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int gameLevel = 1;
    [SerializeField] public GameObject endPanel;
    [SerializeField] public GameObject losePanel;

    [SerializeField] private TMP_Text m_TextComponent;
    [SerializeField] private Camera _mainCamera;


    [SerializeField] private Animator animator;
    [SerializeField] public GameObject playerCube;
    [SerializeField] private PlayerController playerController;

    [SerializeField] public GameObject enemyPrefab;

    public GameObject enemy;
    public GameObject enemy1;
    public GameObject enemy2;
    [SerializeField] public bool boolEnemy;
    [SerializeField] public int adCounter = 0;



    public static GameManager Instance { get; set; }
    private void Awake()
    {
        Instance = this;
        gameLevel = PlayerPrefs.GetInt("Game Level");
        _mainCamera.orthographicSize = 7 + ((gameLevel - 1) * 1.1f);
        Debug.Log(gameLevel);
        //gameLevel = 1;
        TowerManager.Instance.Create();

        //_mainCamera.orthographicSize = gameLevel + 6;



    }

    private void Start()
    {
        Debug.Log(gameLevel);
        m_TextComponent.text = gameLevel.ToString();
    }

    public void CreateNewLevel()
    {
        enemy = new GameObject();
        enemy1 = new GameObject();
        enemy2 = new GameObject();

        TowerManager.Instance.Create();
        endPanel.active = false;
        losePanel.active = false;

        m_TextComponent.text = gameLevel.ToString();
        _mainCamera.orthographicSize = 7 + ((gameLevel-1) * 1.1f);
        animator.SetBool("IsFall", false);
        animator.SetBool("IsLose", false);

        playerCube.SetActive(false);

        var tile = TowerManager.Instance.GetTile(Vector2Int.zero);
        playerController.Move(tile);

        if (gameLevel >= 5 && gameLevel <= 7)
        {
            enemy = Instantiate(enemyPrefab);
        }
        else if (gameLevel >= 8 && gameLevel <= 10)
        {
            enemy = Instantiate(enemyPrefab);
            enemy1 = Instantiate(enemyPrefab);

        }
        else if (gameLevel >= 11)
        {
            enemy = Instantiate(enemyPrefab);
            enemy1 = Instantiate(enemyPrefab);
            enemy2 = Instantiate(enemyPrefab);

        }

    }

    public void destroyEnemy()
    {
        Destroy(enemy);
        Destroy(enemy1);
        Destroy(enemy2);

    }

    public void cleanSaves()
    {

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Game Level",1);
        gameLevel = PlayerPrefs.GetInt("Game Level");

    }
}
