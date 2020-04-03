using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTextBoxSet", menuName = "TextBoxSet", order = 2)]

public class TextBoxSet : ScriptableObject {

    public Sprite[] textBoxColorSprites;
    public Sprite[] textBoxLineSprites;
}
