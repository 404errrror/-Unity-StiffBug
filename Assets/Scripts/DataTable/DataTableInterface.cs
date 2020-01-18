using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataTableInterface<T, PropertyType> : Singleton<T>  where T : class where PropertyType : struct
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
    /// <summary>
    /// Alias를 제외하고, Key에 해당되는 Property를 정의해주세요!
    /// </summary>
    protected abstract PropertyType RowToProperty(Dictionary<string, string> row);

    public PropertyType GetProperty(string keyAlias)
    {
        if(isInit == false)
        {
            Debug.LogError("DataTableInterface.cs GetProperty(), 데이터 테이블이 초기화 되지 않았습니다!!");
            return new PropertyType();
        }

        if(dataTable.ContainsKey(keyAlias) == false)
        {
            Debug.LogError("DataTableInterface.cs, <" + keyAlias + "> 에 해당되는 Key가 존재하지 않습니다!!");
            return new PropertyType();
        }

        return dataTable[keyAlias];
    }

    /// <summary>
    /// 데이터 테이블의 모든 요소를 가져옵니다. 복사 후 리턴하기 때문에 성능이 느립니다!!
    /// </summary>
    public Dictionary<string, PropertyType> GetAll()
    {
        if (isInit == false)
        {
            Debug.LogError("DataTableInterface.cs GetAll(),  데이터 테이블이 초기화 되지 않았습니다!!");
            return new Dictionary<string, PropertyType>();
        }
        return new Dictionary<string, PropertyType>(dataTable);
    }

    protected void Load(string path, string keyAlias = "KeyAlias")
    {
        List<Dictionary<string, string>> csvDataTable = CSVReader.Read(path);

        if (csvDataTable.Count <= 0 || csvDataTable[0].ContainsKey(keyAlias) == false)
        {
            Debug.LogError("DataTableInterface.cs, <" + keyAlias + "> 에 해당되는 Key가 존재하지 않습니다!!");
            return;
        }

        foreach (var row in csvDataTable)
        {
            string _alias = row[keyAlias] as string;
            dataTable.Add(_alias, RowToProperty(row));
        }
    }

}