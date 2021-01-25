using UnityEngine;

/// <summary>
/// Class with the delegate that activates the inputs in game 6.
/// </summary>
public class MobileInputs6 : MonoBehaviour
{
    public delegate void Inputs6Delegate(int direction);
    public static event Inputs6Delegate Button;

    /// <summary>
    /// Function that is called by pressing the buttons on the screen.
    /// </summary>
    /// <param name="buttonPressed">1 up, 2 right, 3 down, 4 left.</param>
    public void PressButtons(int buttonPressed)
    {
        if (Button != null)
        {
            Button(buttonPressed);
        }
    }
}
