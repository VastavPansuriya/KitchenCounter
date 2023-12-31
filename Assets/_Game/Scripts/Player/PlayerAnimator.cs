using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Player playerObj;

    private Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAnimator.SetBool(IS_WALKING, playerObj.IsWalking());
    }

    private void Update()
    {
        playerAnimator.SetBool(IS_WALKING, playerObj.IsWalking());
    }
}
