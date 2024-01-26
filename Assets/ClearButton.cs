using System;
using UnityEngine;

public class ClearButton : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private PauseButton pauseButton;

    public void OnPress()
    {
        grid.Clear();
        pauseButton.OnPress();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            OnPress();
    }
}
