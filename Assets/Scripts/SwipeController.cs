using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int maxPage;
    public int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;

    [SerializeField] List<Image> maskElements;

    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;

    void Awake()
    {
        currentPage = 2;
        targetPos = levelPagesRect.localPosition;
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    public void MovePage()
    {
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    void Update()
    {
        if (currentPage == 1)
        {
            maskElements[0].color = new Color(1, 1, 1, 1);
            maskElements[1].color = new Color(1, 1, 1, 0.35f);
            maskElements[2].color = new Color(1, 1, 1, 0.35f);
        }
        else if (currentPage == 2)
        {
            maskElements[0].color = new Color(1, 1, 1, 0.35f);
            maskElements[1].color = new Color(1, 1, 1, 1);
            maskElements[2].color = new Color(1, 1, 1, 0.35f);
        }
        else if (currentPage == 3)
        {
            maskElements[0].color = new Color(1, 1, 1, 0.35f);
            maskElements[1].color = new Color(1, 1, 1, 0.35f);
            maskElements[2].color = new Color(1, 1, 1, 1);
        }
    }
}
