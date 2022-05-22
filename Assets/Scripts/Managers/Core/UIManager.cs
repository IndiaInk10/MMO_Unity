using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Managers 에서 연결해서 사용
public class UIManager
{
    // sort 안하는 canvas와 차별화된 값을 주기 위함
    int _order  = 10;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    // SceneUI를 잠깐 물려주는 변수
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

    // 코드적으로 Popup을 수행한 경우(Unity 내부에서가 아니라 외부에서 Popup이 된 경우)
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utill.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // Canvas의 중첩의 경우 부모 Canvas의 영향을 받지 않음
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
            // type의 이름 추출
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
            // type의 이름 추출
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
            // type의 이름 추출
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        T sceneUI = Utill.GetOrAddComponent<T>(go);
        // SceneUI 저장
        _sceneUI = sceneUI;        

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }
    // name = Prefab name
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
        {
            // type의 이름 추출
            name = typeof(T).Name; 
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

        T popup = Utill.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    // UI_Popup Base인 녀석을 받아와서 Popup 대상이 맞는지 확인
    // 순서가 꼬인 경우를 예외처리 한다는 뜻(?)
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
