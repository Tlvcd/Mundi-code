using UnityEngine;

public class SpriteRendererMaterialAdd : MonoBehaviour
{

    [SerializeField]
    SpriteRenderer[] sprites;

    [SerializeField]
    Material mat;

    private Material _defaultMat;

    private void OnValidate()
    {
        foreach (var sprite in sprites)
        {
            _defaultMat = sprite.sharedMaterials[0];
            sprite.sharedMaterials = new Material[2] {_defaultMat, mat };
        }
    }

}
