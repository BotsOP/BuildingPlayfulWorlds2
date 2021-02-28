using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public RectTransform selectionBox;
    public List<BasicUnitHandler> unitSelectionList = new List<BasicUnitHandler>();
    public GameObject inventory;
    Outline baseOutline;
    Vector2 startPos;
    bool selectedGround;

    void Start()
    {
        baseOutline = GameObject.Find("Base").GetComponent<Outline>();
    }

    void Update()
    {
        if(!Input.GetKey(KeyCode.LeftShift))
        {
            MouseInput();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            ReleaseSelectionBox();
        }
    }

    void MouseInput()
    {
        
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            startPos = Input.mousePosition;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.CompareTag("MyUnit"))
                {
                    BasicUnitHandler unit = hit.transform.gameObject.GetComponent<BasicUnitHandler>();
                    unitSelectionList.Add(unit);
                    unit.isActive = !unit.isActive;
                    selectedGround = false;
                    WhoIsSelected();
                }

                else
                    selectedGround = true;
            }
        }
        
        else if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.CompareTag("MyUnit"))
                {
                    BasicUnitHandler unit = hit.transform.gameObject.GetComponent<BasicUnitHandler>();
                    DeselectAll();
                    unitSelectionList.Add(unit);
                    unit.isActive = true;
                    selectedGround = false;
                    WhoIsSelected();
                }
                
                else if(hit.transform.gameObject.name == "Base")
                {
                    DeselectAll();
                    WhoIsSelected();
                    baseOutline.enabled = true;
                    inventory.SetActive(true);
                }

                else
                {
                    selectedGround = true;
                    baseOutline.enabled = false;
                    inventory.SetActive(false);
                }
                    
            }
        }

        if (Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.LeftControl))
        {
            ReleaseSelectionBox();
            WhoIsSelected();
        }
        else if(selectedGround && Input.GetMouseButtonUp(0))
        {
            DeselectAll();
            WhoIsSelected();
            ReleaseSelectionBox();
        }

        if(Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }
    }

    void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("MyUnit"))
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                BasicUnitHandler unitScript = unit.GetComponent<BasicUnitHandler>();
                if(!unitScript.isActive)
                    unitSelectionList.Add(unitScript);
                unitScript.isActive = true;
                WhoIsSelected();
            }
        }
    }

    void WhoIsSelected()
    {
        List<BasicUnitHandler> toBeDeleted = new List<BasicUnitHandler>();
        foreach (BasicUnitHandler unit in unitSelectionList)
        {
            if(unit.isActive)
            {
                unit.outline.enabled = true;
            }
            else
            {
                unit.outline.enabled = false;
                toBeDeleted.Add(unit);
            }
        }

        foreach(BasicUnitHandler unit in toBeDeleted)
        {
            unitSelectionList.Remove(unit);
        }
    }

    void DeselectAll()
    {
        foreach (BasicUnitHandler unit in unitSelectionList)
        {
            unit.isActive = false;
        }
    }

    void UpdateSelectionBox(Vector2 curMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }
}
