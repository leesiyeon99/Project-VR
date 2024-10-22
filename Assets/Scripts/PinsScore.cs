using System.Collections;
using UnityEngine;

public class PinsScore : MonoBehaviour
{
    public Pin[] pins; 
    private Coroutine scoreRecordCoroutine; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            if (scoreRecordCoroutine == null)
            {
                scoreRecordCoroutine = StartCoroutine(CheckPinStatusOverTime(4f));
                //Debug.Log("Æ®¸®°Å ¹ß»ý");
            }
        }
    }

    private IEnumerator CheckPinStatusOverTime(float duration)
    {
        yield return new WaitForSeconds(duration);

        int fallenPinCount = CheckPinStatus(); 

        if (fallenPinCount > 0)
        {
            //Debug.Log($"ÇÉ ¼ö: {fallenPinCount}");
            ScoreManager.Instance.NotifyPinFallen(fallenPinCount); 
        }
        else
        {
            ScoreManager.Instance.NotifyPinFallen(0);
        }

        scoreRecordCoroutine = null;

        foreach (var pin in pins)
        {
            pin.isFallen = false;
        }
    }

    private int CheckPinStatus()
    {
        int fallenPinCount = 0;

        foreach (var pin in pins)
        {
            float angleDifferenceX = Quaternion.Angle(pin.transform.localRotation, Quaternion.Euler(0, 0, 0));
            float angleDifferenceZ = Quaternion.Angle(pin.transform.localRotation, Quaternion.Euler(0, 0, 0));


            if (angleDifferenceX >= 30 || angleDifferenceZ >= 30)
            {
                //Debug.Log($"{pin.name} ÇÉ ³Ñ¾îÁü");
                pin.isFallen = true; 
                fallenPinCount++;
            }
        }

        return fallenPinCount; 
    }

}
