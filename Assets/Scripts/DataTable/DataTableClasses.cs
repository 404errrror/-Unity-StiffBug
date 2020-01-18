using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMenuTable : DataTableInterface<EditorMenuTable, EditorMenuTable.EditorMenuProperty>
{
    public struct EditorMenuProperty
    {
        public string alias;
        public string name;
        public string imagePath;
        public string textImagePath;
        public string parent;
        public string customData;
    }
    override protected void Init()
    {
        Load("Local/DataTable/EditorMenuTable");
    }

    protected override EditorMenuProperty RowToProperty(Dictionary<string, string> row)
    {
        EditorMenuProperty result = new EditorMenuProperty();
        foreach (var item in row)
        {
            switch (item.Key)
            {
                case "KeyAlias":        result.alias = item.Value           as string;          break;
                case "Name":            result.name = item.Value            as string;          break;
                case "ImagePath":       result.imagePath = item.Value       as string;          break;
                case "TextImagePath":   result.textImagePath = item.Value   as string;          break;
                case "Parent":          result.parent = item.Value          as string;          break;
                case "CustomData":      result.customData = item.Value      as string;          break;

                default:
                    Debug.LogWarning("DataTableClasses.cs, EditorMenuTable, <" + item.Key + ">의 데이터 변환이 실패하였습니다!!");
                    break;
            }
        }
        return result;
    }
}
