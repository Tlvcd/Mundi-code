using UnityEngine;

public class UpdateMaterialPosition : MonoBehaviour
{
    [SerializeField]
    Material mat1,mat2;
    int hashCode;

    private void Awake()
    {
        hashCode = Shader.PropertyToID("cutoutPos");
    }

    private void Update()
    {
        var pos = transform.position;
        mat1.SetVector(hashCode, pos);
        mat2.SetVector(hashCode, pos);
    }

}
