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

    public void JumpPage()
    {
        if (currentPage == 2)
        {
            Next();
        }
        else if (currentPage == 1)
        {
            currentPage = currentPage + 2;
            targetPos += pageStep * 2;
            MovePage();
        }
    }

    public void SprintPage()
    {
        if (currentPage == 1)
        {
            Next();
        }
        else if (currentPage == 3)
        {
            Previous();
        }
    }

    public void DashPage()
    {
        if (currentPage == 2)
        {
            Previous();
        }
        else if (currentPage == 3)
        {
            currentPage = currentPage - 2;
            targetPos -= pageStep * 2;
            MovePage();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    void Update()
    {

    }
}
