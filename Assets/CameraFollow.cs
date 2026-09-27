using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 쫓아다닐 대상 (플레이어)
    public Vector3 offset;   // 떨어져 있을 거리 (등 뒤 높이)

    // LateUpdate는 모든 물체가 다 움직인 '후'에 실행됨 (카메라 덜덜거림 방지)
    void LateUpdate()
    {
        if (target != null)
        {
            // 플레이어 위치 + 설정한 거리만큼 떨어져서 따라감
            transform.position = target.position + offset;
        }
    }
}