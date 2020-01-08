using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataTableManager : MonoSingleton<DataTableManager>
{
    void Awake()
    {
        EditorMenuTable.Instance._Init();

        Debug.Log(EditorMenuTable.Instance.GetProperty("Block").name);
    }
}
