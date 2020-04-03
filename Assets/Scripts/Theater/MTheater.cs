using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MTheater : MonoBehaviour
{
    public GameObject focusCharacter;
    public string theaterAlias;


    // Start is called before the first frame update
    void Start()
    {
        var theaterProperty = TheaterTable.Instance.GetProperty(theaterAlias);
        GenerateTextBox(theaterProperty.textBoxType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var theaterProperty = TheaterTable.Instance.GetProperty(theaterAlias);
        GenerateTextBox(theaterProperty.textBoxType);

    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void GenerateTextBox(string textType)
    {
        var textBoxInfo = Resources.Load<TextBoxSet>("TextBoxResources/" + textType);
        var textBoxPrefab = Resources.Load<GameObject>("Prefabs/Theater/TextBoxInterface");
        var textBoxObject = Instantiate(textBoxPrefab, transform);
        var textBoxComp = textBoxObject.GetComponent<MTextBox>();

        textBoxComp.SetTextSprites(textBoxInfo.textBoxLineSprites, textBoxInfo.textBoxColorSprites);
        
    }
}
