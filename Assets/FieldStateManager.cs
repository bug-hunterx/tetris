using System;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

public class FieldStateManager
{

    private List<GameObject>[] fieldState;//one column is arraylist. fieldState.size is number of columns

    //	private int[] numberOfCubesInColumns;

    private int rowsMaxNumber;
    private int columnsNumber;
    private float cubeSize;
	ScoreScalable scoreScript;

    public FieldStateManager(int columnsNumber, int rowsNumber, float cubeSize)
    {
        this.rowsMaxNumber = rowsNumber;
        this.columnsNumber = columnsNumber;
        fieldState = new List<GameObject>[columnsNumber];
        for (int i = 0; i < columnsNumber; i++)
        {
            fieldState[i] = new List<GameObject>();
        }
        this.cubeSize = cubeSize;
        Debug.Log("Field manager created: columnsNumber = " + columnsNumber + "; rowsnumber = " + rowsNumber);

		scoreScript = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScalable>();
    }

    public bool addCube(GameObject cube)
    {
        int columnIndex = (int)((cube.transform.position.x + (float)Screen.width / 4) / (cubeSize / 2f));
        int rowIndex = fieldState[columnIndex].Count;
        Debug.Log("Cube added into field manager. Column = " + (columnIndex) + ", Row = " + rowIndex);
        if (rowIndex < rowsMaxNumber)
        {
            fieldState[columnIndex].Add(cube);
            printState();
            destroySameInRowAndColumn(cube.GetComponent<SpriteRenderer>().sprite, rowIndex, columnIndex);

            return true;
        }
        else
        {
            return false;
        }
        

    }

    private void printState()
    {
        for(int column=0;column<fieldState.Length; column++){
            Debug.Log("Column" + column + " size = " + fieldState[column].Count);     
        }
        
    }

    private void destroySameInRowAndColumn(Sprite baseSprite, int baseRowIndex, int baseColumnIndex)
    {
        int minColumnIndexWithSameSprite = baseColumnIndex;
        int maxColumnIndexWithSameSprite = baseColumnIndex;
        int minRowIndexWithSameSprite = baseRowIndex;
		int numberOfCubesDestroyed = 0;
        //look for left cube in sequence of same cubes
        for (int i = baseColumnIndex - 1; i >= 0; i--)
        {
			if (fieldState[i].Count > baseRowIndex && fieldState[i][baseRowIndex] != null)
            {
				if (fieldState[i][baseRowIndex].GetComponent<SpriteRenderer>().sprite == baseSprite)
                {
                    minColumnIndexWithSameSprite = i;
                }
				else
				{
					break;
				}
                
            }
			else
			{
				break;
			}
        }
        //look for right cube in sequence
        for (int i = baseColumnIndex + 1; i < columnsNumber; i++)
        {
			if (fieldState[i].Count > baseRowIndex && fieldState[i][baseRowIndex] != null)
            {
				if (fieldState[i][baseRowIndex].GetComponent<SpriteRenderer>().sprite == baseSprite)
                {
                    maxColumnIndexWithSameSprite = i;
				}
				else
				{
					break;
				}
                
            }
			else
			{
				break;
			}
        }
        //look for lowest cube in sequence
        for (int i = baseRowIndex - 1; i >= 0; i--)
        {
            if (fieldState[baseColumnIndex][i].GetComponent<SpriteRenderer>().sprite == baseSprite)
            {
                minRowIndexWithSameSprite = i;
            }
            else
            {
                break;
            }

        }

        Debug.Log("Same cubes in row No " + baseRowIndex + " between " + minColumnIndexWithSameSprite + " and " + maxColumnIndexWithSameSprite + " columns");
        Debug.Log("Same cubes in column No " + baseColumnIndex + " between " + minRowIndexWithSameSprite + " and " + baseRowIndex + " rows");

        if (maxColumnIndexWithSameSprite - minColumnIndexWithSameSprite >= 2)
        {
            //delete cube in row - including base one
            for (int i = minColumnIndexWithSameSprite; i <= maxColumnIndexWithSameSprite; i++)
            {
                Debug.Log("Destroy object in row No " + baseRowIndex + " and column No " + i);
                UObject.Destroy(fieldState[i][baseRowIndex]);
				fieldState[i].RemoveAt(baseRowIndex);
				numberOfCubesDestroyed++;
            }
        }

        if (baseRowIndex - minRowIndexWithSameSprite >= 2)

        {
			int numberOfDeleted = 0;
            //delete cube in column. Chack if base one still exist  
            for (int i = minRowIndexWithSameSprite; i <= System.Math.Min(baseRowIndex, fieldState[baseColumnIndex].Count-1); i++)
            {
                Debug.Log("Destroy object in row No " + baseRowIndex + " and column No " + i);
                UObject.Destroy(fieldState[baseColumnIndex][i]);
				numberOfDeleted++;
				numberOfCubesDestroyed++;
            }
			fieldState[baseColumnIndex].RemoveRange(minRowIndexWithSameSprite, numberOfDeleted); 
        }

		scoreScript.updateScore(numberOfCubesDestroyed);


    }
}
