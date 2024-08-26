using UnityEngine;

public class MOVE : MonoBehaviour
{
    public float speed = 2f;  // �̵� �ӵ�
    private Vector3 startPosition;  // ���� ��ġ
    private bool moving = true;  // �̵� ������ ����

    void Start()
    {
        startPosition = transform.position;  // ���� ��ġ�� ���� ��ġ�� ����
    }

    void Update()
    {
        if (moving)
        {
            // ���������� �̵�
            transform.Translate(Vector3.right * speed * Time.deltaTime);

            // 5��ŭ �̵��ߴ��� üũ
            if (Vector3.Distance(startPosition, transform.position) >= 5f)
            {
                moving = false;  // �̵� ����
            }
        }
    }

}
