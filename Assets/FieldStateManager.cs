using System;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

public class FieldStateManager
{

    private List<GameObject> allCubesOnField;//all cubes. unsorted

    private int rowsMaxNumber;
    private int columnsNumber;
    private float cubeSize;
	ScoreScalable scoreScript;

	private int minNumberInRowOrColumn = 3;

    public FieldStateManager(int columnsNumber, int rowsNumber, float cubeSize)
    {
        this.rowsMaxNumber = rowsNumber;
        this.columnsNumber = columnsNumber;
		allCubesOnField = new List<GameObject>();
        
        this.cubeSize = cubeSize;

		scoreScript = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScalable>();
    }

    public bool addCube(GameObject cube)
    {
        //use for weight system
		//int columnIndex = (int)((cube.transform.position.x + (float)Screen.width / 4) / (cubeSize / 2f));

		int rowIndex = (int)((cube.transform.position.y) / (cubeSize / 2f));
        Debug.Log("Cube added into field manager. Row = " + rowIndex);
        if (rowIndex < rowsMaxNumber)
        {
            allCubesOnField.Add(cube);
            printState();
			destroySameInRowAndColumn((OneElementManager) cube.GetComponentInChildren(typeof(OneElementManager)));

            return true;
        }
        else
        {
            return false;
        }
        

    }

    private void printState()
    {

			//Debug.Log("cubes : " + allCubesOnField.ForEach);     
        
        
    }

	private void destroySameInRowAndColumn(OneElementManager baseCube)
    {
        List<OneElementManager> neigborsH = new List<OneElementManager>();
		List<OneElementManager> neigborsV = new List<OneElementManager>();

		//TODO fix this ugly code. what getObject* methods should return? Is there any othere object can be touched except OneElement? 
		//check horizontaly to left
		OneElementManager temp = convertToOneElement(baseCube.getObjectTouchedLeft());
		while(temp!=null){
			addIntoListIfSameTextures(baseCube, temp, neigborsH);
			temp = convertToOneElement(temp.getObjectTouchedLeft());
		}
		//check horizontaly to right
		temp = convertToOneElement(baseCube.getObjectTouchedRight());
		while(temp!=null){
			addIntoListIfSameTextures(baseCube, temp, neigborsH);
			temp = convertToOneElement(temp.getObjectTouchedRight());
		}
		//check verticaly down
		temp = convertToOneElement(baseCube.getObjectTouchedBottom());
		while(temp!=null){
			addIntoListIfSameTextures(baseCube, temp, neigborsV);
			temp = convertToOneElement(temp.getObjectTouchedBottom());
		}
		//check verticaly up - for old cube after world will moved on
		temp = convertToOneElement(baseCube.getObjectTouchedTop());
		while(temp!=null){
			addIntoListIfSameTextures(baseCube, temp, neigborsV);
			temp = convertToOneElement(temp.getObjectTouchedTop());
		}



		int numberOfCubesDestroyed = 0;
        
        //Debug.Log("Same cubes in row No " + baseRowIndex + " between " + minColumnIndexWithSameSprite + " and " + maxColumnIndexWithSameSprite + " columns");
        //Debug.Log("Same cubes in column No " + baseColumnIndex + " between " + minRowIndexWithSameSprite + " and " + baseRowIndex + " rows");

		if (neigborsH.Count >= minNumberInRowOrColumn-1)
        {
            //delete cube in row - including base one
            foreach(OneElementManager oneElement in neigborsH){
				UObject.Destroy(oneElement.gameObject);
				numberOfCubesDestroyed++;
			}
			UObject.Destroy(baseCube.gameObject);
        }

		if (neigborsV.Count >= minNumberInRowOrColumn-1)

        {
			Debug.Log(neigborsV.Count + " cubes in vertical line about to delete");
            //delete cube in column. Chack if base one still exist  
			if(numberOfCubesDestroyed==0){
				UObject.Destroy(baseCube.gameObject);
			}
			foreach(OneElementManager oneElement in neigborsV){
				UObject.Destroy(oneElement.gameObject);
				numberOfCubesDestroyed++;
			}
        }

		scoreScript.updateScore(numberOfCubesDestroyed);


    }

	private void addIntoListIfSameTextures (OneElementManager baseCube, OneElementManager temp, List<OneElementManager> neigborsList)
	{
		if(baseCube.GetComponent<SpriteRenderer>().sprite.Equals(temp.GetComponent<SpriteRenderer>().sprite)){
			neigborsList.Add(temp);
		}
	}

	private OneElementManager convertToOneElement(RaycastHit2D hit){
		if(hit && hit.collider.gameObject.GetComponentInChildren(typeof(OneElementManager))!=null){
			return (OneElementManager) hit.collider.gameObject.GetComponentInChildren(typeof(OneElementManager));
		}else{
			return null;
		}
	}
}
