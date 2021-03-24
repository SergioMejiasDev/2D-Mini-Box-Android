using System.Collections;
using UnityEngine;

/// <summary>
/// Class that manages the animations and actions of the Kong.
/// </summary>
public class KongMovement : MonoBehaviour
{
    [Header("Barrels")]
    [SerializeField] Transform instantiateArea = null;

    [Header("Vertical Barrels")]
    [SerializeField] Transform instantiateVerticalArea = null;
    [SerializeField] GameObject verticalBarrel = null;
    bool firstThrow = false;

    [Header("Components")]
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
    /// Function called to instantiate a barrel next to the Kong.
    /// </summary>
    public void InstantiateBarrel()
    {
        GameObject barrel = ObjectPooler.SharedInstance.GetPooledObject("Game7/Barrel");

        if (barrel != null)
        {
            barrel.transform.position = instantiateArea.position;
            barrel.transform.rotation = Quaternion.identity;
            barrel.SetActive(true);
        }
    }

    /// <summary>
    /// Function called to instantiate a vertical barrel under the Kong.
    /// </summary>
    public void InstantiateVerticalBarrel()
    {
        if (!firstThrow)
        {
            firstThrow = true;

            StartCoroutine(ThrowVerticalBarrel());

            return;
        }

        if (Random.value > 0.05f)
        {
            return;
        }

        StartCoroutine(ThrowVerticalBarrel());
    }

    /// <summary>
    /// Function called from the delegate to stop the Kong animations.
    /// </summary>
    void StopAnimation()
    {
        StopAllCoroutines();

        anim.SetBool("ThrowBarrels", false);
    }

    /// <summary>
    /// Function called from the delegate to reset the Kong animations.
    /// </summary>
    void ResetScene()
    {
        anim.SetBool("ThrowBarrels", true);

        firstThrow = false;
    }

    /// <summary>
    /// Coroutine that synchronizes the animation with the launch of vertical barrels.
    /// </summary>
    /// <returns></returns>
    IEnumerator ThrowVerticalBarrel()
    {
        anim.SetBool("ThrowBarrels", false);

        Instantiate(verticalBarrel, instantiateVerticalArea.position, Quaternion.identity);

        yield return new WaitForSeconds(0.25f);

        anim.SetBool("ThrowBarrels", true);
    }
}
