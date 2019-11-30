/*
 *  2019.11.10
 *  MonoBehaviour를 상속받는 싱글턴 클래스입니다.
 *  상속받아서 싱글턴 클래스로 만들 수 있습니다.
 *  아직 테스트 X
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                /* instance 가 존재하지 않으면 찾는다. */
                instance = FindObjectOfType<T>() as T;

                /* 찾지 못했다면 생성한다. */
                if (instance == null)
                {
                    /* 새로운 오브젝트를 생성 후 컴포넌트 T 를 붙임. */
                    instance = new GameObject("@" + typeof(T).ToString(), typeof(T)).AddComponent<T>();
                    DontDestroyOnLoad(instance);
                }
            }
            return instance;
        }
    }

}
