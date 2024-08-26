using UnityEngine;

public class MOVE : MonoBehaviour
{
    public float speed = 2f;  // 이동 속도
    private Vector3 startPosition;  // 시작 위치
    private bool moving = true;  // 이동 중인지 여부

    void Start()
    {
        startPosition = transform.position;  // 현재 위치를 시작 위치로 저장
    }

    void Update()
    {
        if (moving)
        {
            // 오른쪽으로 이동
            transform.Translate(Vector3.right * speed * Time.deltaTime);

            // 5만큼 이동했는지 체크
            if (Vector3.Distance(startPosition, transform.position) >= 5f)
            {
                moving = false;  // 이동 멈춤
            }
        }
    }

}
