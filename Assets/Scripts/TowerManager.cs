using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

    [SerializeField] private TileController tilePrefab;
    [SerializeField] private int amount;
    [SerializeField] private GameObject camera;

    [SerializeField] private List<TileController> listTile = new List<TileController>();
    [SerializeField] public GameObject parentObject;

    public static TowerManager Instance { get; set; }
    private void Awake()
    {
        Instance = this;

    }

    public TileController GetTile(Vector2Int coordinate)
    {
        return listTile.Find(x => x.coordinate == coordinate);
    }

    private void Start()
    {
    }

    [ContextMenu(nameof(Create))]
    public void Create()
    {
        var cameraYvalue = new float();

        Create(GameManager.Instance.gameLevel+1);
        if (GameManager.Instance.gameLevel < 5)
            cameraYvalue = GameManager.Instance.gameLevel - 1f;
        else if (GameManager.Instance.gameLevel < 8) cameraYvalue = GameManager.Instance.gameLevel - 2f;
        else if (GameManager.Instance.gameLevel < 12) cameraYvalue = GameManager.Instance.gameLevel - 4f;
        else if (GameManager.Instance.gameLevel < 20) cameraYvalue = GameManager.Instance.gameLevel - 7f;
        else  cameraYvalue = GameManager.Instance.gameLevel - 10f;


        camera.transform.position = new Vector3(GameManager.Instance.gameLevel, cameraYvalue, camera.transform.position.z);

    }

    private void Create(int firstStepCubeAmount)
    {
        listTile = new List<TileController>();
        parentObject = new GameObject()
        {
            transform = { name = "Parent" }
        };

        Vector3 position = Vector3.zero;
        Vector2Int coordinate = Vector2Int.zero;
        for(int step = firstStepCubeAmount,height = 0; step > 0; step--,height++)
        {
            for(int count = 0; count < step; count++)
            {
                var tile = Instantiate(tilePrefab,position, Quaternion.identity, parentObject.transform);

                tile.coordinate= coordinate;
                tile.transform.name = $"Tile [{coordinate.x},{coordinate.y}]";
                listTile.Add(tile);

                coordinate.x += 2;

                position.x++;
                position.z++;

            }
            coordinate.y++;
            coordinate.x = (height+1);

            position.y++;

            position.z=(height+1);
            position.x=0;
        }
    }

    public bool HasUnMark()
    {
        foreach(var tile in listTile)
        {
            if (!tile.greenObject.active)
            {
                return true;
            }
        }
        return false;

    }

}
