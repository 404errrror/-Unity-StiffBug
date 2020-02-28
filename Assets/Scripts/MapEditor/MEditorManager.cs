using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MEditorManager : MonoSingleton<MEditorManager>
{
    public GameObject gridParent;
    public GameObject gridLine;
    [Range(0.1f, 1.0f)]
    public float gridSpace;

    // Start is called before the first frame update
    void Start()
    {
        //GenerateGrid(gridSpace);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateGrid(float space)
    {
        int gridNum = (int)(20.0f / space);

        for(int i = -gridNum / 2; i < gridNum / 2; ++i)
        {
            GameObject horizontalLiine = Instantiate(gridLine, gridParent.transform);
            horizontalLiine.transform.localPosition = new Vector3(i * space, 0.0f, 10.0f);

            GameObject verticalLine = Instantiate(gridLine, gridParent.transform);
            verticalLine.transform.localPosition = new Vector3(0.0f, i * space, 10.0f);
            verticalLine.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
        }
    }
}
