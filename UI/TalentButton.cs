using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalentButton : MonoBehaviour
{
    [SerializeField]
    Talent talentAsset;

    [SerializeField]
    TalentManager manager;

    [SerializeField]
    Button parentButton;
    [SerializeField]
    TMP_Text buttonText;

    [SerializeField]
    TalentButton unlockAfterCompletion;

    [field: SerializeField]
    public bool Locked { get; private set; }
    public event System.Action<bool> onLockStateChange;

    private void Awake()
    {
        parentButton.interactable = !Locked;
        buttonText.text = talentAsset.name;
    }

    public void UnlockField()
    {
        Locked = false;
        onLockStateChange?.Invoke(false);

        parentButton.interactable = true;
    }

    public void LockField()
    {
        Locked = true;
        onLockStateChange?.Invoke(true);

        parentButton.interactable = false;
    }

    public void UnlockTalent()
    {
        if (!manager.TryUnlockTalent(talentAsset, (int)talentAsset.price)) return;

        LockField();
        unlockAfterCompletion?.UnlockField();
    }


}
