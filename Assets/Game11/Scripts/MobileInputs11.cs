﻿using UnityEngine;

/// <summary>
/// Class with the delegate that activates the inputs in Game 02.
/// </summary>
public class MobileInputs11 : MonoBehaviour
{
    public delegate void Inputs11Delegate(int direction);
    public static event Inputs11Delegate Button;

    /// <summary>
    /// Function that is called by pressing the buttons on the screen.
    /// </summary>
    /// <param name="buttonPressed">1 up, 3 down, 5 disable.</param>
    public void PressButtons(int buttonPressed)
    {
        if (Button != null)
        {
            Button(buttonPressed);
        }
    }
}
