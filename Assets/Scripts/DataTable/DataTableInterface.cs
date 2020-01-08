using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataTableInterface<T, PropertyType> : Singleton<T>  where T : class
{
    private bool isInit;
    private Dictionary<string, PropertyType> dataTable;

    public void _Init()
    {
        isInit = true;
        dataTable = new Dictionary<string, PropertyType>();

        Init();
    }

    protected abstract void Init();
    protected abstract PropertyType RowToProperty(Dictionary<string, string> row);

    public PropertyType GetProperty(string alias)
    {
        if(isInit == false)
        {
            Debug.LogError("Please Initialize!!");
        }

        if(dataTable.ContainsKey(alias) == false)
        {
            Debug.LogError("DataTableInterface.cs, " + alias + "에 해당되는 Key가 존재하지 않습니다!!");
        }

        return dataTable[alias];
    }

    protected void Load(string alias, string path)
    {
        List<Dictionary<string, string>> temp = CSVReader.Read(path);

        foreach (var row in temp)
        {
            string _alias = row[alias] as string;
            row.Remove(alias);

            dataTable.Add(_alias, RowToProperty(row));
        }
    }

}