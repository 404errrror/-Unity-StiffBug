/*
 *  2019.11.10
 *  싱글턴 클래스입니다.
 *  상속받아서 싱글턴 클래스로 만들 수 있습니다.
 */

using System.Collections;
using System.Collections.Generic;

public class Singleton<T> where T : class
{
    protected static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = System.Activator.CreateInstance(typeof(T)) as T;

            return instance;
        }
    }
}
