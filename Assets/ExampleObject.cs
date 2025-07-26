using UnityEngine;

#if UNITY_EDITOR
#endif

[CreateAssetMenu]
public class ExampleObject : ScriptableObject
{
    [Header("Simple binding")]
    public string simpleLabel = "Hello World!";
}
