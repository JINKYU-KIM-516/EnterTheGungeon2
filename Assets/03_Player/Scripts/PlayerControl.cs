using UnityEngine;
using static UnityEngine.InputSystem.InputSettings;

public class PlayerControl : MonoBehaviour
{
    protected Animator m_Animator;
    protected Player m_Player;
    protected Rigidbody2D m_Rigidbody;

    public bool CanControl;

    private int BlankConsumption = 1;
    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Player = GetComponent<Player>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        CanControl = true;
    }

    void Update()
    {
        UpdateMove();
        UpdateRoll();
        UpdateFire();
        UpdateBlank();
    }
    
    void UpdateMove_Normalized()
    {
        float xx = Input.GetAxisRaw("Horizontal");
        float yy = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(xx, yy).normalized;
        m_Rigidbody.MovePosition(m_Rigidbody.position + direction * m_Player.MoveSpeed * Time.deltaTime);
    }
    void UpdateMove()
    {
        if (!CanControl)
            return;
        float xx = Input.GetAxisRaw("Horizontal");
        float yy = Input.GetAxisRaw("Vertical");
        this.transform.Translate(xx * m_Player.MoveSpeed * Time.deltaTime, yy* m_Player.MoveSpeed * Time.deltaTime, 0);
    }

    void UpdateRoll()
    {
        // 우클릭 시 구르기 애니메이션 재생
        if (CanControl && Input.GetMouseButtonDown(1))
        {
            m_Animator.SetTrigger("Roll");
            m_Player.Roll();
        }
            
    }

    void UpdateFire()
    {
        if(CanControl && Input.GetMouseButton(0))
            m_Player.Fire();
    }

    void UpdateBlank()
    {
        if(CanControl && Input.GetKeyDown(KeyCode.Q))
            m_Player.UseBlank(BlankConsumption);
    }
}
