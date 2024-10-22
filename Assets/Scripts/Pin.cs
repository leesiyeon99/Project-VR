using UnityEngine;

public class Pin : MonoBehaviour
{
    public float pinFallThreshold; 
    public float toppleLife = 3f; 
    public int tries = 10;
    public bool isFallen = false;
    private Quaternion defaultRotation; 
    private int currentTries; 

    public PinsScore score;

    protected void Awake()
    {
        defaultRotation = transform.localRotation; 
    }

    public void CheckTopple()
    {
        CancelToppleCheck(); 
        CheckRotation(); 
        InvokeRepeating("CheckRotation", 0f, 1f); 
    }

    public void CancelToppleCheck()
    {
        currentTries = 0; 
        CancelInvoke("CheckRotation"); 
        CancelInvoke("HidePin");
    }

    protected void CheckRotation()
    {
        currentTries++;
        if (!Mathf.Abs(Quaternion.Dot(defaultRotation, transform.localRotation)).ApproxEquals(1f, pinFallThreshold))
        {
           //Invoke("HidePin", toppleLife); 
        }
        else if (currentTries > tries)
        {
            CancelToppleCheck(); 
        }
    }

    protected void HidePin()
    {
        gameObject.SetActive(false); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball")) 
        {
            CheckTopple();
        }
    }
}

public static class FloatExtensions
{
    public static bool ApproxEquals(this float a, float b, float tolerance)
    {
        return Mathf.Abs(a - b) < tolerance;
    }
}
