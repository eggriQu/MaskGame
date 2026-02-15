using System.Collections;
using System.Collections.Generic;
using Menus;
using UnityEngine;

public class MoveCameraButton : MonoBehaviour, IMenuButton
{
    [SerializeField] private Camera UIcamera;
    [SerializeField] private GameObject targetLocation;
    [SerializeField] private float moveDuration;
    public void OnClickMenuButton()
    {
        if (UIcamera == null)
        {
            Debug.Log("UICamera not assigned to button: " + gameObject.name);
        }
        else
        {
            StartCoroutine(SmoothCameraMove(moveDuration));
        }
    }




    private IEnumerator SmoothCameraMove(float duration)
    {
        float elapsedTime = 0;
        Vector3 startPos = UIcamera.transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            UIcamera.gameObject.transform.position = new Vector3(
                Mathf.Lerp(startPos.x, targetLocation.transform.position.x, t),
                Mathf.Lerp(startPos.y, targetLocation.transform.position.y, t),
                Mathf.Lerp(startPos.z, targetLocation.transform.position.z, t));
            yield return null;
        }
        UIcamera.gameObject.transform.position = targetLocation.transform.position;
    }
}
