using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f; // 이동 속도
    public float jumpForce = 5f; // 점프 힘

    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // 내 몸의 물리 기능 가져오기

        // 아이폰 프로 모델용 120프레임 제한 해제 (부드러움!)
        Application.targetFrameRate = 120;
    }

    void Update()
    {
        // 1. 키보드 입력 (WASD)
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 2. 이동 방향 계산
        moveDirection = new Vector3(x, 0, z).normalized;

        // 3. 점프 (스페이스바)
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // 4. 물리 엔진으로 이동 (벽 뚫기 방지)
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}