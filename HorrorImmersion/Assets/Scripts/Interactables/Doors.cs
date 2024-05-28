using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour, IInteractable
{
    [SerializeField] Animator animator;

    private bool isOpen;

    public void Interact()
    {
        gameObject.GetComponent<Collider>().enabled = false;

        if(animator.GetBool("isClose") == true)
        {
            animator.SetBool("isClose", false); // open
            isOpen = true;
        }
        else
        {
            animator.SetBool("isClose", true); // close
            isOpen = false;
        }

        // just to add a slight delay to collision
        gameObject.GetComponent<Collider>().enabled = true;
    }

    public bool isDoorOpen()
    {
        return isOpen;
    }
}
