using UnityEngine;

public class DragSprite : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private bool Draggable = true;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        Debug.Log("mouse press: " + gameObject.name);

        if(Draggable)
        {Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        isDragging = true;}
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            // update position while dragging
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x + offset.x, mousePosition.y + offset.y, transform.position.z);
        }
    }

void OnMouseUp()
{
    Debug.Log("mouse released: " + gameObject.name);
    isDragging = false;

    // get draggable collider id
    Collider2D myCollider = GetComponent<Collider2D>();
    if (myCollider != null)
    {
        // buffer (collider check)
        Collider2D[] colliders = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D().NoFilter();

        // collider check
        int count = Physics2D.OverlapCollider(myCollider, filter, colliders);
        Debug.Log("collider detected: " + count + " colliders");

        foreach (var collider in colliders)
        {
            if (collider != null && collider.CompareTag("bone-target"))
            {
                Debug.Log("snapping to target: " + collider.gameObject.name);

                // snap to slot
                transform.position = collider.transform.position;

                // set draggables parent to slot
                // transform.SetParent(collider.transform);

                // disable further dragging
                Draggable = false;

                break;
            }
        }
    }
}
}
