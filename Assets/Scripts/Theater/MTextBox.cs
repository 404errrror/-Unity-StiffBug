using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTextBox : MonoBehaviour
{
    Sprite[] boxLineSprites;
    Sprite[] boxColorSprites;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTextSprites(Sprite[] lineSprites, Sprite[] colorSprites)
    {
        boxLineSprites = lineSprites;
        boxColorSprites = colorSprites;

        // ToDo. Null Check
        transform.Find("LineSprite").GetComponent<SpriteRenderer>().sprite = boxLineSprites[0];
        transform.Find("ColorSprite").GetComponent<SpriteRenderer>().sprite = boxColorSprites[0];
    }
}
