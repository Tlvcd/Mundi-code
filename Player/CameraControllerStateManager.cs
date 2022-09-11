using Axis.Abstractions;
using UnityEngine;

public class CameraControllerStateManager : MonoBehaviour
{
    #region state_specific_vars
    private StateClass<CameraControllerStateManager> CurrState;
    private CameraFollowClass CamFollow = new CameraFollowClass();
    #endregion

    #region state_accessible_vars
    public Camera MainCam { get; private set; }
    [Min(0f)] public float CameraSpeed;
    #endregion

    private void Awake()
    {
        MainCam = Camera.main;
    }

    private void OnEnable()
    {
        CurrState = CamFollow;
        CurrState.OnStateEnter(this);
    }

    private void OnDisable()
    {
        CurrState.OnStateExit();
        CurrState = null;
    }

    void Update()
    {
        CurrState.OnStateUpdate();
    }
    
    [ContextMenu("Pass parameters to current state")]
    public void UpdateParametersInState()
    {
        CurrState.OnStateParametersChange(this);
    }
}
