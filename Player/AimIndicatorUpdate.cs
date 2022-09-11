using UnityEngine;

public class AimIndicatorUpdate : MonoBehaviour
{
    PlayerInputs _input;

    [SerializeField]
    GameObject pointer;

    private readonly Vector2 defaultDir = new Vector2(1,0);

    private void Awake()
    {
        _input = PlayerInputManagerClass.GetInputClass();
    }

    private void Update()
    {
        Vector2 direction = _input.BasePlayer.AimGamepad.ReadValue<Vector2>();

        if (direction != Vector2.zero)
        {
            CalcAngle(direction);
            return;
        }


        direction = _input.BasePlayer.Aim.ReadValue<Vector2>();
        if(direction == Vector2.zero)
        {
            pointer.SetActive(false);
            return;
        }

        direction.x -= Screen.width / 2;
        direction.y -= Screen.height / 2;
        direction.Normalize();
        CalcAngle(direction);

        if(!pointer.activeInHierarchy) pointer.SetActive(true);
    }

    private void CalcAngle(Vector2 direction)
    {
        var idk = -Vector2.SignedAngle(direction, defaultDir);
        transform.rotation = Quaternion.Euler(0, 0, idk);
    }

}
