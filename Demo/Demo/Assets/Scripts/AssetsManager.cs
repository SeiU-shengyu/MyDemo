using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetsManager : MonoBehaviour {
    private Dictionary<int, Sprite> m_icons = new Dictionary<int, Sprite>();
    private Dictionary<ActorType, ActorPool> m_actors = new Dictionary<ActorType, ActorPool>();

    void Awake()
    {
        Texture2D[] textures = Resources.LoadAll<Texture2D>("ItemImage");
        for (int i = 0; i < textures.Length; i++)
        {
            Sprite sprite = Sprite.Create(textures[i], new Rect(0, 0, textures[i].width, textures[i].height), new Vector2(0.5f, 0.5f));
            m_icons.Add(int.Parse(textures[i].name), sprite);
        }

        GameObject gameObject = Resources.Load<GameObject>("Item");
        m_actors.Add(ActorType.ITEM, new ActorPool(10, gameObject));
    }

    // Use this for initialization
    void Start() {

    }

    private Actor testActor;
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            testActor = m_actors[ActorType.ITEM].GetActor();
        if(Input.GetMouseButtonDown(0))
            m_actors[ActorType.ITEM].ReleaseActor(testActor);
    }

    public Sprite GetIconSprite(int id)
    {
        return m_icons[id];
    }

    public Actor GetActor(ActorType actorType)
    {
        return m_actors[actorType].GetActor();
    }
}

public class ActorPool
{
    private GameObject m_templateObj;
    private List<Actor> m_idlePool;
    private List<Actor> m_busyPool;
    private Transform m_idleParent;
    private Transform m_busyParent;

    public ActorPool(int initCounts,GameObject obj)
    {
        GameObject gameObject1 = new GameObject(obj.name);
        gameObject1.transform.parent = GameObject.Find("Canvas/UIActorPool").transform;
        m_idleParent = new GameObject(obj.name + "IdlePool").transform;
        m_busyParent = new GameObject(obj.name + "BusyPool").transform;
        m_idleParent.SetParent(gameObject1.transform);
        m_busyParent.SetParent(gameObject1.transform);

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
            actor.transform.SetParent(m_busyParent);
            return actor;
        }
        else
        {
            Actor actor = Object.Instantiate(m_templateObj).GetComponent<Actor>();
            m_busyPool.Add(actor);
            actor.ReUse();
            actor.transform.SetParent(m_busyParent);
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
