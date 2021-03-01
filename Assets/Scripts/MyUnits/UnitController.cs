using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    UnitSelection unitSelec;
    GameObject lastSelected;
    void Start()
    {
        unitSelec = gameObject.GetComponent<UnitSelection>();
    }

    void Update()
    {
        MouseInput();
    }

    void MouseInput()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject.tag == "Enemy")
                {
                    Debug.Log("I HIT AN ENEMY");

                    if(unitSelec.unitSelectionList.Count != 0)
                    {
                        if(lastSelected != null)
                        lastSelected.GetComponent<Outline>().enabled = false;

                    lastSelected = hit.transform.gameObject;
                    lastSelected.GetComponent<Outline>().enabled = true;
                    }

                    AttackUnit(hit);
                }
                else
                {
                    Vector3 moveToPos = hit.point;
                    MoveUnit(moveToPos);
                    lastSelected.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }

    void AttackUnit(RaycastHit hit)
    {
        Vector3 moveToPos = hit.point;
        List<Vector3> targetPositionList = GetPositionListAround(moveToPos, new float[] { 0.75f, 1.5f, 2.25f }, new int[] { 5, 10, 20 });

        int targetPositionIndex = 0;

        foreach (BasicUnitHandler unit in unitSelec.unitSelectionList)
        {
            if (unit.isActive)
            {
                unit.SetUnitState(1);
                unit.targetFinder.targetList.Insert(0, hit.collider.gameObject);
                unit.WalkTo(targetPositionList[targetPositionIndex]);
                targetPositionIndex = (targetPositionIndex + 1) % targetPositionList.Count;
            }
        }
    }

    public void MoveUnit(Vector3 moveToPos)
    {
        List<Vector3> targetPositionList = GetPositionListAround(moveToPos, new float[] { 1.5f, 3f, 4.5f }, new int[] { 5, 10, 20 });

        int targetPositionIndex = 0;
        int i = 0;
        foreach (BasicUnitHandler unit in unitSelec.unitSelectionList)
        {
            if (unit.isActive && unit.gameObject.activeSelf)
            {
                i++;
                unit.SetUnitState(0);
                unit.agent.isStopped = false;
                unit.WalkTo(targetPositionList[targetPositionIndex]);
                targetPositionIndex = (targetPositionIndex + 1) % targetPositionList.Count;
            }
        }
    }

    List<Vector3> GetPositionListAround(Vector3 startPos, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPos);
        for(int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPos, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    List<Vector3> GetPositionListAround(Vector3 startPos, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPos + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, angle, 0) * vec;
    }
}
