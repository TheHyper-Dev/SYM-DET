using UnityEngine;
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Player player;
    internal Transform TR;
    [SerializeField] float cam_accel = 35f;
    [SerializeField] Vector3 cam_offset = Vector3.zero;
    void Awake()
    {
        TR = GetComponent<Transform>();
    }

    void Update()
    {
        if (!player.LookEnabled) return;
        Vector3 newCamPos = player.TR.position;
        newCamPos.z = -10f;
        TR.position = Vector3.Lerp(TR.position, newCamPos + cam_offset, cam_accel * Time.deltaTime);
    }
}
