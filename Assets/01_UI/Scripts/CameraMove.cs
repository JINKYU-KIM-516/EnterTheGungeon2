using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private Player m_PlayerInfo;

    public Transform Target;
    //[SerializeField] private Vector3 Offset;
    private Vector3 m_DirectionVec = Vector3.zero;

    void Awake()
    {
        m_DirectionVec = Target.position - transform.position;
        m_PlayerInfo = Player.GetComponent<Player>();
    }

    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position
        , Target.position - m_DirectionVec
        , m_PlayerInfo.MoveSpeed * Time.deltaTime);
    }
}
