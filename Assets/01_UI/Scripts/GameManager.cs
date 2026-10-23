using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerObj;
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private Player m_Player;
    [SerializeField] private PlayerControl m_PlayerControl;
    //[SerializeField] private RoomManager m_RoomManager;
    //[SerializeField] private EnemyManager m_EnemyManager;
    //[SerializeField] private ItemManager m_ItemManager;
    [SerializeField] protected Transform m_BulletManager;
    [SerializeField] protected ScreenBlinder m_ScreenBlinder;

    private float BossIntroDuration = 3f;
    public int CurrentFloor;

    public bool IsGameOver;
    private void Awake()
    {
        m_PlayerControl = m_PlayerObj.GetComponent<PlayerControl>();
        m_Player = m_PlayerObj.GetComponent<Player>();
        CurrentFloor = 1;
        IsGameOver = false;
    }

    private void Update()
    {
        UpdateCheckPlayerDied();
    }

    void UpdateCheckPlayerDied()
    {
        if (!IsGameOver && m_Player.IsDead)
        {
            IsGameOver = true;
            StartCoroutine(GameOver());
        }
    }

    public IEnumerator GameOver()
    {
        m_PlayerControl.CanControl = true;
        m_ScreenBlinder.StartBlind();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Title");
    }

    public void StartGoNextFloor()
    {
        StartCoroutine(GoNextFloor());
    }

    IEnumerator GoNextFloor()
    {
        m_PlayerControl.CanControl = false;
        m_ScreenBlinder.StartBlind();
        
        yield return new WaitForSeconds(1.5f);

        CurrentFloor++;
        //[SerializeField] private RoomManager m_RoomManager;
        //m_RoomManager.ResetRooms();
        //foreach (Transform enemy in m_EnemyManager.transform)
        //    Destroy(enemy.gameObject);
        //foreach (Transform item in m_ItemManager.transform)
        //    Destroy(item.gameObject);
        //foreach (Transform bullet in m_BulletManager.transform)
        //    Destroy(bullet.gameObject);

        Vector2 playerPos = new Vector2(12f, 6f);
        m_PlayerObj.transform.position = playerPos;
        m_MainCamera.transform.position = playerPos;

        yield return new WaitForSeconds(1.5f);

        m_ScreenBlinder.StartShow();

        yield return new WaitForSeconds(1f);

        m_PlayerControl.CanControl = true;


    }

    public IEnumerator CameraTargetBoss(GameObject p_boss)
    {
        m_MainCamera.GetComponent<CameraMove>().Target = p_boss.transform;
        m_PlayerControl.CanControl = false;
        yield return new WaitForSeconds(BossIntroDuration);
        m_MainCamera.GetComponent<CameraMove>().Target = m_PlayerObj.transform;
        m_PlayerControl.CanControl = true;
    }
}
