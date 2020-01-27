using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MEditorMenuManager : MonoSingleton<MEditorMenuManager>
{
    public GameObject parentCanvas;
    public GameObject menuItemRoot;
    public GameObject menuItemPref;
    public GameObject menuItemRowPref;

    private List<string> selectedNodeAliasList;
    private Dictionary<string, EditorMenuNode> nodesMap;
    private Dictionary<string, MMenuItem> menuItemMap;

    void Start()
    {
        selectedNodeAliasList = new List<string>();
        nodesMap = new Dictionary<string, EditorMenuNode>();
        menuItemMap = new Dictionary<string, MMenuItem>();

        CompileNodes();
        InitNodeObject();
    }

    void CompileNodes()
    {
        var editorMenuTable = EditorMenuTable.Instance.GetAll();
        nodesMap.Add("Root", new EditorMenuNode("Root"));

        foreach (var row in editorMenuTable)
        {
            EditorMenuNode node = new EditorMenuNode(row.Key);
            nodesMap.Add(row.Key, node);

            if(row.Value.parent == "")
            {
                nodesMap["Root"].ChildNodes.Add(node);
            }
            else
            {
                nodesMap[row.Value.parent].ChildNodes.Add(node);
            }
        }

        selectedNodeAliasList.Add("Root");
    }

    void InitNodeObject()
    {

        /* Generate menuItem Row Scroll View */
        GameObject itemRow = Instantiate(menuItemRowPref);
        itemRow.transform.SetParent(menuItemRoot.transform);
        itemRow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -600);
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
        menuItemRoot.GetComponent<Animator>().SetTrigger("Start");

    }

    public void SelectedItem(MMenuItem menuItem)
    {
        menuItemRoot.GetComponent<Animator>().SetTrigger("Start");

        /* 같은 레벨의 MenuItem 오브젝트들을 찾고 버튼을 비활성화. */
        if (selectedNodeAliasList.Count > 0)
        {
            /* 로직 : 노드의 부모를 찾아 그 자식들을 찾는다. */
            List<EditorMenuNode> rowMenuItemAlias = nodesMap[selectedNodeAliasList[selectedNodeAliasList.Count - 1]].ChildNodes;
            foreach (var MenuItemAlias in rowMenuItemAlias)
            {
                if (menuItemMap.ContainsKey(MenuItemAlias.Alias) == true)
                {
                    menuItemMap[MenuItemAlias.Alias].GetComponent<Button>().enabled = false;
                }
            }
        }

        selectedNodeAliasList.Add(menuItem.Alias);
    }
}
