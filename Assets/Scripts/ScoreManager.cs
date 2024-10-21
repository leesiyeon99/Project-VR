using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : BaseUI
{
    private void Awake()
    {
        Bind();
    }

    private void Start()
    {
        GetUI<TextMeshProUGUI>("1-1 Score").text = "1";
        GetUI<TextMeshProUGUI>("2-1 Score").text = "2";
    }
}
