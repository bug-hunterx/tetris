using UnityEngine;
using System.Collections.Generic;
using System;

public class FieldStateManager : MonoBehaviour {

	private List<GameObject>[] fieldState;//one column is arraylist. fieldState.size is number of columns

//	private int[] numberOfCubesInColumns;

	private int rowsMaxNumber;
	private int columnsNumber;
	private float cubeSize;

	public FieldStateManager(int columnsNumber, int rowsNumber, float cubeSize){
		this.rowsMaxNumber = rowsNumber;
		this.columnsNumber = columnsNumber;
		fieldState = new List<GameObject>[columnsNumber];
        for (int i = 0; i < columnsNumber; i++)
        {
            fieldState[i] = new List<GameObject>();
        }
	//	numberOfCubesInColumns = new int[columnsNumber];
		this.cubeSize = cubeSize;
		Debug.Log("Field manager created: columnsNumber = " + columnsNumber + "; rowsnumber = " + rowsNumber);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool addCube(GameObject cube){
		int columnIndex = (int) ((cube.transform.position.x + (float) Screen.width/4)/(cubeSize/2f)) ;
		int rowIndex = fieldState[columnIndex].Count;
        Debug.Log("Cube added into field manager. Column = " + (columnIndex) + ", Row = " + rowIndex);
		if(rowIndex<rowsMaxNumber){
			fieldState[columnIndex].Add(cube);
			destroySameInRow(cube.GetComponent<SpriteRenderer>().sprite, rowIndex, columnIndex);

			return true;
		}else{
			return false;
		}

	}

	private void destroySameInRow (Sprite baseSprite, int baseRowIndex, int baseColumnIndex)
	{
		int minColumnIndexWithSameSprite = baseColumnIndex;
		int maxColumnIndexWithSameSprite = baseColumnIndex;
        int minRowIndexWithSameSprite = baseRowIndex;
        //look for left cube in sequence of same cubes
		for(int i= baseColumnIndex-1;i>=0;i--){
			if(fieldState[i][baseColumnIndex]!=null){
				if(fieldState[i][baseColumnIndex].GetComponent<SpriteRenderer>().sprite == baseSprite){
					minColumnIndexWithSameSprite =i;
				}else{
					break;
				}
			}
		}
        //look for right cube in sequence
		for(int i= baseColumnIndex+1;i<columnsNumber;i++){
			if(fieldState[i][baseColumnIndex]!=null){
				if(fieldState[i][baseColumnIndex].GetComponent<SpriteRenderer>().sprite == baseSprite){
					minColumnIndexWithSameSprite =i;
				}else{
					break;
				}
			}
		}
        //look for lowest cube in sequence
        for (int i = baseRowIndex-1; i >=0; i--)
        {
            if (fieldState[baseColumnIndex][i] != null)
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
        }

		Debug.Log("Same cubes in row No " + baseRowIndex + " between " + minColumnIndexWithSameSprite + " and " + maxColumnIndexWithSameSprite + " columns");
        Debug.Log("Same cubes in column No " + baseColumnIndex + " between " + minRowIndexWithSameSprite + " and " + baseRowIndex + " rows");
		
        if(maxColumnIndexWithSameSprite-minColumnIndexWithSameSprite>=2){
			for(int i=minColumnIndexWithSameSprite;i<=maxColumnIndexWithSameSprite;i++){
				Debug.Log("Destroy object in row No " + baseRowIndex + " and column No " + i);
				Destroy(fieldState[i][baseRowIndex]);
				//Array.Copy(fieldState[)
				//TODO change all field representing arrays. 
				//TODO increse score
			}
		}
        
	}
}
