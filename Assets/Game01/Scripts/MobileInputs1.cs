using UnityEngine;

/// <summary>
/// Class with the delegate that activates the inputs in game 1.
/// </summary>
public class MobileInputs1 : MonoBehaviour
{
    public delegate void Inputs1Delegate(int direction);
    public static event Inputs1Delegate Button;

    /// <summary>
    /// Function that is called by pressing the buttons on the screen.
    /// </summary>
    /// <param name="buttonPressed">1 up, 2 right, 4 left, 5 disable.</param>
    public void PressButtons(int buttonPressed)
    {
        if (Button != null)
        {
            Button(buttonPressed);
        }
    }
}
