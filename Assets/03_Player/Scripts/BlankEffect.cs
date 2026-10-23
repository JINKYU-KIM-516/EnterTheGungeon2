using UnityEngine;
using UnityEngine.UIElements;

public class BlankEffect : MonoBehaviour
{
    private float MinScale = 0f;
    public float MaxScale;
    private bool IsScaling = false;

    public float ZoneOutDuration = 3.0f;
    public float ScaleSpeed = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("EnemyBullet"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            //collision.gameObject.GetComponent<Enemy>().ZoneOut(ZoneOutDuration);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    void Start()
    {
        
    }

    public void Init(Vector2 p_pos, float p_scale)
    {
        this.transform.position = p_pos;
        MaxScale = p_scale;
        this.transform.localScale = new Vector3(MinScale, MinScale, MinScale);
        IsScaling = true;
    }
    void Update()
    {
        if(IsScaling)
            UpdateScale();
    }

    void UpdateScale()
    {
        // 현재 크기
        float currentScale = transform.localScale.x;

        // 점점 커지게
        float newScale = Mathf.MoveTowards(currentScale, MaxScale, ScaleSpeed * Time.deltaTime);

        transform.localScale = new Vector3(newScale, newScale, newScale);

        // 목표 크기에 도달하면 멈춤
        if (newScale >= MaxScale)
        {
            IsScaling = false;
            GameObject.Destroy(this.gameObject);
        }
    }
}
