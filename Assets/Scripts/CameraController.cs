using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public Transform cameraTarget;

    public float distanceToTarget = 3.0f, cameraHeight = 3.0f;
    public float groundSmoothTime = 2.0f;

    Vector3 currentCameraGroundPositon, currentCameraGroundPositionVelocity;

    Vector3 GetCameraGroundTarget()
    {
        return cameraTarget.position + cameraTarget.forward * -distanceToTarget;
    }
    void Start()
    {
        currentCameraGroundPositon = GetCameraGroundTarget();
    }

    // Update is called once per frame
    void Update()
    {
        currentCameraGroundPositon = 
            Vector3.SmoothDamp(
                currentCameraGroundPositon, 
                GetCameraGroundTarget(), 
                ref currentCameraGroundPositionVelocity, 
                groundSmoothTime);

        Vector3 cameraAirPosition = currentCameraGroundPositon + Vector3.up * cameraHeight;

        transform.position = cameraAirPosition;
        transform.LookAt(cameraTarget);
    }
}
