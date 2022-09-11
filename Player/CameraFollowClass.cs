using Axis.Abstractions;
using UnityEngine;

public class CameraFollowClass : StateClass<CameraControllerStateManager>
{
    private CameraControllerStateManager _manager;
    
    public override void OnStateEnter(CameraControllerStateManager obj)
    {
        _manager = obj;
    }

    public override void OnStateParametersChange(CameraControllerStateManager obj)
    {
        _manager = obj;
    }

    public override void OnStateUpdate()
    {
        Vector2 _mainCamPos = _manager.MainCam.transform.position;
        _mainCamPos = Vector3.Lerp(_mainCamPos, _manager.transform.position, _manager.CameraSpeed * Time.deltaTime);

        _manager.MainCam.transform.position = new Vector3(
            _mainCamPos.x,
            _mainCamPos.y,
            _manager.MainCam.transform.position.z
        );
    }

    public override void OnStateExit()
    {
        
    }
}
