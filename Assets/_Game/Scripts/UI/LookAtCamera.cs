using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }
    [SerializeField] private Mode mode;

    private Camera mainCm;

    private void Start()
    {
        mainCm = Camera.main;
    }

    private void LateUpdate()
    {
        LookingTypes();
    }

    private void LookingTypes()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(mainCm.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCam = transform.position - mainCm.transform.position;
                transform.LookAt(transform.position + dirFromCam);
                break;
            case Mode.CameraForward:
                transform.forward = mainCm.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -mainCm.transform.forward;
                break;
        }
    }
}
