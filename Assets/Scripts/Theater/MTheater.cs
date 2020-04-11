using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MTheater : MonoBehaviour
{
    public string theaterAlias;

    TextBoxSet textBoxSetInfo;
    GameObject textBoxPref;

    GameObject textBoxObj;

    bool isOpen;

    void Start()
    {
        var theaterProperty = TheaterTable.Instance.GetProperty(theaterAlias);

        textBoxSetInfo = Resources.Load<TextBoxSet>("TextBoxResources/" + theaterProperty.textBoxType);
        textBoxPref = Resources.Load<GameObject>("Prefabs/Theater/TextBoxInterface");

        var ownerObj = GameObject.Find(theaterProperty.ownerObject);
        if (ownerObj == null)
            Debug.LogError("MTheater.GenerateTextBox(), OwnerObject<" + theaterProperty.ownerObject + ">를 찾지못했습니다.");

        if (ownerObj.transform.Find("Theater").Find("TextBox") == null)
        {
            textBoxObj = GenerateTextBox(ownerObj);
        }

        isOpen = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen == false && collision.CompareTag("Player") == true)
        {
            OpenTextBox();
            MEventManager.Instance.AddActionButtonEvent(NextTextBox);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isOpen == true && collision.CompareTag("Player") == true)
        {
            CloseTextBox();
            MEventManager.Instance.RemoveActionButtonEvent(NextTextBox);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private GameObject GenerateTextBox(GameObject ownerObject)
    {
        var theaterProperty = TheaterTable.Instance.GetProperty(theaterAlias);
        var textBoxObject = Instantiate(textBoxPref, ownerObject.transform.Find("Theater"));

        textBoxObject.name = "TextBox";

        return textBoxObject;
    }

    private void OpenTextBox()
    {
        var theaterProperty = TheaterTable.Instance.GetProperty(theaterAlias);
        var textBoxComp = textBoxObj.GetComponent<MTextBox>();

        textBoxComp.SetTextSprites(textBoxSetInfo.textBoxLineSprites, textBoxSetInfo.textBoxColorSprites);
        textBoxComp.SetText(ref theaterProperty.stringData);

        if (theaterProperty.focusObject != "")
        {
            GameObject focusObj = GameObject.Find(theaterProperty.focusObject);
            if (focusObj != null)
            {
                Camera.main.GetComponent<CameraLimit>().target = focusObj;
            }
        }

        textBoxObj.GetComponent<Animator>().SetBool("IsOpen", true);
        isOpen = true;
    }

    private void CloseTextBox()
    {
        textBoxObj.GetComponent<Animator>().SetBool("IsOpen", false);
        isOpen = false;

        GameObject playerObj = GameObject.FindGameObjectsWithTag("Player")[0];
        if(playerObj != null)
        {
            Camera.main.GetComponent<CameraLimit>().target = playerObj;
        }
    }

    private void NextTextBox()
    {
        StartCoroutine("NextTextBox_");
    }

    private IEnumerator NextTextBox_()
    {
        var theaterProperty = TheaterTable.Instance.GetProperty(theaterAlias);

        textBoxObj.GetComponent<Animator>().SetBool("IsOpen", false);
        isOpen = false;

        yield return new WaitForSeconds(0.5f);

        if(theaterProperty.theaterType != ETheaterType.Loop)
        {
            theaterAlias = theaterProperty.nextAlias;
            var newTheaterProperty = TheaterTable.Instance.GetProperty(theaterAlias);

            var newOwnerObj = GameObject.Find(newTheaterProperty.ownerObject);
            var newTextBoxObj_transform = newOwnerObj.transform.Find("Theater").Find("TextBox");
            if (newTextBoxObj_transform == null)
                textBoxObj = GenerateTextBox(newOwnerObj);
            else
                textBoxObj = newTextBoxObj_transform.gameObject;
        }
        OpenTextBox();
    }
}
