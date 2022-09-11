using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Axis Mundi/Internal/PAASS")]
public class PlayerAnimationAssets : ScriptableObject
{
    [field: SerializeField, HideInInspector]
    public int CurrentAnimHash { get; private set; }
    public event Action OnAnimChange;
    public event Action<float> OnAnimSpeedChange;

    [SerializeField]
    private PlayerState state;

    public void PlayAnimation(AnimClip clip)
    {
        CurrentAnimHash = 
            AnimHashes[((int)clip)]
            .arr[state.GetPlayerDirection()];
        OnAnimChange?.Invoke();
    }

    public int GetAnimation(AnimClip clip, int direction)
    {
        return AnimHashes[((int)clip)]
                .arr[direction];
    }

    public void AnimationSpeed(float speed)
    {
        OnAnimSpeedChange?.Invoke(speed);
    }


    #region AnimClips
    [SerializeField]
    private AnimationClip[] attackAnim, hitAnim, idleAnim, jumpAnim, spellAnim, walkAnim, deathAnim;

    
    [field:SerializeField,HideInInspector]
    public int[] Attack { get; private set; }
    [field: SerializeField, HideInInspector]
    public int[] Hit { get; private set; }
    [field: SerializeField, HideInInspector]
    public int[] Idle { get; private set; }
    [field: SerializeField, HideInInspector]
    public int[] Jump { get; private set; }
    [field: SerializeField, HideInInspector]
    public int[] Spell { get; private set; }
    [field: SerializeField, HideInInspector]
    public int[] Walk { get; private set; }
    [field: SerializeField, HideInInspector]
    public int[] Death { get; private set; }

    [SerializeField]
    private IntArrays[] AnimHashes;

#if UNITY_EDITOR
    private void OnValidate()
    {
        AnimHashes = new IntArrays[7];

        AnimHashes[0] = new IntArrays( Attack = FillTableWithHash(attackAnim, Attack));
        AnimHashes[1] = new IntArrays(Hit = FillTableWithHash(hitAnim, Hit));
        AnimHashes[2] = new IntArrays(Idle = FillTableWithHash(idleAnim, Idle));
        AnimHashes[3] = new IntArrays(Jump = FillTableWithHash(jumpAnim, Jump));
        AnimHashes[4] = new IntArrays(Spell = FillTableWithHash(spellAnim, Spell));
        AnimHashes[5] = new IntArrays(Walk = FillTableWithHash(walkAnim, Walk));
        AnimHashes[6] = new IntArrays(Death = FillTableWithHash(deathAnim, Death));

        
    }

    private int[] FillTableWithHash(AnimationClip[] origin, int[] destination)
    {
        destination = new int[origin.Length];
        for (int i = 0; i < origin.Length; i++)
        {
            destination[i] = Animator.StringToHash(origin[i].name);
        }

        return destination;
    }
#endif

    #endregion

    [Serializable]
    private struct IntArrays
    {
        [field: SerializeField]
        public int[] arr { get; private set; }

        public IntArrays(int[] array)
        {
            arr = array;
        }

    }
}
public enum AnimClip : int
{
    Attack,
    Hit,
    Idle,
    Jump,
    Spell,
    Walk,
    Death
}
