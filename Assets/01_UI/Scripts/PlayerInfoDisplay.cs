using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfoDisplay : MonoBehaviour
{
    [SerializeField] protected Transform Container;
    [SerializeField] protected GameObject Player;

    [SerializeField] protected GameObject EmptyHeartPrefab;
    [SerializeField] protected GameObject HeartPrefab;
    [SerializeField] protected GameObject ArmorPrefab;
    [SerializeField] protected GameObject BlankPrefab;
    [SerializeField] protected GameObject KeyPrefab;

    private Player m_Player;
    private List<GameObject> EmptyHeartList = new List<GameObject>();
    private List<GameObject> HeartList = new List<GameObject>();
    private List<GameObject> ArmorList = new List<GameObject>();
    private List<GameObject> BlankList = new List<GameObject>();
    private List<GameObject> KeyList = new List<GameObject>();

    private float ArmorPos;

    void Awake()
    {
        m_Player = Player.GetComponent<Player>();
    }

    private void Start()
    {
        for (int i = 0; i < m_Player.Armor; i++)
            UpdateArmor();
        for (int i = 0; i < m_Player.MaxHP; i++)
            UpdateMaxHP();
        for (int i = 0; i < m_Player.CurrentHP; i++)
            UpdateCurrentHP();
        for (int i = 0; i < m_Player.Blank; i++)
            UpdateBlank();
        for (int i = 0; i < m_Player.Key; i++)
            UpdateKey();
    }

    void Update()
    {
        UpdateMaxHP();
        UpdateCurrentHP();
        UpdateArmor();
        UpdateBlank();
        UpdateKey();
    }
    void UpdateMaxHP()
    {
        if (EmptyHeartList.Count < m_Player.MaxHP)
        {
            GameObject heart = Instantiate(EmptyHeartPrefab, Container);
            RectTransform rect = heart.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(EmptyHeartList.Count * rect.sizeDelta.x, 0);
            ArmorPosChanged(rect.sizeDelta.x);
            EmptyHeartList.Add(heart);
        }
        else if (EmptyHeartList.Count > m_Player.MaxHP)
        {
            if (EmptyHeartList.Count == 0)
                return;
            GameObject lastHeart = EmptyHeartList[EmptyHeartList.Count - 1];
            RectTransform rect = lastHeart.GetComponent<RectTransform>();
            ArmorPosChanged(-rect.sizeDelta.x);
            EmptyHeartList.RemoveAt(EmptyHeartList.Count - 1);
            Destroy(lastHeart);
        }
    }

    void UpdateCurrentHP()
    {
        if (HeartList.Count < m_Player.CurrentHP)
        {
            GameObject heart = Instantiate(HeartPrefab, Container);
            RectTransform rect = heart.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(HeartList.Count * rect.sizeDelta.x, 0);
            HeartList.Add(heart);
        }
        else if (HeartList.Count > m_Player.CurrentHP)
        {
            if (HeartList.Count == 0)
                return;
            GameObject lastHeart = HeartList[HeartList.Count - 1];
            HeartList.RemoveAt(HeartList.Count - 1);
            Destroy(lastHeart);
        }
    }

    void UpdateArmor()
    {
        if (ArmorList.Count < m_Player.Armor)
        {
            GameObject armor = Instantiate(ArmorPrefab, Container);
            RectTransform rect = armor.GetComponent<RectTransform>();
            //rect.anchoredPosition = new Vector2((ArmorList.Count + m_Player.MaxHP) * rect.sizeDelta.x, 0);
            rect.anchoredPosition = new Vector2(ArmorPos + ArmorList.Count* rect.sizeDelta.x, 0);
            ArmorList.Add(armor);
        }
        else if (ArmorList.Count > m_Player.Armor)
        {
            if (ArmorList.Count == 0)
                return;
            GameObject lastArmor = ArmorList[ArmorList.Count - 1];
            ArmorList.RemoveAt(ArmorList.Count - 1);
            Destroy(lastArmor);
        }
    }

    void ArmorPosChanged(float p_pos)
    {
        ArmorPos += p_pos;
        foreach (var armor in ArmorList)
        {
            RectTransform rect = armor.GetComponent<RectTransform>();
            Vector2 pos = rect.anchoredPosition;
            pos.x += p_pos;
            rect.anchoredPosition = pos;
        }
    }

    void UpdateBlank()
    {
        if (BlankList.Count < m_Player.Blank)
        {
            GameObject blank = Instantiate(BlankPrefab, Container);
            RectTransform rect = blank.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(BlankList.Count * rect.sizeDelta.x, -rect.sizeDelta.y);
            BlankList.Add(blank);
        }
        else if (BlankList.Count > m_Player.Blank)
        {
            if (BlankList.Count == 0)
                return;
            GameObject lastBlank = BlankList[BlankList.Count - 1];
            BlankList.RemoveAt(BlankList.Count - 1);
            Destroy(lastBlank);
        }
    }

    void UpdateKey()
    {
        if (KeyList.Count < m_Player.Key)
        {
            GameObject key = Instantiate(KeyPrefab, Container);
            RectTransform rect = key.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(KeyList.Count * rect.sizeDelta.x, -rect.sizeDelta.y * 2);
            KeyList.Add(key);
        }
        else if (KeyList.Count > m_Player.Key)
        {
            if (KeyList.Count == 0)
                return;
            GameObject lastKey = KeyList[KeyList.Count - 1];
            KeyList.RemoveAt(KeyList.Count - 1);
            Destroy(lastKey);
        }
    }
}
