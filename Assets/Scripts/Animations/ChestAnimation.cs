using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimation : MonoBehaviour
{
    /*[SerializeField] private Animation openAnimation;
    [SerializeField] private Animation closeAnimation;
    [SerializeField] private Animation emptyAnimation;

    public void OpenChestAnimation()
    {
        openAnimation.Play();
    }

    public void CloseChestAnimation()
    {
        closeAnimation.Play();
    }

    public void EmptyChestAnimation()
    {
        emptyAnimation.Play();
    }*/

    public void OpenChestAnimation()
    {
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().Play("Chest_Open");
    }

    public void CloseChestAnimation()
    {
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().Play("Chest_Close");
    }

    public void EmptyChestAnimation()
    {
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().Play("Chest_Empty");
    }
}