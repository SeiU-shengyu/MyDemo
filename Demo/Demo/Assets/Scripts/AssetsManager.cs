using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetsManager : MonoBehaviour {
    private Dictionary<int, Sprite> m_icons = new Dictionary<int, Sprite>();
    private Dictionary<ActorType, ActorPool> m_actors = new Dictionary<ActorType, ActorPool>();
    private List<Actor> aliveActor = new List<Actor>();
    public List<Actor> AliveActor { get { return aliveActor; } set { aliveActor = value; } }

    public static AssetsManager Instance;

    void Awake()
    {
        Instance = this;
        Texture2D[] textures = Resources.LoadAll<Texture2D>("ItemImage");
        for (int i = 0; i < textures.Length; i++)
        {
            Sprite sprite = Sprite.Create(textures[i], new Rect(0, 0, textures[i].width, textures[i].height), new Vector2(0.5f, 0.5f));
            m_icons.Add(int.Parse(textures[i].name), sprite);
        }

        //加载资源并创建对应的对象池
        GameObject gameObject = Resources.Load<GameObject>("Item");
        m_actors.Add(ActorType.ITEM, new ActorPool(10, gameObject));
        gameObject = Resources.Load<GameObject>("BufferObj");
        m_actors.Add(ActorType.AACTORBUFFER, new ActorPool(10, gameObject));
        gameObject = Resources.Load<GameObject>("SkillItem");
        m_actors.Add(ActorType.SKILL, new ActorPool(10, gameObject));
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
    }

    /// <summary>
    /// 获取加载过的Icon图片资源
    /// </summary>
    /// <param name="id">icon对应的id</param>
    /// <returns></returns>
    public Sprite GetIconSprite(int id)
    {
        return m_icons[id];
    }

    /// <summary>
    /// 从对应的Actor对象池中获取加载过的Actor
    /// </summary>
    /// <param name="actorType">Actor对应的类型</param>
    /// <returns></returns>
    public Actor GetActor(ActorType actorType)
    {
        return m_actors[actorType].GetActor();
    }

    /// <summary>
    /// 释放被引用的Actor到对应的对象池
    /// </summary>
    /// <param name="actorType"></param>
    /// <param name="actor"></param>
    public void ReleaseActor(ActorType actorType, Actor actor)
    {
        m_actors[actorType].ReleaseActor(actor);
    }

    public void AddAliveActor(Actor actor)
    {
        aliveActor.Add(actor);
    }
    public void RemoveAliveActor(Actor actor)
    {
        aliveActor.Remove(actor);
    }
}

public class ActorPool
{
    private GameObject m_templateObj;
    private List<Actor> m_idlePool;
    private List<Actor> m_busyPool;
    private Transform m_idleParent;

    public ActorPool(int initCounts,GameObject obj)
    {
        GameObject gameObject = new GameObject(obj.name);
        gameObject.transform.parent = GameObject.Find("ActorPool").transform;
        m_idleParent = gameObject.transform;

        m_templateObj = obj;
        m_idlePool = new List<Actor>(initCounts);
        m_busyPool = new List<Actor>(initCounts);
        for (int i = 0; i < initCounts; i++)
        {
            Actor actor = Object.Instantiate(obj).GetComponent<Actor>();
            actor.UnUse();
            actor.transform.SetParent(m_idleParent);
            m_idlePool.Add(actor);
        }
    }

    public Actor GetActor()
    {
        if (m_idlePool.Count > 0)
        {
            Actor actor = m_idlePool[m_idlePool.Count - 1];
            m_idlePool.Remove(actor);
            m_busyPool.Add(actor);
            actor.ReUse();
            return actor;
        }
        else
        {
            Actor actor = Object.Instantiate(m_templateObj).GetComponent<Actor>();
            m_busyPool.Add(actor);
            actor.ReUse();
            return actor;
        }
    }

    public void ReleaseActor(Actor actor)
    {
        m_busyPool.Remove(actor);
        m_idlePool.Add(actor);
        actor.UnUse();
        actor.transform.SetParent(m_idleParent);
    }
}
