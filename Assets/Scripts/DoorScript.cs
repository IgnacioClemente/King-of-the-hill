using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] Transform door;

    bool isOpened = false;

   private void OnTriggerEnter(Collider other)
    {
        if(!isOpened)
        {
            isOpened = true;
            Vector3 endPos = door.transform.position + new Vector3(0, 4, 0);
            door.DOMove(endPos, 1f).SetEase(Ease.OutQuad);
        }
    }
}
