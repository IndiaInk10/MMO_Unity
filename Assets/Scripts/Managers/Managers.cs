using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // ���ϼ��� ����ȴ�
    // Managers script�� �޸� ���� ������Ʈ�� ��� 
    // GetInstance�� ���� @Managers ��� ���� ������Ʈ�� �߰��� �� �ִ�
    //public static Managers GetInstance() { Init();  return Instance; } // ������ Manager�� �����´�
    static Managers Instance { get { Init(); return s_instance; } } // ������ Manager�� �����´�

    #region Contents
    GameManager _game = new GameManager();
    public static GameManager Game { get { return s_instance._game; } }
    #endregion

    #region Core
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();
    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        _input.OnUpdate();
    }

    // Init �ȿ��� Instance ���� ���ѷ���
    static void Init()
    {
        if(s_instance == null)
        {
            // �ʱ�ȭ
            // ���� �ڿ��� �Ҹ��ϴ� Component�� �̸����� ã�� ���
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            // ������� �����ϰų� �߰��ϴ� ���� ������� �ʴ´�
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._data.Init();
            s_instance._pool.Init();
            s_instance._sound.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        Scene.Clear();
        UI.Clear();

        // pooling�� Object�� ����� ���� ������ �������� Clear
        Pool.Clear();
    }
}