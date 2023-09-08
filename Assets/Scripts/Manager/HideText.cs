using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideText : MonoBehaviour
{
    // To be called from animation event of the reward text animation 
    public void HideRewardText()
    {
        gameObject.SetActive(false);
    }
}
