using UnityEngine;

/// <summary>
/// Class that automates the movement of the ground.
/// </summary>
public class FloorMovement : MonoBehaviour
{
    float speed = 0;
    [SerializeField] float end = 0;
    [SerializeField] float begin = 0;

    private void OnEnable()
    {
        GameManager9.ChangeSpeed += ChangeSpeed;
        GameManager9.StopMovement += ChangeSpeed;
    }

    private void OnDisable()
    {
        GameManager9.ChangeSpeed -= ChangeSpeed;
        GameManager9.StopMovement -= ChangeSpeed;
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        
        if (transform.position.x <= end)
        {
            Vector2 startPosition = new Vector2(begin, transform.position.y);
            transform.position = startPosition;
        }
    }

    /// <summary>
    /// Function called through the delegate to change the speed of the object.
    /// </summary>
    /// <param name="newSpeed">The speed we want to set.</param>
    void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
