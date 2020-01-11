using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMenuTable : DataTableInterface<EditorMenuTable, EditorMenuTable.EditorMenuProperty>
{
    public struct EditorMenuProperty
    {
        public string alias;
        public string name;
        public string imageName;
        public string parent;
        public int blockID;
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
                case "Name":        result.name = item.Value        as string;          break;
                case "ImageName":   result.imageName = item.Value   as string;          break;
                case "Parent":      result.parent = item.Value      as string;          break;
                case "BlockID":     int.TryParse(item.Value, out result.blockID);       break;

                default:
                    Debug.LogError("DataTableClasses.cs, EditorMenuTable, <" + item.Key + ">의 데이터 변환이 실패하였습니다!!");
                    break;
            }
        }
        return result;
    }
}
