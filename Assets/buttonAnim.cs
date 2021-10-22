using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class buttonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text buttonText;

    private Vector3 bigSize = new Vector3(1.3f, 1.3f, 1.3f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.transform.localScale = bigSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
