using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spriteSwap : MonoBehaviour
{
    public Sprite onSprite;

    public Sprite offSprite;

    private Image image;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void UpdateSprite()
    {
        image.sprite = PauseMenu.IsMuted ? offSprite : onSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
