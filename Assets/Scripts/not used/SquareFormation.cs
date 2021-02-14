using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareFormation : MonoBehaviour
{
    [SerializeField] int amountTargets;
    [SerializeField] int maxRowSize;
    [SerializeField] GameObject sphereTarget;

    Vector3 newPos;

    private IEnumerator coroutine;

    void Start()
    {
        coroutine = WaitAndPrint();
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndPrint()
    {
        int amountRows = 1;
        float newZPos;

        for (int j = 0; j < amountRows; j++)
        {
            newZPos = 1.1f * j;
            Debug.Log("NIEUWE ROW");
            int l = 0;
            int k = 0;
            int flip = 0;
            bool insideWall = false;

            for (int i = 0; i < amountTargets; i++)
            {
                if (i % 2 == 0 && k == 0)
                {
                    flip = 1;
                }
                else
                {
                    if (k == 0)
                        flip = -1;
                    l++;
                }

                newPos = new Vector3(transform.position.x + 1.1f * l * flip, transform.position.y, newZPos);
                GameObject newSphere = Instantiate(sphereTarget, newPos, Quaternion.identity);
                CheckCollision checkCollision = newSphere.GetComponent<CheckCollision>();

                yield return new WaitForSeconds(0.1f);

                if (checkCollision.isColliding)
                {
                    if (i != 0)
                        k++;
                    else
                        insideWall = true;
                    if (k == 1)
                    {
                        i--;
                        Debug.Log(insideWall);
                        if (insideWall)
                            i--;
                    }

                    flip *= -1;
                    if (flip == 1)
                        l--;
                    Destroy(newSphere);
                    if (k == 2)
                    {
                        amountRows++;
                        amountTargets -= i;
                        i += 1000;
                    }
                }

                if(i == maxRowSize - 1)
                {
                    amountRows++;
                    amountTargets -= i;
                    i += 1000;
                }
            }
        }
    }
}
