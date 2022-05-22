using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utill
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        // Generic이기 때문에 UnityEngine.Component type일때만 이라는 조건 추가
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if(transform == null)
            return null;
        
        return transform.gameObject;
    }

    // recursive: 재귀적으로 바로 아래 자식까지만 탐색할지 더 아래의 자식까지 탐색할지 결정
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            // 바로 직속 자식의 수만큼 반복
            for(int i = 0; i < go.transform.childCount; i++)
            {
                // Transform을 통해 자식 Object에 접근
                Transform transform = go.transform.GetChild(i);

                // enum
                if(string.IsNullOrEmpty(transform.name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if(component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach(T component in go.GetComponentsInChildren<T>())
            {
                // enum 이름이 주어지지 않은 경우 그냥 모든 자식들을 return
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
