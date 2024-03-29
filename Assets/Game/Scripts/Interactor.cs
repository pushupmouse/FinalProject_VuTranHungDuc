using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    public float DownwardOffset; // New variable to specify the downward offset

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(InteractorSource.position, InteractRange);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }

    // For visualization in the editor
    private void OnDrawGizmosSelected()
    {
        if (InteractorSource != null)
        {
            Vector3 centerPosition = InteractorSource.position + new Vector3(0f, -DownwardOffset, 0f); // Apply downward offset
            Gizmos.DrawWireSphere(centerPosition, InteractRange);
        }
    }
}