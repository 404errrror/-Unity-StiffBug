using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MTheater : MonoBehaviour
{
    public GameObject focusCharacter;
    public string theaterAlias;

    TextBoxSet textBoxSetInfo;
    GameObject textBoxPref;

    bool isOpen;

    void Start()
    {
        var theaterProperty = TheaterTable.Instance.GetProperty(theaterAlias);

        textBoxSetInfo = Resources.Load<TextBoxSet>("TextBoxResources/" + theaterProperty.textBoxType);
        textBoxPref = Resources.Load<GameObject>("Prefabs/Theater/TextBoxInterface");

        isOpen = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isOpen == false && collision.CompareTag("Player") == true)
            GenerateTextBox();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isOpen == true && collision.CompareTag("Player") == true)
            RemoveTextBox();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void GenerateTextBox()
    {
        var theaterProperty = TheaterTable.Instance.GetProperty(theaterAlias);

        var textBoxObject = transform.Find("TextBox");
        if (textBoxObject == null)
        {
            textBoxObject = Instantiate(textBoxPref, transform).transform;
            textBoxObject.name = "TextBox";
        }

        var textBoxComp = textBoxObject.GetComponent<MTextBox>();

        textBoxComp.SetTextSprites(textBoxSetInfo.textBoxLineSprites, textBoxSetInfo.textBoxColorSprites);
        textBoxComp.SetText(ref theaterProperty.stringData);

        isOpen = true;
    }

    private void RemoveTextBox()
    {
        transform.Find("TextBox").GetComponent<Animator>().SetTrigger("Close");
        isOpen = false;
    }
}
