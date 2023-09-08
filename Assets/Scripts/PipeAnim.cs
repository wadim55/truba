using UnityEngine;
using System.Collections;

public class PipeAnim : MonoBehaviour {

    [HideInInspector]
    public bool playAnim;     //ссылка на то, когда воспроизводить анимацию

    private Animator Anim=>GetComponent<Animator>();    //ссылка на компонент animator в объекте

   
	private void Update ()
    {
        if (playAnim)
        {
            StartCoroutine(PlayAnim());
        }
            
    }

   private  IEnumerator PlayAnim()
    {
        Anim.Play("PipeAnim");
        yield return new WaitForSeconds(0.33f);
        Anim.Play("PipeIdle");
        playAnim = false;
    }


}
