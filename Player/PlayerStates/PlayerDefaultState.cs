using UnityEngine;
using Axis.Abstractions;
using UnityEngine.InputSystem;
public class PlayerDefaultState// : StateClass<PlayerMovementStateManager> //work in progress
{
   /* public float speed, accel, dampspeed, tempBoost=1f;
    private Vector3 currInput, res;
    private Vector2 refVector, movepos;
    private PlayerMovementStateManager _player;
    private bool isDodging;
    private float timeElapsed=0, maxTime=0.02f;
    
    public override void OnStateEnter(PlayerMovementStateManager obj)
    {
        _player = obj;
        
        accel = _player.Defaultaccel;
        dampspeed = _player.Defaultdampspeed; //ustawia zmienne powiazane z ruchem, zrobione prowizorycznie by sprawdzic jak dziala.
        _player.inputs.BasePlayer.Dodge.performed += Dodge;
        
    }


    public override void OnStateExit()
    {
        _player.inputs.BasePlayer.Dodge.performed -= Dodge;
    }
    private void Dodge(InputAction.CallbackContext obj)
    {
        if (isDodging) { return; }
        isDodging = true;
        tempBoost = 15f;

        
        
    }

    public override void OnStateUpdate()
    {
        
        CalculateDodgeSpeed();
        CalculateMovement();
        if (isDodging) return;
        PlayerMove();
    }

    private void CalculateDodgeSpeed()
    {
        if (isDodging)
        {
            
            tempBoost = Mathf.Lerp(tempBoost, 1, timeElapsed / maxTime);
            
            timeElapsed += Time.deltaTime;
            _player.rb.velocity = movepos* _player._playerStats.moveSpeed *tempBoost;

            if (timeElapsed>maxTime+(maxTime*0.25f))
            {
                _player.rb.velocity = Vector2.zero;
                timeElapsed = 0f;
                isDodging = false;
                tempBoost = 1f;
            }

        }
    }

    private void PlayerMove()
    {
        

        Debug.DrawRay(_player.transform.position, res / 4f, Color.green);
        _player.rb.velocity = res;
        //tempBoost = tempBoost != 1 ? 1 : tempBoost;
        //_player.Dither.SetVector("cutoutPos", new Vector4(_player.transform.position.x, _player.transform.position.y, 0, 0));
    }

    private void CalculateMovement()
    {
        movepos = _player.inputs.BasePlayer.Movement.ReadValue<Vector2>();
        _player.direction = movepos;
        currInput = Vector2.SmoothDamp(currInput, movepos, ref refVector, dampspeed); // pobiera input gracza i wygladza przejscia

        //gdy nie wykryto inputu, ustawia zmienna wygladzania na 0. Jest po to zeby mozna bylo szybko zmienic kierunek po postoju
        currInput = (movepos.magnitude == 0) ? Vector3.zero : currInput;
        res = Vector3.Lerp(res, currInput * _player._playerStats.moveSpeed*tempBoost, accel * Time.deltaTime);

        
    }

    
    */
}
