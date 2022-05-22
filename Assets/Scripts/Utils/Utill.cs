using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utill
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        // Generic�̱� ������ UnityEngine.Component type�϶��� �̶�� ���� �߰�
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

    // recursive: ��������� �ٷ� �Ʒ� �ڽı����� Ž������ �� �Ʒ��� �ڽı��� Ž������ ����
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            // �ٷ� ���� �ڽ��� ����ŭ �ݺ�
            for(int i = 0; i < go.transform.childCount; i++)
            {
                // Transform�� ���� �ڽ� Object�� ����
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
                // enum �̸��� �־����� ���� ��� �׳� ��� �ڽĵ��� return
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
