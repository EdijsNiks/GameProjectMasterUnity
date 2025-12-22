using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RigPersistence : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
