using UnityEngine;

/// <summary>
/// Class that is in charge of activating certain colliders in the crocodiles and turtles that kill the player.
/// </summary>
public class FroggerEnableCollider : MonoBehaviour
{
    [SerializeField] GameObject boxCollider = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enable">1 to enable the colliders, other to disable.</param>
    public void EnableColliders(int enable)
    {
        if (boxCollider == null)
        {
            return;
        }

        if (enable == 1)
        {
            boxCollider.SetActive(true);
        }

        else
        {
            boxCollider.SetActive(false);
        }
    }
}
