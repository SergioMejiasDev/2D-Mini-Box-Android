using System.Collections;
using UnityEngine;

/// <summary>
/// Class that manages the princess animations.
/// </summary>
public class PrincessAnimation : MonoBehaviour
{
    [SerializeField] GameObject helpMessage = null;

    [SerializeField] Animator anim;

    private void OnEnable()
    {
        GameManager7.Stop += StopAnimation;
        GameManager7.Reset += ResetScene;
    }

    private void OnDisable()
    {
        GameManager7.Stop -= StopAnimation;
        GameManager7.Reset -= ResetScene;
    }

    /// <summary>
    /// Function called through delegate that stops all animations.
    /// </summary>
    void StopAnimation()
    {
        StopAllCoroutines();

        anim.SetBool("Movement", false);
        helpMessage.SetActive(false);
    }

    /// <summary>
    /// Function called through the delegate that resets the animations.
    /// </summary>
    void ResetScene()
    {
        helpMessage.SetActive(false);

        StartCoroutine(Animate());
    }

    /// <summary>
    /// Coroutine that synchronizes the animation of the princess.
    /// </summary>
    /// <returns></returns>
    IEnumerator Animate()
    {
        while (true)
        {
            yield return new WaitForSeconds(4);

            anim.SetBool("Movement", true);
            helpMessage.SetActive(true);

            yield return new WaitForSeconds(2);

            anim.SetBool("Movement", false);
            helpMessage.SetActive(false);
        }
    }
}
