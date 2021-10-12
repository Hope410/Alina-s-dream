using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerLootSelector : MonoBehaviour
{
  [SerializeField]
  private Camera _playerCamera;

  [SerializeField]
  private Image _pivotImage;

  [SerializeField]
  private float _rayLength = 2.0f;

  [SerializeField]
  private Loot _selectedLoot;

  [SerializeField]
  private bool _isMousePressed = false;

  [SerializeField]
  private event UnityAction<RaycastHit> onRayTrigger;

  [SerializeField]
  private event UnityAction<RaycastHit> onMousePressed;

  [SerializeField]
  private Color ACTIVE_COLOR = new Color(1, 1, 1, 1);

  [SerializeField]
  private Color INACTIVE_COLOR = new Color(1, 1, 1, 25f);

  private void Awake()
  {
    onRayTrigger += OnRayTriggerHandler;
  }

  private void OnDestroy()
  {
    onRayTrigger -= OnRayTriggerHandler;
  }

  private void Update()
  {
    Vector3 rayOrigin = _playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

    RaycastHit hit;

    _isMousePressed = Input.GetMouseButton(0);

    if (Physics.Raycast(rayOrigin, _playerCamera.transform.forward, out hit, _rayLength) && !_isMousePressed)
    {
      onRayTrigger(hit);
    }

    if (_isMousePressed)
    {
      if (_selectedLoot)
      {
        _selectedLoot.transform.position = rayOrigin + _playerCamera.transform.forward;

        ToggleLootGravity(_selectedLoot, false);
      }
    }
    else
    {
      if (_selectedLoot)
      {
        ToggleLootGravity(_selectedLoot, true);
      }
    }
  }

  private void ToggleLootGravity(Loot loot, bool state)
  {
    var rb = _selectedLoot.GetComponent<Rigidbody>();
    if (rb != null)
    {
      rb.useGravity = state;
      rb.constraints = state ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
    }
  }

  private void OnRayTriggerHandler(RaycastHit hit)
  {
    Loot loot = hit.collider.GetComponent<Loot>();

    if (loot != null)
    {
      if (_selectedLoot == null) {
        _selectedLoot = loot;
        _pivotImage.color = ACTIVE_COLOR;
      }
    }
    else
    {
      if (_selectedLoot != null) {
        _selectedLoot = null;
        _pivotImage.color = INACTIVE_COLOR;
      }
    }
  }
}