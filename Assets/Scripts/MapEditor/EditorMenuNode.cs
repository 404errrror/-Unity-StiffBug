using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMenuNode
{
    public string Alias { get; private set; }
    public List<EditorMenuNode> ChildNodes { get; private set; }
    public EditorMenuNode ParentNode { get; private set; }

    public EditorMenuNode(string alias, EditorMenuNode parentNode, List<EditorMenuNode> childNodes = null)
    {
        Alias = alias;
        ParentNode = parentNode;
        ChildNodes = childNodes;
        
        if(ChildNodes == null)
            ChildNodes = new List<EditorMenuNode>();
    }
}
