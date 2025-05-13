using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject tooltipImage; // ������ � ������������ ���������
    private Text tooltipText;        // ����� � ���������
    public string message = "���� ��������� ��������� �����!";
    public Vector2 offset = new Vector2(20f, -20f);  // �������� ������ � ����
    void Start()
    {
        BattleManager bm = FindFirstObjectByType<BattleManager>();
        tooltipImage = bm.tooltipImage;
        tooltipText = bm.tooltipText;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipImage.SetActive(true);
        tooltipText.text = message;
        UpdateTooltipPosition(eventData);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipImage.SetActive(false);
    }
    private void UpdateTooltipPosition(PointerEventData eventData)
    {
        Vector2 mousePosition = eventData.position + offset;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        RectTransform tooltipRect = tooltipImage.GetComponent<RectTransform>();
        float tooltipWidth = tooltipRect.rect.width;
        float tooltipHeight = tooltipRect.rect.height;
        if (mousePosition.x + tooltipWidth > screenWidth)
        {
            mousePosition.x = screenWidth - tooltipWidth - 10;  // ������ �� ������ �������
        }
        if (mousePosition.y + tooltipHeight > screenHeight)
        {
            mousePosition.y = screenHeight - tooltipHeight - 10;  // ������ �� ������ �������
        }
        tooltipImage.transform.position = mousePosition;
    }
}