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

public class TheaterTable : DataTableInterface<TheaterTable, TheaterTable.TheaterProperty>
{
    public struct TheaterProperty
    {
        public string alias;
        public string nextAlias;
        public string stringData;
        public string textBoxType;
        public string ownerObject;
        public string focusObject;
        public ETheaterType theaterType;
    }
    override protected void Init()
    {
        Load("Local/DataTable/TheaterTable");
    }

    protected override TheaterProperty RowToProperty(Dictionary<string, string> row)
    {
        TheaterProperty result = new TheaterProperty();
        foreach (var item in row)
        {
            switch (item.Key)
            {
                case "KeyAlias": result.alias = item.Value              as string; break;
                case "NextTheaterAlias": result.nextAlias = item.Value  as string; break;
                case "StringData": result.stringData = item.Value       as string; break;
                case "TextBoxType": result.textBoxType = item.Value     as string; break;
                case "OargetObject": result.ownerObject = item.Value    as string; break;
                case "FocusObject": result.focusObject = item.Value     as string; break;
                case "TheaterType": result.theaterType = (ETheaterType)System.Enum.Parse(typeof(ETheaterType), item.Value); break;

                default:
                    Debug.LogWarning("DataTableClasses.cs, TheaterTable, <" + item.Key + ">의 데이터 변환이 실패하였습니다!!");
                    break;
            }
        }
        return result;
    }
}
