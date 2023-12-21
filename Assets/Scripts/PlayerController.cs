using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private bool IsPressLeft => Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
    private bool IsPressRight => Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
   // private bool IsPressUp => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

    private TileController _current;

    private Vector2
            _startedPos, // hareketin baþladýðý konum
            _delta; // hareketin devam ettiði konum

    private Vector2 _value; // hareket sonucunda yansýtýlan deðer

    public Animator animator;

    public AudioSource audioWin;
    public AudioSource audioStep;
    public AudioSource audioLose;


    public Vector2 GetValue() // Bu fonksiyonumuz karakter hareketini yazdýðýmýz scriptten çaðýrýlacak ve hareket koduna dahil edilecek.
    {
        return _value;
    }
    public float maxDistance = 100f; // kullanýcýnýn hareketinin maksimum ne kadar olacaðý

    private void Awake()
    {
        animator = transform.GetComponentInChildren<Animator>();
    }

    private void Update()
    {

        

        // bool IsPressUp => _value.y > 0;



    }

    private void FixedUpdate()
    {
        /*if (Input.GetMouseButton(0))
        {

                MousePosCalculate();

        }*/


        /*if (ValueLeftPressed())
        {
            var tile = _current.GetNeighbour(ValueYPressed() ? Direction.UpLeft : Direction.DownLeft);
            Move(tile);
        }

        if (ValueRightPressed())
        {
            var tile = _current.GetNeighbour(ValueYPressed() ? Direction.UpRight : Direction.DownRight);
            Move(tile);
        }*/
    }


    private void Start()
    {
        var tile = TowerManager.Instance.GetTile(Vector2Int.zero);
        Move(tile);
    }

   /* private void MousePosCalculate()
    {
        if (Input.GetMouseButtonDown(0))
            _startedPos = (Vector2)Input.mousePosition; // baþlangýç konumunu alýyoruz

        if (Input.GetMouseButtonUp(0))
        {
            // Deðerleri sýfýrlýyoruz hareket bittiði zaman
            _delta = Vector2.zero;
            _startedPos = Vector2.zero;
            _value = Vector2.zero;
        }

        if (!Input.GetMouseButton(0)) return; // hareket ediyorsa hesaplamalarý yapýyoruz
        _delta = (Vector2)Input.mousePosition - _startedPos;
        _delta.x = Mathf.Clamp(_delta.x, -maxDistance, maxDistance);
        _delta.y = Mathf.Clamp(_delta.y, -maxDistance, maxDistance);
        //StartCoroutine(ExampleCoroutine(0.15f));

        _value = _delta / maxDistance;
       // _startedPos = (Vector2)Input.mousePosition;

        Debug.Log(_value);
    }*/

   /* private bool ValueYPressed()
    {
        if(_value.y > 0.05f) return true;

        return false;
    }
    private bool ValueRightPressed()
    {
        if (_value.x > 0.10f) return true;

        return false;
    }


    private bool ValueLeftPressed()
    {
        if (_value.x < -0.10f) return true;

        return false;
    }*/

    /* void Update()
     {

       /*  if(IsPressLeft)
         {
             var tile = _current.GetNeighbour(IsPressUp ? Direction.UpLeft : Direction.DownLeft);
             Move(tile);
         }

         if(IsPressRight)
         {
             var tile = _current.GetNeighbour(IsPressUp ? Direction.UpRight : Direction.DownRight);
             Move(tile);
         }
     }
 */

    public void Move(TileController tileController)
    {
        if (!tileController) return;
        transform.position = tileController.snapPoint.position;
        if (!tileController.greenObject.active)
        {
            tileController.greenObject.active = true;
        }
        else
        {
            tileController.greenObject.active = false;
        }
        _current = tileController;

        var finish = !TowerManager.Instance.HasUnMark();
        if (finish)
        {
            GameManager.Instance.destroyEnemy();
            transform.position = new Vector3(GameManager.Instance.gameLevel, GameManager.Instance.gameLevel-1.5f, 0);

            GameManager.Instance.gameLevel++;
            PlayerPrefs.SetInt("Game Level", GameManager.Instance.gameLevel);
            GameManager.Instance.gameLevel = PlayerPrefs.GetInt("Game Level");
            Destroy(TowerManager.Instance.parentObject);
            //TowerManager.Instance.Create();
            GameManager.Instance.endPanel.active = true;

            /*var tile = TowerManager.Instance.GetTile(Vector2Int.zero);
            Move(tile);*/

            animator.SetBool("IsFall", true);
            GameManager.Instance.playerCube.SetActive(true);
            audioWin.Play();

            GameManager.Instance.adCounter++;

            Debug.Log(GameManager.Instance.adCounter);

        }
    }

    IEnumerator ExampleCoroutine(float sec, String parameter)
    {
        animator.SetBool(parameter, true);
        yield return new WaitForSeconds(sec);
        animator.SetBool(parameter, false);

    }

    public void MoveRUp()
    {
        var tile = _current.GetNeighbour(Direction.UpRight);
        Move(tile);

        RotateTransformOnY(0);
    }

    public void MoveRDown()
    {
        var tile = _current.GetNeighbour(Direction.DownRight);
        Move(tile);

        RotateTransformOnY(90);

    }
    public void MoveLUp()
    {
        var tile = _current.GetNeighbour(Direction.UpLeft);
        Move(tile);

        RotateTransformOnY(270);
    }
    public void MoveLDown()
    {
        var tile = _current.GetNeighbour(Direction.DownLeft);
        Move(tile);
        RotateTransformOnY(180);

    }

    public void RotateTransformOnY(float rotateY)
    {
        //transform.rotation = new Quaternion(0, rotateY, 0, 0);
        transform.eulerAngles = new Vector3(0, rotateY, 0);
        StartCoroutine(ExampleCoroutine(0.3f,"IsInAir"));
        audioStep.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            RestartGame();
            GameManager.Instance.adCounter++;
            Debug.Log(GameManager.Instance.adCounter);

        }
    }

    public void RestartGame()
    {
        GameManager.Instance.boolEnemy = false;
        GameManager.Instance.destroyEnemy();
        transform.position = new Vector3(GameManager.Instance.gameLevel, GameManager.Instance.gameLevel - 1.5f, 0);

        //GameManager.Instance.gameLevel++;
        PlayerPrefs.SetInt("Game Level", GameManager.Instance.gameLevel);
        GameManager.Instance.gameLevel = PlayerPrefs.GetInt("Game Level");
        Destroy(TowerManager.Instance.parentObject);
        //TowerManager.Instance.Create();
        GameManager.Instance.losePanel.active = true;

        /*var tile = TowerManager.Instance.GetTile(Vector2Int.zero);
        Move(tile);*/

        animator.SetBool("IsLose", true);
        GameManager.Instance.playerCube.SetActive(true);
        audioLose.Play();

    }


}

