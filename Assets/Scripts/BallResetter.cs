using UnityEngine;

public class BallResetter : MonoBehaviour
{
    public GameObject ball;
    public Transform resetTarget; 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BackStop"))
        {
            ResetBallPosition();
        }
    }

    public void ResetBallPosition()
    {
        ball.transform.position = resetTarget.position;

        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
            ballRigidbody.velocity = Vector3.zero; 
            ballRigidbody.angularVelocity = Vector3.zero; 
        }
    }
}

