using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Todo : 풀링 */

public class MEditorMenuManager : MonoSingleton<MEditorMenuManager>
{
    public GameObject parentCanvas;
    public GameObject menuItemRoot;
    public GameObject menuItemPref;
    public GameObject menuItemRowPref;
    public GameObject selectedItem;

    private List<string> selectedNodeAliasList;
    private Dictionary<string, EditorMenuNode> nodesMap;
    private Dictionary<string, MMenuItem> menuItemMap;

    private Animator menuRootAnimator;

    void Start()
    {
        selectedNodeAliasList = new List<string>();
        nodesMap = new Dictionary<string, EditorMenuNode>();
        menuItemMap = new Dictionary<string, MMenuItem>();
        menuRootAnimator = menuItemRoot.GetComponent<Animator>();

        CompileNodes();
        InitNodeObject();
    }

    private void Update()
    {
        
    }

    void CompileNodes()
    {
        var editorMenuTable = EditorMenuTable.Instance.GetAll();
        nodesMap.Add("Root", new EditorMenuNode("Root", null));

        foreach (var row in editorMenuTable)
        {
            EditorMenuNode node;

            if(row.Value.parent == "")
            {
                node = new EditorMenuNode(row.Key, nodesMap["Root"]);
                nodesMap["Root"].ChildNodes.Add(node);
            }
            else
            {
                node = new EditorMenuNode(row.Key, nodesMap[row.Value.parent]);
                nodesMap[row.Value.parent].ChildNodes.Add(node);
            }

            nodesMap.Add(row.Key, node);

        }

        selectedNodeAliasList.Add("Root");
    }

    void InitNodeObject()
    {

        /* Generate menuItem Row Scroll View */
        GameObject itemRow = Instantiate(menuItemRowPref);
        itemRow.transform.SetParent(menuItemRoot.transform);
        itemRow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000, 0);
        itemRow.transform.localScale = Vector3.one;

        /* Generate Root menuItem */
        for (int i = 0; i < nodesMap["Root"].ChildNodes.Count; ++i)
        {
            GameObject newObject = Instantiate(menuItemPref);

            /* Transform Settings */
            newObject.transform.SetParent(itemRow.transform.Find("Viewport").Find("Content"));
            newObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(250 * i - 500,0);
            newObject.transform.localScale = Vector3.one;
            newObject.name = "MenuItem_" + i;

            MMenuItem menuItem = newObject.GetComponent<MMenuItem>();
            EditorMenuTable.EditorMenuProperty menuProperty = EditorMenuTable.Instance.GetProperty(nodesMap["Root"].ChildNodes[i].Alias);
            menuItem.SetImage(menuProperty.imagePath);
            menuItem.Alias = menuProperty.alias;
            menuItemMap.Add(menuProperty.alias, menuItem);
        }

        /* Item의 시작 애니메이션 트리거 */
        //menuItemRoot.GetComponent<Animator>().SetTrigger("Start");

    }

    public void OpenMenu()
    {
        menuItemRoot.GetComponent<Animator>().SetBool("Open", true);
    }

    public void CloseMenu()
    {
        menuItemRoot.GetComponent<Animator>().SetBool("Open", false);
    }

    public IEnumerator SelectedItem(MMenuItem menuItem)
    {
        /* Wait 상태가 될 때 까지 대기. 연속으로 여러 입력됐을 경우를 대비. */
        for(; ; )
        {
            if (menuRootAnimator.GetCurrentAnimatorStateInfo(0).IsName("Wait"))
                break;
            yield return new WaitForFixedUpdate();
        }


        /* 선택한 아이템의 레벨과 마지막 선택한 아이템의 레벨이 같다면 마지막 선택 아이템 선택해제. */
        {
            List<MMenuItem> selectItLevel = GetSameLeveItems(menuItem.Alias);

            foreach (var menuIt in selectItLevel)
            {
                if (menuIt == GetLastSelecteItem())
                {
                    DeselectItem();
                    break;
                }
            }
        }

        /* Item에 자식 노드가 없다면 */
        if (nodesMap[menuItem.Alias].ChildNodes.Count <= 0)
        {
            selectedNodeAliasList.Add(menuItem.Alias);
            menuItem.GetComponent<Animator>().SetTrigger("Select");
            selectedItem.transform.Find("ItemImage").GetComponent<Image>().sprite = menuItem.imageObject.sprite;
            yield break;
        }

        selectedNodeAliasList.Add(menuItem.Alias);
        GenerateChildNode(menuItem.Alias);

        menuItemRoot.GetComponent<Animator>().SetTrigger("Select");
        menuItem.GetComponent<Animator>().SetTrigger("Select");


        /* 같은 레벨의 MenuItem 오브젝트들을 찾고 버튼을 비활성화. */
        if (selectedNodeAliasList.Count > 0)
        {
            /* 로직 : 노드의 부모를 찾아 그 자식들을 찾는다. */
            List<MMenuItem> rowMenuItems = GetSameLeveItems(GetLastSelectedNode().Alias);
            foreach (var rowMenuItem in rowMenuItems)
            {
                rowMenuItem.GetComponent<Button>().enabled = false;
            }
        }

    }

    public void CanccelItem()
    {

        if (selectedNodeAliasList.Count <= 1)
            return;

        if (GetLastSelectedNode().ChildNodes.Count > 0)
        {
            menuItemRoot.GetComponent<Animator>().SetTrigger("Cancel");

            /* Row Object 삭제 & 버튼 다시 활성화 */
            GameObject CancelRow = GetLastRowObject();
            RemoveRowObject(CancelRow);
            foreach (var Item in GetSameLeveItems(GetLastSelectedNode().Alias))
            {
                Item.GetComponent<Button>().enabled = true;
            }
        }

        GetLastSelecteItem().GetComponent<Animator>().SetTrigger("Deselect");
        selectedNodeAliasList.RemoveAt(selectedNodeAliasList.Count - 1);
    }

    private void DeselectItem()
    {
        GetLastSelecteItem().GetComponent<Animator>().SetTrigger("Deselect");
        selectedNodeAliasList.RemoveAt(selectedNodeAliasList.Count - 1);
    }

    private void GenerateChildNode(string alias)
    {
        /* Generate menuItem Row Scroll View */
        GameObject itemRow = Instantiate(menuItemRowPref);
        itemRow.transform.SetParent(menuItemRoot.transform);
        itemRow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -400);
        itemRow.transform.localScale = Vector3.one;

        /* Generate Root menuItem */
        for (int i = 0; i < nodesMap[alias].ChildNodes.Count; ++i)
        {
            GameObject newObject = Instantiate(menuItemPref);

            /* Transform Settings */
            newObject.transform.SetParent(itemRow.transform.Find("Viewport").Find("Content"));
            newObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(250 * i - 500, 0);
            newObject.transform.localScale = Vector3.one;
            newObject.name = "MenuItem_" + i;

            MMenuItem menuItem = newObject.GetComponent<MMenuItem>();
            EditorMenuTable.EditorMenuProperty menuProperty = EditorMenuTable.Instance.GetProperty(nodesMap[alias].ChildNodes[i].Alias);

            if (menuItemMap.ContainsKey(menuProperty.alias))
                menuItemMap.Remove(menuProperty.alias);

            menuItem.SetImage(menuProperty.imagePath);
            menuItem.Alias = menuProperty.alias;
            menuItemMap.Add(menuProperty.alias, menuItem);
        }

        /* Item의 시작 애니메이션 트리거 */
        menuItemRoot.GetComponent<Animator>().SetTrigger("Select");
    }

    private EditorMenuNode GetLastSelectedNode()
    {
        return nodesMap[selectedNodeAliasList[selectedNodeAliasList.Count - 1]];
    }

    private MMenuItem GetLastSelecteItem()
    {
        /* Root 는 Item이 없기 때문에. */
        if (selectedNodeAliasList[selectedNodeAliasList.Count - 1] != "Root")
        {
            return menuItemMap[selectedNodeAliasList[selectedNodeAliasList.Count - 1]];
        }

        return null;
    }

    /* 해당 Alias와 같은 레벨의 아이템들을 모두 가져옵니다. */
    private List<MMenuItem> GetSameLeveItems(string alias)
    {
        List<MMenuItem> result = new List<MMenuItem>();

        foreach (var node in nodesMap[alias].ParentNode.ChildNodes)
        {
            if(menuItemMap.ContainsKey(node.Alias))
                result.Add(menuItemMap[node.Alias]);
        }

        return result;
    }

    /* 가장 아래있는 Row Object를 가져옵니다. */
    private GameObject GetLastRowObject()
    {
        EditorMenuNode lastSelectedNode = GetLastSelectedNode();

        /* 마지막 선택한게 자식 노드가 없다면, */
        if(lastSelectedNode.ChildNodes.Count <= 0)
        {
            return menuItemMap[lastSelectedNode.Alias].transform.parent.parent.parent.gameObject;
        }
        else
        {
            return menuItemMap[lastSelectedNode.ChildNodes[0].Alias].transform.parent.parent.parent.gameObject;
        }
        
    }

    /* 해당 Row를 삭제합니다 */
    private void RemoveRowObject(GameObject RowObject)
    {
        RowObject.GetComponent<Animator>().SetTrigger("Remove");

       MMenuItem[] ChildItems = RowObject.GetComponentsInChildren<MMenuItem>();
        foreach (var Item in ChildItems)
        {
            menuItemMap.Remove(Item.Alias);
        }
    }
}
