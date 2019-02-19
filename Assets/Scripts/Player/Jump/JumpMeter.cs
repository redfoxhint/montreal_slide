﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpMeter : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollBar;
    private PlayerMovement player;

    [Header("Jump Configuration")]
    [Tooltip("This value controls the how fast the jump meter increments.")]
    [SerializeField] private float scrollBarValueIncrement; // The amount that the scrollbar value increments by.
    [HideInInspector] public float scrollBarValue; // The amount that the scrollbar is at when we release the spacebar.

    public bool isCalculatingJump;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        isCalculatingJump = false;
    }

    private void Start()
    {
        scrollBar.value = 0f;
    }

    public IEnumerator CalculateJumpForce()
    {
        while (Input.GetButton("Jump")) // Run until we release the space bar.
        {
            if (scrollBarValue >= 1f) // Since the scrollbar value only goes from 0-1, we want to make sure we dont go over 1.
            {
                RestartJump();
                ResetValues();
                yield break;
            }

            scrollBarValue += scrollBarValueIncrement * Time.deltaTime; // Increment jump force amount by the multiplier we set.
            scrollBar.value = scrollBarValue; // Set the scrollbar value to the jump force amount each deltaTime second.
            yield return new WaitForSeconds(Time.deltaTime);
        }

        player.CalculateJump(scrollBarValue);
        isCalculatingJump = false;
        ResetValues();
        yield break;
    }

    public void ResetValues()
    {
        scrollBarValue = 0f;
        scrollBar.value = 0f;
    }

    private void RestartJump()
    {
        ResetValues();
        StartCoroutine((CalculateJumpForce()));
    }
}
