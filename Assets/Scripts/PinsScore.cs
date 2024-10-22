using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PinsScore : MonoBehaviour
{
    public Pin[] pins; 
    private Coroutine scoreRecordCoroutine;
    public Button[] pinUIImages;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            if (scoreRecordCoroutine == null)
            {
                scoreRecordCoroutine = StartCoroutine(CheckPinStatusOverTime(4f));
                //Debug.Log("트리거 발생");
            }
        }
    }

    private IEnumerator CheckPinStatusOverTime(float duration)
    {
        yield return new WaitForSeconds(duration);

        int fallenPinCount = CheckPinStatus(); 

        if (fallenPinCount > 0)
        {
            //Debug.Log($"핀 수: {fallenPinCount}");
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

        for (int i = 0; i < pins.Length; i++)
        {
            float angleDifferenceX = Quaternion.Angle(pins[i].transform.localRotation, Quaternion.Euler(0, 0, 0));
            float angleDifferenceZ = Quaternion.Angle(pins[i].transform.localRotation, Quaternion.Euler(0, 0, 0));

            if (angleDifferenceX >= 30 || angleDifferenceZ >= 30)
            {
                pins[i].isFallen = true;
                fallenPinCount++;

                if (pinUIImages != null && i < pinUIImages.Length)
                {
                    pinUIImages[i].image.color = Color.gray;
                }
            }
            else
            {
                pins[i].isFallen = false;

                if (pinUIImages != null && i < pinUIImages.Length)
                {
                    pinUIImages[i].image.color = Color.white; 
                }
            }
        }

        return fallenPinCount;
    }


    public void ResetPinUI()
    {
        for (int i = 0; i < pinUIImages.Length; i++)
        {
            pinUIImages[i].image.color = Color.white; 
        }
    }

}
