using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEvent : MonoBehaviour {

    public GameObject player;
    private bool IsRotateAndMoveCamera = false;

    public void CutsceneFunction()
    {
        
    }

    public void TestCutsceneCoroutine(int node)
    {
        Camera mainCamera = Camera.main;
        Vector3 OriginalPlayerPosition = player.transform.GetChild(0).transform.position;
        Vector3 OriginalCameraPosition = mainCamera.transform.position;
        StartCoroutine(TestCutscene(node, OriginalPlayerPosition, OriginalCameraPosition));
    } 

    private IEnumerator TestCutscene(int node, Vector3 originalPlayerPosition, Vector3 originalCameraPosition)
    {
        StaticClasses.IsInCutscene = true;
        Camera mainCamera = Camera.main;
        GameObject TestCutsceneObject = StaticClasses.WhatInteractableItemGO.transform.parent.parent.gameObject;
        switch (node)
        {
            case 1:
                mainCamera.transform.SetParent(null);
                mainCamera.transform.position = TestCutsceneObject.transform.GetChild(0).transform.position;
                mainCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                yield return new WaitForSeconds(5);
                mainCamera.transform.position = originalCameraPosition;
                this.TestCutsceneCoroutine(2);
                yield return null;
                break;
            case 2:
                print("node 2");
                mainCamera.transform.position = TestCutsceneObject.transform.GetChild(0).transform.position;
                IsRotateAndMoveCamera = true;
                StartCoroutine(RotateAndMoveGameObject(mainCamera.gameObject, mainCamera.transform.position, TestCutsceneObject.transform.GetChild(1).transform.position, 0.004f, Vector3.zero, new Vector3(20.0f, 0.0f, 0.0f), 1.0f, 4));
                yield return new WaitUntil(() => IsRotateAndMoveCamera == false);
                //Make sure it's at the correct position.
                mainCamera.transform.rotation = Quaternion.Euler(new Vector3(20.0f, 0.0f, 0.0f));
                mainCamera.transform.position = TestCutsceneObject.transform.GetChild(1).transform.position;
                yield return new WaitForSeconds(5);
                mainCamera.transform.position = originalCameraPosition;
                this.TestCutsceneCoroutine(3);
                yield return null;
                break;
            default:
                print("default case; not on case list, add it!");
                mainCamera.transform.SetParent(player.transform.GetChild(0).transform);
                print(mainCamera.transform.position);
                mainCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                mainCamera.transform.position = originalCameraPosition;
                StaticClasses.IsInCutscene = false;
                yield return null;
                break;
        }
        yield return null;
    }

    private IEnumerator RotateAndMoveGameObject(GameObject gameObject, Vector3 OriginalPosition, Vector3 FinalPosition, float Speed, Vector3 OriginalRotation, Vector3 FinalRotation, float RotationSpeed, int WhichLerp)
    {
        Vector3 rotationVector = Vector3.zero;
        for (float i = 0.0f; i <= 1.0f; i += Speed)
        {
            if (Vector3.Distance(gameObject.transform.position, FinalPosition) < 0.01f)
            {
                if (gameObject.transform.rotation == Quaternion.Euler(FinalRotation))
                {
                    break;
                }
            }
            SmoothLerp(gameObject.gameObject, OriginalPosition, FinalPosition, i, WhichLerp);
            rotationVector = Vector3.Lerp(OriginalRotation, FinalRotation, i * RotationSpeed);
            gameObject.transform.rotation = Quaternion.Euler(rotationVector);
            yield return new WaitForSeconds(0.01f);
        }
        IsRotateAndMoveCamera = false;
        yield return null;
    }

    public void SmoothLerp(GameObject gobject, Vector3 start, Vector3 goal, float alpha, int WhichTween)
    {
        if (WhichTween == 1)
        {
            //SlowFastSlow
            gobject.transform.position = Vector3.Lerp(start, goal, 0.5f * Mathf.Sin(Mathf.PI * alpha - Mathf.PI / 2.0f) + 0.5f);
        } else if (WhichTween == 2)
        {
            //FastSlow
            gobject.transform.position = Vector3.Lerp(start, goal, Mathf.Sin(0.5f * Mathf.PI * alpha));
        } else if (WhichTween == 3)
        {
            //SlowFast
            gobject.transform.position = Vector3.Lerp(start, goal, Mathf.Sin(0.5f * (Mathf.PI * alpha - Mathf.PI)) + 1.0f);
        } else if (WhichTween == 4)
        {
            //FastSlowFast
            gobject.transform.position = Vector3.Lerp(start, goal, (1.0f / Mathf.PI) * Mathf.Asin(2.0f * alpha - 1.0f) + 0.5f);
        }
    }
}
