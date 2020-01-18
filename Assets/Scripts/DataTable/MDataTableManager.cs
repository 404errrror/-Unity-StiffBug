using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MDataTableManager : MonoSingleton<MDataTableManager>
{
    void Awake()
    {
        EditorMenuTable.Instance._Init();
    }
}
