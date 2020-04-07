using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class MTextBox : MonoBehaviour
{
    Sprite[] boxLineSprites;
    Sprite[] boxColorSprites;

    SpriteRenderer lineSpriteRenderer;
    SpriteRenderer colorSpriteRenderer;
    TextMeshProUGUI textMesh;

    Animator animator;

    int lineSpriteIndex;
    int colorSpriteIndex;

    void Awake()
    {
        lineSpriteIndex = 0;
        colorSpriteIndex = 0;

        lineSpriteRenderer = transform.Find("LineSprite").GetComponent<SpriteRenderer>();
        colorSpriteRenderer = transform.Find("ColorSprite").GetComponent<SpriteRenderer>();
        textMesh = transform.Find("Canvas").Find("TextMesh").GetComponent<TextMeshProUGUI>();

        animator = transform.GetComponent<Animator>();
    }

    IEnumerator FrameAnimation()
    {
        for (; ; )
        {
            lineSpriteRenderer.sprite = boxLineSprites[lineSpriteIndex];
            colorSpriteRenderer.sprite = boxColorSprites[colorSpriteIndex];

            ++lineSpriteIndex;
            ++colorSpriteIndex;

            if (lineSpriteIndex >= boxLineSprites.Length)
                lineSpriteIndex = 0;
            if (colorSpriteIndex >= boxColorSprites.Length)
                colorSpriteIndex = 0;

            yield return new WaitForSeconds(0.4f);
        }
    }

    public void SetTextSprites(Sprite[] lineSprites, Sprite[] colorSprites)
    {
        boxLineSprites = lineSprites;
        boxColorSprites = colorSprites;

        lineSpriteIndex = 0;
        colorSpriteIndex = 0;

        StopCoroutine("FrameAnimation");
        StartCoroutine("FrameAnimation");

        animator.SetTrigger("Open");
    }

    public void SetText(ref string text)
    {
        textMesh.text = text;
    }
}
