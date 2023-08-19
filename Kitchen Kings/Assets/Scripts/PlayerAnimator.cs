using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private PlayerController playerController;

    //private Animator animator;
    private void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("isWalking", playerController.IsWalking());
    }
}
