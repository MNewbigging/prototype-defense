using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementIndicator : MonoBehaviour
{
  public static MovementIndicator Instance { get; private set; }

  [SerializeField] private MeshRenderer movementIndicator;
  private IEnumerator hideAfterDelayCoroutine;

  private void Awake()
  {
    // Singleton setup
    if (Instance != null)
    {
      Debug.LogError("Found more than one MovementIndicator " + transform);
      Destroy(gameObject);
      return;
    }
    Instance = this;

    // Hide indicator by default
    movementIndicator.enabled = false;
  }

  public void AppearAt(Vector3 position)
  {
    // Move the indicator to new position
    movementIndicator.transform.position = position;

    // Show the indicator
    movementIndicator.enabled = true;

    // Stop coroutine if running
    if (hideAfterDelayCoroutine != null)
    {
      StopCoroutine(hideAfterDelayCoroutine);
    }

    // Hide indicator after delay
    hideAfterDelayCoroutine = HideAfterDelay();
    StartCoroutine(hideAfterDelayCoroutine);
  }

  IEnumerator HideAfterDelay()
  {
    // Length of time to show indicator
    yield return new WaitForSeconds(1.0f);

    // Then hide it
    movementIndicator.enabled = false;
  }
}
