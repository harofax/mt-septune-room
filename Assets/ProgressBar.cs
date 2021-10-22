using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public int minimum;
    public int maximum;
    public int current;
    public Image mask;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating(nameof(GetCurrentFill), 2, 1);
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

    void GetCurrentFill()
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;
    }
}
