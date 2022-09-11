using UnityEngine;
using System;
using Axis.Abstractions;


[RequireComponent(typeof(Saveable)), DisallowMultipleComponent]
public class ObjectTransformSave : MonoBehaviour, ISaveable
{
    public void RestoreState(object obj)
    {
        var data = (SaveData)obj;
        var objTF = gameObject.transform;


        objTF.position = data.RetrievePosition();
        objTF.rotation = data.RetrieveRotation();
        objTF.localScale = data.RetrieveScale();

        Debug.Log("new pos set: "+ objTF.position);
    }

    public object SaveState()
    {
        return new SaveData(gameObject.transform);
    }

    [Serializable]
    private struct SaveData
    {
        private string position;
        private string rotation;
        private string scale;

        public Vector3 RetrievePosition()
        {
            Vector3 pos = JsonUtility.FromJson<Vector3>(position);
            return pos;
        }

        public Vector3 RetrieveScale()
        {
            Vector3 lscale = JsonUtility.FromJson<Vector3>(scale);
            return lscale;
        }

        public Quaternion RetrieveRotation()
        {
            Quaternion rot = JsonUtility.FromJson<Quaternion>(rotation);
            return rot;
        }

        public SaveData(Transform tf)
        {
            position = JsonUtility.ToJson(tf.position);
            rotation = JsonUtility.ToJson(tf.rotation);
            scale = JsonUtility.ToJson(tf.localScale);
        }
    }

}
