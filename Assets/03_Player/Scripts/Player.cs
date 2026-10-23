using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    protected Animator m_Animator;
    protected SpriteRenderer m_SpriteRenderer;
    protected PlayerControl m_PlayerControl;

    [SerializeField] protected Transform m_BulletManager;

    [SerializeField] protected GameObject PlayerBulletPrefab;
    [SerializeField] protected GameObject BlankEffectPrefab;
    
    public Transform GunPos;

    [Header("Gun")] // 총 관련
    public float Damage = 5f; // 데미지
    public float DamageScaling = 1f; // 데미지 배수
    public float BossDamageScaling = 1f; //보스 데미지 배수
    public float Velocity = 5f; // 탄속(총알 속도)
    public float Round = 0.3f; // 다시 발사 할 수 있는 쿨타임
    public float Accuracy = 0.2f; // 정확도 : 낮을수록 마우스 포인터에서 빗나가게 총알이 나감
    public float BulletSizeScaling = 1f;

    private float RoundTimer = 0f;

    [Header("State")] // 플레이어 스탯, 상태 관련
    public int MaxHP = 6; // 최대 체력
    public int CurrentHP = 6; // 현재 체력
    public float MoveSpeed = 5f; // 이동 속도
    public bool IsRolling;
    public bool IsInvincible;
    public bool IsDead;

    [Header("Consumable")] // 소모 아이템 관련
    public int Armor = 0; // 갑옷 : 자동으로 체력 대신 소모
    public int Blank = 0; // 공포탄 : Q 입력 시 모든 적 탄환 삭제
    private float BlankEffectScale = 1f;
    public int Key = 0; // 열쇠 : 상자 상호작용 시 소모

    // 피격 시 무적 관련
    protected bool IsTakenDamage = false;
    protected float DamagedInvincibleDuration = 1f;
    protected float RollInvincibleDuration = 0.8f;

    public Coroutine RollCoroutine = null;
    public Coroutine DamagedInveincibleCoroutine = null;
    public Coroutine BlinkCoroutine = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("EnemyBullet"))
        {
            if (!IsInvincible)
            {
                TakeDamage(1);
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("EnemyBullet"))
        {
            if (!IsInvincible)
            {
                TakeDamage(1);
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            if(!IsInvincible)
            {
                TakeDamage(1);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            if (!IsInvincible)
            {
                TakeDamage(1);
            }
        }
    }

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_PlayerControl = GetComponent<PlayerControl>();
        IsDead = false;
    }

    void Update()
    {
        UpdateState();
    }

    void UpdateState()
    {
        if(RoundTimer >= 0)
            RoundTimer -= Time.deltaTime;
        if (MaxHP < CurrentHP)
            CurrentHP = MaxHP;
        if (!IsDead && CurrentHP <= 0)
            StartCoroutine(Die());

    }

    public void Fire()
    {
        /*
        if (!IsRolling && RoundTimer <= 0)
        {
            GameObject bullet = GameObject.Instantiate(PlayerBulletPrefab);
            Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MousePos.z = 0;
            Vector2 dir = (MousePos - GunPos.position).normalized;
            bullet.GetComponent<Bullet>().Init(GunPos.position, Damage * DamageScaling, Damage * DamageScaling * BossDamageScaling, Velocity, dir);
            Vector3 bulletScale = bullet.transform.localScale;
            bulletScale.x *= BulletSizeScaling;
            bulletScale.y *= BulletSizeScaling;
            bullet.transform.localScale = bulletScale;
            bullet.transform.SetParent(m_BulletManager);
            RoundTimer = Round;
        }
        */
    }

    public void UseBlank(int p_blank)
    {
        if(Blank > 0)
        {
            Blank -= p_blank;
            GameObject blank = GameObject.Instantiate(BlankEffectPrefab);
            blank.GetComponent<BlankEffect>().Init(this.transform.position, BlankEffectScale);
        }
    }

    public void Roll()
    {
        RollCoroutine = StartCoroutine(SetRoll(RollInvincibleDuration));
        DamagedInveincibleCoroutine = StartCoroutine(SetInvincible(RollInvincibleDuration));
    }

    protected IEnumerator SetRoll(float p_time)
    {
        IsRolling = true;
        yield return new WaitForSeconds(p_time);
        IsRolling = false;
        RollCoroutine = null;
    }
    // p_time초 동안 무적
    protected IEnumerator SetInvincible(float p_time)
    {
        Debug.Log("무적 시작");
        IsInvincible = true;

        yield return new WaitForSeconds(p_time);

        IsInvincible = false;
        DamagedInveincibleCoroutine = null;
        Debug.Log("무적 끝");
    }

    // p_time초 동안 깜빡임
    protected IEnumerator SetBlink(float p_time)
    {
        Debug.Log("깜빡임 시작");
        float count = 0f;
        float blinkInterval = 0.1f;

        while (count < p_time)
        {
            m_SpriteRenderer.enabled = !m_SpriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            count += blinkInterval;
        }

        m_SpriteRenderer.enabled = true;
        BlinkCoroutine = null;
        Debug.Log("깜빡임 끝");
    }
    public void TakeDamage(int p_damage)
    {
        if (Armor > 0)
        {
            Armor--;
            UseBlank(0);
        }
        else
            CurrentHP -= p_damage;

        if(CurrentHP > 0)
        {
            DamagedInveincibleCoroutine = StartCoroutine(SetInvincible(DamagedInvincibleDuration));
            BlinkCoroutine = StartCoroutine(SetBlink(DamagedInvincibleDuration));
        }
    }

    IEnumerator Die()
    {
        m_PlayerControl.CanControl = false;
        m_Animator.SetTrigger("Die");
        yield return new WaitForSeconds(3f);
        IsDead = true;
    }
}
