using UnityEngine;

public class Bumper : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody ballRigidbody = collision.rigidbody;
        if (ballRigidbody != null)
        {
            ballRigidbody.velocity = Vector3.Reflect(ballRigidbody.velocity, Vector3.right);
        }
    }
}
