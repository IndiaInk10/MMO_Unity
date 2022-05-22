using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다
    // Managers script가 달린 게임 오브젝트가 없어도 
    // GetInstance를 통해 @Managers 라는 게임 오브젝트를 추가할 수 있다
    //public static Managers GetInstance() { Init();  return Instance; } // 유일한 Manager를 가져온다
    static Managers Instance { get { Init(); return s_instance; } } // 유일한 Manager를 가져온다

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

    // Init 안에서 Instance 사용시 무한루프
    static void Init()
    {
        if(s_instance == null)
        {
            // 초기화
            // 많은 자원을 소모하는 Component를 이름으로 찾는 방법
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            // 마음대로 삭제하거나 추가하는 것을 허용하지 않는다
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

        // pooling한 Object를 사용할 수도 있으니 마지막에 Clear
        Pool.Clear();
    }
}
