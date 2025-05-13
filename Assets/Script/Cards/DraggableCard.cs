using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    public int originalIndex;
    private Vector3 offset;
    private Camera cam;
    public CardDrag dragManager;
    void Start()
    {
        cam = Camera.main;

        if (dragManager == null)
            dragManager = FindFirstObjectByType<CardDrag>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalIndex = transform.GetSiblingIndex();
        offset = transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(transform.position).z));
        dragManager.StartDragging(this);
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(transform.position).z));
        transform.position = worldPos + offset;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        DraggableCard target = dragManager.GetClosestCard(this);
        var myRow = GetComponent<Card>().parentRow;
        if (target != null && target != this && myRow != null && myRow.allowCardSwap)
        {
            int myIndex = originalIndex;
            int targetIndex = target.transform.GetSiblingIndex();
            transform.SetSiblingIndex(targetIndex);
            target.transform.SetSiblingIndex(myIndex);
            var list = GetComponent<Card>().parentRow.cards;
            var a = list[myIndex];
            list[myIndex] = list[targetIndex];
            list[targetIndex] = a;
        }
        transform.localPosition = Vector3.zero;
        dragManager.StopDragging();
    }
}