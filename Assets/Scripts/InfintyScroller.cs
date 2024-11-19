using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfintyScroller : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public float itemHeight = 200f; // Height of each item in the scroll view.
    public int totalItems; // Total number of items in the content.

    public float scrollSpeed = 500f; // Speed of the continuous scroll.
    public float scrollDuration = 5f; // Duration of the scrolling in seconds.

    private bool isScrolling = false;
    //[SerializeField] Button spinBtn;

    public float maxSnapPositionY = 1220f; // Maximum Y position for snapping.
    public float snapInterval = 245f; // The interval to snap to.

    

    private void Start()
    {
       
    }
    void Update()
    {
        // Check if the content's y position has moved beyond the reset position.
        if (content.anchoredPosition.y >= 1220f)
        {
            // Loop through the child items and move the first item to the end.
            Transform firstChild = content.GetChild(0);
            firstChild.SetAsLastSibling();

            // Adjust the content's anchored position to appear seamless.
            content.anchoredPosition -= new Vector2(0, itemHeight);
        }
        else if (content.anchoredPosition.y <= -itemHeight)
        {
            // Loop through the child items and move the last item to the beginning.
            Transform lastChild = content.GetChild(content.childCount - 1);
            lastChild.SetAsFirstSibling();

            // Adjust the content's anchored position to appear seamless.
            content.anchoredPosition += new Vector2(0, itemHeight);
        }
    }

    public IEnumerator ScrollForDuration(float duration)
    {
        float randScrollSpeed = Random.Range(800, scrollSpeed);
        if (!isScrolling)
        {
            isScrolling = true;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                content.anchoredPosition += new Vector2(0, randScrollSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            //Debug.Log("Position before snap " + content.anchoredPosition.y);

            isScrolling = false;
            SnapToPosition();
        }
    }

    private void SnapToPosition()
    {
        //Debug.Log("SNAPPING");
        // Get the current Y position.
        float currentY = content.anchoredPosition.y;

        // Find the highest multiple of snapInterval (245) that is <= maxSnapPositionY (1220).
        float snappedY = Mathf.Floor(currentY / snapInterval) * snapInterval;
        snappedY = Mathf.Clamp(snappedY, 0, maxSnapPositionY);

        // Set the snapped position.
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, snappedY);
        //Debug.Log("After snap " + content.anchoredPosition.y + ",,," + snappedY);
    }

}
