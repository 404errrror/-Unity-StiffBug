using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MEditorMenuManager : MonoSingleton<MEditorMenuManager>
{
    public GameObject parentCanvas;
    public GameObject menuItemMover;
    public GameObject menuItemPref;
    public GameObject menuItemRowPref;

    private List<EditorMenuNode> selectedList;
    private List<EditorMenuNode> rootNodeList;
    private List<EditorMenuNode> nodes;

    void Start()
    {
        selectedList = new List<EditorMenuNode>();
        rootNodeList = new List<EditorMenuNode>();
        nodes = new List<EditorMenuNode>();

        CompileNodes();
        InitNodeObject();
    }

    void CompileNodes()
    {
        var editorMenuTable = EditorMenuTable.Instance.GetAll();
        foreach (var row in editorMenuTable)
        {
            EditorMenuNode node = new EditorMenuNode(row.Key);
            nodes.Add(node);

            if(row.Value.parent == "")
            {
                rootNodeList.Add(node);
            }
            else
            {
                // 노드의 Parent 찾기
                for(int i = nodes.Count - 1; i >= 0; --i)
                {
                    if(nodes[i].Alias == row.Value.parent)
                    {
                        nodes[i].ChildNodes.Add(node);
                        break;
                    }

                    // Parent 를 찾지 못했다면
                    if(i == 0)
                    {
                        Debug.LogError("MEDitorMenuManager.cs ComileNode(), <" + row.Value.alias + "> 에서 Parent(" + row.Value.parent + ") 를 찾지 못했습니다!!" +
                            "부모는 테이블 위치에서 자신보다 위에 존재해야합니다!");
                    }
                }
            }

        }
    }

    void InitNodeObject()
    {
        int objectCount = rootNodeList.Count;

        /* Generate menuItem Row Scroll View */
        GameObject itemRow = Instantiate(menuItemRowPref);
        itemRow.transform.SetParent(menuItemMover.transform);
        itemRow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,100);
        itemRow.transform.localScale = Vector3.one;

        /* Generate menuItem */
        for (int i = 0; i < objectCount; ++i)
        {
            GameObject newObject = Instantiate(menuItemPref);

            /* Transform Settings */
            newObject.transform.SetParent(itemRow.transform.Find("Viewport").Find("Content"));
            newObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(250 * i - 500,0);
            newObject.transform.localScale = Vector3.one;
            newObject.name = "MenuItem_" + i;

            MMenuItem menuItem = newObject.GetComponent<MMenuItem>();
            EditorMenuTable.EditorMenuProperty menuProperty = EditorMenuTable.Instance.GetProperty(rootNodeList[i].Alias);
            menuItem.SetImage(menuProperty.imagePath);
        }

        /* Item의 시작 애니메이션 트리거 */
        menuItemMover.GetComponent<Animator>().SetTrigger("Start");
    }
}
