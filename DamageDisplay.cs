using UnityEngine;
using TMPro;

public class DamageDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro display;
    private static int sortOrder;

    public static void Create(Vector2 pos, float damage, DamageType dType)
    {
        DamageDisplay dis= Instantiate(AssetManager.I.DamageIndicator);
        dis.UpdateDisplay(pos,damage,dType);
    }

    public void UpdateDisplay(Vector2 pos,float damage, DamageType dType)
    {
        var calc = new Vector2(pos.x+ Random.Range(-0.4f, 0.4f),
            pos.y+ Random.Range(-0.8f, 0.8f));
        transform.position = calc;

        damage = (int)damage;
        display.text = damage==0 ? "Miss" :damage.ToString();

        display.color = dType.TypeColor;

        display.sortingOrder = sortOrder;
        sortOrder++;

        LeanTween.scale(gameObject, Vector3.one, 0.333f).setEaseOutExpo();
        LeanTweenExt.LeanAlphaTMP(display, 1, 0.15f).setEaseOutQuad().setOnComplete(() => FadeOutAnim());

    }

    private void FadeOutAnim()
    {
        LeanTweenExt.LeanAlphaTMP(display, 0, 0.3f).setEaseInQuint().setDelay(0.3f);
        LeanTween.moveY(gameObject, transform.position.y + 0.25f, 0.6f).setEaseInQuad().setOnComplete(() => Destroy(gameObject));
    }
    
}
