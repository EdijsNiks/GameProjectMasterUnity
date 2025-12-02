using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance.StartLevel();
            Destroy(gameObject);
        }
    }
}

