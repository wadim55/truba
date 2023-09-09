using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PipeScript : MonoBehaviour
{

    private MyAction myAction; // объект инпут системы
    
    [SerializeField]
    private string animalTag;      //ссылка на конкретный объект тега, чтобы разрешить столкновение с определенной трубой
    private bool moving = false;   //это для lerp, которые происходят, когда труба перемещается при нарезании резьбы

    private AudioSource sound => GetComponent<AudioSource>();    
    public AudioClip[] clips;

    private void Awake()
    {
        myAction = new MyAction();
        myAction.map.MoveLeft.performed += context => MoveLeft();
        myAction.map.MoveRight.performed += context => MoveRight();
    }

    private void OnEnable()
    {
       myAction.Enable();
        
    }

    private void OnDisable()
    {
        myAction.Disable();
    }

    void Update ()
    {
        //когда труба выходит за пределы допустимых значений, ее передача
        //ну, это означает, что когда труба перемещается в крайнее правое положение, она пеоеходит в левое
        if (transform.position.x > 5.25f)
        {
            transform.position = new Vector3(-5.25f, transform.position.y);
        }

        if (transform.position.x < -5.25f)
        {
            transform.position = new Vector3(5.25f, transform.position.y);
        }
    }

    //это позволяет обнаружить объект, столкнувшийся с трубой
    void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.tag);
        print(animalTag);
        if (other.gameObject.CompareTag(animalTag))
        {
          //  увеличиваем счет, воспроизводим анимацию трубы
            sound.PlayOneShot(clips[0]);
            other.gameObject.SetActive(false);
            transform.GetChild(0).GetComponent<PipeAnim>().playAnim = true;
            GameManager.instance.currentScore++;
        }
        else
        {
            //проигрыш
            sound.PlayOneShot(clips[1]);
            GameManager.instance.isGameOver = true;
            //трясем камеру
            CameraShake.instance.ShakeCamera(0.05f, 1f);
        }
    }

    //здесь мы перемещаем трубу от последней позиции к новой позиции вправо
    public void MoveRight()
    {
      print("право");
        Vector3 lastPos = transform.position;
        Vector3 newPos = new Vector3(lastPos.x + 1.5f, lastPos.y);

        StartCoroutine(MoveFromTo(lastPos, newPos, 0.2f));
    }
    //здесь мы перемещаем трубу от последней позиции к новой позиции влево
    public void MoveLeft()
    {
        Vector3 lastPos = transform.position;
        Vector3 newPos = new Vector3(lastPos.x - 1.5f, lastPos.y);

        StartCoroutine(MoveFromTo(lastPos, newPos, 0.2f));
    }

   
    IEnumerator MoveFromTo(Vector3 pointA, Vector3 pointB, float time)
    {
        if (!moving)
        {                     
            moving = true;                 
            float t = 0f;
            while (t < 1.0f)
            {
                t += Time.deltaTime / time; 
                transform.position = Vector3.Lerp(pointA, pointB, t); 
                yield return 0;        
            }
            moving = false;             
        }
    }
}
