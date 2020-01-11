using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMenuNode
{
    string alias;
    List<EditorMenuNode> childNode;
    EditorMenuNode(string alias, List<EditorMenuNode> childNode = null)
    {
        this.alias = alias;
        this.childNode = childNode;
    }
}
