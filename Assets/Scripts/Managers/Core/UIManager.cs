using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Managers ���� �����ؼ� ���
public class UIManager
{
    // sort ���ϴ� canvas�� ����ȭ�� ���� �ֱ� ����
    int _order  = 10;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    // SceneUI�� ��� �����ִ� ����
    UI_Scene _sceneUI;

    public GameObject Root
    { 
        get 
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }

    // �ڵ������� Popup�� ������ ���(Unity ���ο����� �ƴ϶� �ܺο��� Popup�� �� ���)
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utill.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // Canvas�� ��ø�� ��� �θ� Canvas�� ������ ���� ����
        canvas.overrideSorting = true;

        if(sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }
    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
        {
            // type�� �̸� ����
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return go.GetOrAddComponent<T>();
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
        {
            // type�� �̸� ����
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return go.GetOrAddComponent<T>();
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
        {
            // type�� �̸� ����
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        T sceneUI = Utill.GetOrAddComponent<T>(go);
        // SceneUI ����
        _sceneUI = sceneUI;        

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }
    // name = Prefab name
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
        {
            // type�� �̸� ����
            name = typeof(T).Name; 
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

        T popup = Utill.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    // UI_Popup Base�� �༮�� �޾ƿͼ� Popup ����� �´��� Ȯ��
    // ������ ���� ��츦 ����ó�� �Ѵٴ� ��(?)
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if(_popupStack.Peek() != popup)
        {
            Debug.Log("Close PopupFailed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        //UI_Popup popup = _popupStack.Pop();
        //Managers.Resource.Destroy(popup.gameObject);
        //popup = null;
        Managers.Resource.Destroy(_popupStack.Pop().gameObject);

        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
