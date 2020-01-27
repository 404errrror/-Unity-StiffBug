using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MMenuItem : MonoBehaviour
{
    public Image imageObject;
    public Image textImageObject;

    private Sprite imageSprite;
    private Sprite textSprite;

    public string Alias { get; set; }

    void Start()
    {
        RefreshImage();
    }

    public void SetImage(string resourcePath)
    {
        imageSprite = Resources.Load<Sprite>(resourcePath);
        if (imageSprite == null)
            Debug.LogWarning("MMenuItem.cs SetImage(), <" + resourcePath + "> 에 sprite 파일이 존재하지 않습니다!!");
    }

    public void SetTextImage(string resourcePath)
    {
        textSprite = Resources.Load<Sprite>(resourcePath);
        if (textSprite == null)
            Debug.LogWarning("MMenuItem.cs SetImage(), <" + resourcePath + "> 에 sprite 파일이 존재하지 않습니다!!");
    }

    public void ClickEvent()
    {
        MEditorMenuManager.Instance.SelectedItem(this);
    }

    private void RefreshImage()
    {
        imageObject.sprite = imageSprite;
        textImageObject.sprite = textSprite;
    }
}
