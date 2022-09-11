using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellsUI : MonoBehaviour
{
    [SerializeField] private PlayerState stateObject;

    [SerializeField] private Image spellPreview, keyBind;


    private Spell currSpell;

    private void OnEnable()
    {
        stateObject.OnSelectedSpellChange += ChangeDisplaySpell;
    }

    private void OnDisable()
    {
        stateObject.OnSelectedSpellChange -= ChangeDisplaySpell;
    }

    private void ChangeDisplaySpell(Spell obj)
    {
        currSpell = obj;
        spellPreview.sprite = currSpell.Sprite;

        if (!currSpell)
        {
            keyBind.enabled = false;
            return;
        }
        keyBind.enabled = true;
    }

    private void Update()
    {
        if (!currSpell) return;

        spellPreview.fillAmount = currSpell.CoolDownProgress();
    }
}
