using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.src.GameModule.Data;
using UnityEngine;

namespace Assets.src.GameModule
{
    public class Game
    {
        private Cargo cargo;
        private float mx;
        private float my;
        private System.Random rnd;

        public Game()
        {
            cargo = Cargo.getInstance();
            mx = 0;
            my = 0;
            rnd = new System.Random();
        }

        public void Go()
        {
            if (cargo.mouseActivity == MouseActivity.free)
            {
                mouseFree();
            }
            if (cargo.mouseActivity == MouseActivity.click)
            {
                mouseClicked();
            }
            if (cargo.mouseActivity == MouseActivity.drag)
            {
                mouseDrag();
            }
            if (cargo.mouseActivity == MouseActivity.drop)
            {
                mouseDrop();
            }

            if (cargo.freeItems.Count>0)
            {
                addNewItems();
            }
            dropItems();
        }

        private void mouseFree()
        {
            if (Input.GetMouseButton(0))
            {
                cargo.mouseActivity = MouseActivity.click;
                //Debug.Log("MouseActivity.click");
            }
        }

        private void mouseClicked()
        {
            cargo.mousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            cargo.mousePositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

            for (int i = 0; i < cargo.items.Count; i++)
            {
                if (cargo.items[i].itemCase != ItemCase.inGame)
                {
                    continue;
                }
                if (cargo.items[i].itemX - 0.5f <= cargo.mousePositionX &&
                        cargo.items[i].itemX + 0.5f >= cargo.mousePositionX &&
                        cargo.items[i].itemY - 0.5f <= cargo.mousePositionY &&
                        cargo.items[i].itemY + 0.5f >= cargo.mousePositionY)
                {
                    cargo.mouseArrayItem = i;
                    cargo.mouseArrayField = cargo.items[i].onFieldArray;

                    cargo.mouseActivity = MouseActivity.drag;
                    //Debug.Log("cargo.mouseArrayItem=" + cargo.mouseArrayItem);
                    //Debug.Log("cargo.mouseArrayField=" + cargo.items[i].onFieldArray);
                    //Debug.Log("MouseActivity.drag");
                    break;
                }
            }
        }

        private void mouseDrag()
        {
            if (Input.GetMouseButton(0))
            {
                if (cargo.verHorMoving==0)
                {
                    checkDragDirection();
                } else
                {
                    mx = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                    my = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

                    firstItemDrag();
                    secondItemDrag();
                }
            }
            else
            {
                cargo.mouseActivity = MouseActivity.drop;
                //Debug.Log("MouseActivity.drop");
            }
        }

        private void checkDragDirection()
        {
            float dx = Math.Abs(cargo.mousePositionX - Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
            float dy = Math.Abs(cargo.mousePositionY - Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

            if (dx >= dy && dx > 0.14f)
            {
                cargo.verHorMoving = 1;
                //Debug.Log("cargo.verHorMoving=" + cargo.verHorMoving);
            }
            else if (dx < dy && dy > 0.14f)
            {
                cargo.verHorMoving = -1;
                //Debug.Log("cargo.verHorMoving=" + cargo.verHorMoving);
            }
            
        }

        private void firstItemDrag()
        {
            Vector3 temp = cargo.items[cargo.mouseArrayItem].item.transform.position;

            if (cargo.verHorMoving == 1)
            {
                if (mx >= cargo.fields[cargo.mouseArrayField].fieldX - cargo.sizeHoriz &&
                   mx <= cargo.fields[cargo.mouseArrayField].fieldX + cargo.sizeHoriz)
                {
                    cargo.items[cargo.mouseArrayItem].item.transform.position = new Vector3(mx, temp.y, temp.z);
                }
                else if (mx < cargo.fields[cargo.mouseArrayField].fieldX - cargo.sizeHoriz)
                {
                    cargo.items[cargo.mouseArrayItem].item.transform.position = new Vector3(cargo.fields[cargo.mouseArrayField].fieldX - cargo.sizeHoriz, temp.y, temp.z);
                }
                else
                {
                    cargo.items[cargo.mouseArrayItem].item.transform.position = new Vector3(cargo.fields[cargo.mouseArrayField].fieldX + cargo.sizeHoriz, temp.y, temp.z);
                }
            } else
            {                
                if (my >= cargo.fields[cargo.mouseArrayField].fieldY - cargo.sizeVertic &&
                   my <= cargo.fields[cargo.mouseArrayField].fieldY + cargo.sizeVertic)
                {
                    cargo.items[cargo.mouseArrayItem].item.transform.position = new Vector3(temp.x, my, temp.z);
                }
                else if (my < cargo.fields[cargo.mouseArrayField].fieldY - cargo.sizeVertic)
                {
                    cargo.items[cargo.mouseArrayItem].item.transform.position = new Vector3(temp.x, cargo.fields[cargo.mouseArrayField].fieldY - cargo.sizeVertic, temp.z);
                }
                else
                {
                    cargo.items[cargo.mouseArrayItem].item.transform.position = new Vector3(temp.x, cargo.fields[cargo.mouseArrayField].fieldY + cargo.sizeVertic, temp.z);
                }
            }
        }
        private void secondItemDrag()
        {
            Vector3 temp = cargo.items[cargo.mouseArrayItem].item.transform.position;

            if (cargo.secondArrayItem == -1)
            {
                secondItemNotExists();
            }
            else
            {
                secondItemMoving();
            }
            
        }

        private void secondItemNotExists()
        {
            if (cargo.verHorMoving == 1)
            {
                //left or right
                bool lessThen = mx < cargo.fields[cargo.mouseArrayField].fieldX;

                if (lessThen &&
                    cargo.fields[cargo.mouseArrayField].indexX > 0 &&
                    cargo.fields[cargo.mouseArrayField - 1].itemArrayId != -1 &&
                    cargo.items[cargo.fields[cargo.mouseArrayField - 1].itemArrayId].itemCase == ItemCase.inGame
                    )
                {
                    cargo.secondArrayField = cargo.mouseArrayField - 1;
                    cargo.secondArrayItem = cargo.fields[cargo.mouseArrayField - 1].itemArrayId;
                }
                if (!lessThen &&
                    cargo.fields[cargo.mouseArrayField].indexX < cargo.columnCount - 1 &&
                    cargo.fields[cargo.mouseArrayField + 1].itemArrayId != -1 &&
                    cargo.items[cargo.fields[cargo.mouseArrayField + 1].itemArrayId].itemCase == ItemCase.inGame
                    )
                {
                    cargo.secondArrayField = cargo.mouseArrayField + 1;
                    cargo.secondArrayItem = cargo.fields[cargo.mouseArrayField + 1].itemArrayId;
                }                
            }
            else
            {
                //up or down
                bool lessThen = my < cargo.fields[cargo.mouseArrayField].fieldY;

                if (!lessThen && 
                    cargo.fields[cargo.mouseArrayField].indexY > 0 &&
                    cargo.fields[cargo.mouseArrayField - cargo.columnCount].itemArrayId != -1 &&
                    cargo.items[cargo.fields[cargo.mouseArrayField - cargo.columnCount].itemArrayId].itemCase == ItemCase.inGame
                    )
                {
                    cargo.secondArrayField = cargo.mouseArrayField - cargo.columnCount;
                    cargo.secondArrayItem = cargo.fields[cargo.mouseArrayField - cargo.columnCount].itemArrayId;
                }
                if (lessThen && 
                    cargo.fields[cargo.mouseArrayField].indexY < cargo.rowCount - 1 &&
                    cargo.fields[cargo.mouseArrayField + cargo.columnCount].itemArrayId != -1 &&
                    cargo.items[cargo.fields[cargo.mouseArrayField + cargo.columnCount].itemArrayId].itemCase == ItemCase.inGame
                    )
                {
                    cargo.secondArrayField = cargo.mouseArrayField + cargo.columnCount;
                    cargo.secondArrayItem = cargo.fields[cargo.mouseArrayField + cargo.columnCount].itemArrayId;

                }
            }
        }

        private void secondItemMoving()
        {
            if (cargo.verHorMoving == 1)
            {
                //left or right
                bool lessThen = mx < cargo.fields[cargo.mouseArrayField].fieldX;

                //clear secondItem
                if (lessThen && cargo.secondArrayField > cargo.mouseArrayField ||
                    !lessThen && cargo.secondArrayField < cargo.mouseArrayField
                   )
                {
                    cargo.items[cargo.secondArrayItem].item.transform.position =
                        new Vector3(cargo.items[cargo.secondArrayItem].itemX, cargo.items[cargo.secondArrayItem].itemY, 1);
                    cargo.secondArrayItem = -1;
                    cargo.secondArrayField = -1;
                }

                //position of second item
                if (cargo.secondArrayItem != -1)
                {
                    float tx = cargo.fields[cargo.mouseArrayField].fieldX - cargo.items[cargo.mouseArrayItem].item.transform.position.x;
                    cargo.items[cargo.secondArrayItem].item.transform.position = 
                        new Vector3(cargo.fields[cargo.secondArrayField].fieldX + tx, cargo.fields[cargo.secondArrayField].fieldY, 1);
                }
            }
            else
            {
                //up or down
                bool lessThen = my < cargo.fields[cargo.mouseArrayField].fieldY;

                //clear secondItem
                if (!lessThen && cargo.secondArrayField > cargo.mouseArrayField ||
                     lessThen && cargo.secondArrayField < cargo.mouseArrayField
                   )
                {
                    cargo.items[cargo.secondArrayItem].item.transform.position = 
                        new Vector3(cargo.items[cargo.secondArrayItem].itemX, cargo.items[cargo.secondArrayItem].itemY, 1);
                    cargo.secondArrayItem = -1;
                    cargo.secondArrayField = -1;
                }
                if (cargo.secondArrayItem != -1)
                {
                    float ty = cargo.fields[cargo.mouseArrayField].fieldY - cargo.items[cargo.mouseArrayItem].item.transform.position.y;
                    cargo.items[cargo.secondArrayItem].item.transform.position =
                        new Vector3(cargo.fields[cargo.secondArrayField].fieldX, cargo.fields[cargo.secondArrayField].fieldY + ty, 1);
                }
            }
        }

        private void returnTwoItems(bool all=true)
        {
            cargo.items[cargo.mouseArrayItem].item.transform.position = new Vector3(
                            cargo.fields[cargo.mouseArrayField].fieldX,
                            cargo.fields[cargo.mouseArrayField].fieldY,
                            cargo.items[cargo.mouseArrayItem].item.transform.position.z);
            if (all)
            {
                cargo.items[cargo.secondArrayItem].item.transform.position = new Vector3(
                        cargo.fields[cargo.secondArrayField].fieldX,
                        cargo.fields[cargo.secondArrayField].fieldY,
                        cargo.items[cargo.secondArrayItem].item.transform.position.z);
            }
        }
        private void mouseDrop()
        {
            if (cargo.secondArrayItem!=-1)
            {
                float dx = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                float dy = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
                if ((cargo.verHorMoving == 1 && Math.Abs(dx - cargo.fields[cargo.mouseArrayField].fieldX) < cargo.sizeHoriz/2) ||
                    (cargo.verHorMoving == -1 && Math.Abs(dy - cargo.fields[cargo.mouseArrayField].fieldY) < cargo.sizeVertic/2))
                {
                    returnTwoItems();
                    cargo.secondArrayItem = -1;
                }
            }
            if (cargo.secondArrayItem != -1)
            {
                List<int> tempAllField = new List<int>();
               
                //create the list for deleting
                calculateItems(tempAllField, checkAroundItem(cargo.mouseArrayItem, cargo.secondArrayField, cargo.mouseArrayField));
                calculateItems(tempAllField, checkAroundItem(cargo.secondArrayItem, cargo.mouseArrayField, cargo.secondArrayField));
                
                if (tempAllField.Count>0)
                {
                    //put items on new positions
                    cargo.fields[cargo.mouseArrayField].itemArrayId = cargo.secondArrayItem;
                    cargo.fields[cargo.mouseArrayField].itemId = cargo.items[cargo.secondArrayItem].iid;
                    cargo.items[cargo.mouseArrayItem].onFieldArray = cargo.secondArrayField;
                    cargo.items[cargo.mouseArrayItem].onFieldId = cargo.fields[cargo.secondArrayField].fid;
                    cargo.items[cargo.mouseArrayItem].itemX = cargo.fields[cargo.secondArrayField].fieldX;
                    cargo.items[cargo.mouseArrayItem].itemY = cargo.fields[cargo.secondArrayField].fieldY;
                    cargo.items[cargo.mouseArrayItem].item.transform.position = new Vector3(
                            cargo.fields[cargo.secondArrayField].fieldX,
                            cargo.fields[cargo.secondArrayField].fieldY,
                            cargo.items[cargo.secondArrayItem].item.transform.position.z);

                    cargo.fields[cargo.secondArrayField].itemArrayId = cargo.mouseArrayItem;
                    cargo.fields[cargo.secondArrayField].itemId = cargo.items[cargo.mouseArrayItem].iid;
                    cargo.items[cargo.secondArrayItem].onFieldArray = cargo.mouseArrayField;
                    cargo.items[cargo.secondArrayItem].onFieldId = cargo.fields[cargo.mouseArrayField].fid;
                    cargo.items[cargo.secondArrayItem].itemX = cargo.fields[cargo.mouseArrayField].fieldX;
                    cargo.items[cargo.secondArrayItem].itemY = cargo.fields[cargo.mouseArrayField].fieldY;
                    cargo.items[cargo.secondArrayItem].item.transform.position = new Vector3(
                          cargo.fields[cargo.mouseArrayField].fieldX,
                          cargo.fields[cargo.mouseArrayField].fieldY,
                          cargo.items[cargo.mouseArrayItem].item.transform.position.z);

                    //score icon cost
                    scoreIconCost(tempAllField.Count);
                    //clear envolved items and fields
                    clearEnvolvedItems(tempAllField);
                } else
                {
                    returnTwoItems();
                }                
            } else
            {
                returnTwoItems(false);
            }

            cargo.mouseArrayField = -1;
            cargo.mouseArrayItem = -1;
            cargo.secondArrayField = -1;
            cargo.secondArrayItem = -1;
            cargo.mousePositionX = 0;
            cargo.mousePositionY = 0;
            cargo.verHorMoving = 0; 

            cargo.mouseActivity = MouseActivity.free;
        }

        private void scoreIconCost(int score)
        {
            //Debug.Log("score=" + score);
            if (cargo.leftScore > 0)
            {
                cargo.events["newScore"] = true;
                //Debug.Log("cargo.events[\"newScore\"]=" + cargo.events["newScore"]);
                if (cargo.leftScore >= score * cargo.iconCost)
                {
                    cargo.leftScore -= score * cargo.iconCost;
                    cargo.currentScore += score * cargo.iconCost;
                }
                else
                {
                    //Debug.Log("bef: currentScore=" + cargo.currentScore + "; leftScore="+cargo.leftScore);
                    cargo.currentScore += cargo.leftScore;
                    cargo.leftScore = 0;
                    //Debug.Log("aft: currentScore=" + cargo.currentScore + "; leftScore=" + cargo.leftScore);
                }
                //Debug.Log("cargo.currentScore=" + cargo.currentScore);
                //Debug.Log("cargo.leftScore=" + cargo.leftScore);
            }
        }

        private void clearEnvolvedItems(List<int> tempAllField)
        {

            //clear envolved items and fields
            for (int i = 0; i < tempAllField.Count; i++)
            {
                if (cargo.fields[tempAllField[i]].itemId == -1)
                {
                    continue;
                }
                cargo.freeItems.Add(cargo.fields[tempAllField[i]].itemArrayId);
                cargo.items[cargo.fields[tempAllField[i]].itemArrayId].item.transform.position =
                    new Vector3(cargo.left + cargo.sizeHoriz,
                        cargo.top + cargo.sizeVertic,
                        1);
                cargo.items[cargo.fields[tempAllField[i]].itemArrayId].onFieldId = -1;
                cargo.items[cargo.fields[tempAllField[i]].itemArrayId].onFieldArray = -1;
                cargo.items[cargo.fields[tempAllField[i]].itemArrayId].itemCase = ItemCase.inDrop;
                cargo.items[cargo.fields[tempAllField[i]].itemArrayId].item.SetActive(false);

                cargo.fields[tempAllField[i]].itemId = -1;
                cargo.fields[tempAllField[i]].itemArrayId = -1;
            }
        }

        private void calculateItems(List<int> mainList, List<int> addedList)
        {
            int j = 0;
            while (addedList.Count > 0)
            {
                mainList.Add(addedList[0]);
                addedList.RemoveAt(0);
                j++;
                if (j > 10)
                {
                    //Debug.Log("eer");
                    break;
                }
            }
        }
        private List<int> checkAroundItem(int mainItemId, int mainNewPosition, int mainOldPosition)
        {
           // Debug.Log("in=" + mainItemId);
            List<int> tempAllField = new List<int>();
            List<int> tempField = new List<int>();
            int tempFieldPos=-1;
            //hor
            //left
            if (cargo.fields[mainNewPosition].indexX > 0)
            {
                for (int i = cargo.fields[mainNewPosition].indexX - 1; i >= 0; i--)
                {
                    if (mainOldPosition != -1 && i == cargo.fields[mainOldPosition].indexX)
                    {
                        break;
                    }
                    else
                    {
                        tempFieldPos = mainNewPosition - (cargo.fields[mainNewPosition].indexX - i);
                        if (cargo.fields[tempFieldPos].itemArrayId==-1 ||
                            cargo.items[cargo.fields[tempFieldPos].itemArrayId].itemCase != ItemCase.inGame)
                        {
                            break;
                        }
                        if (cargo.items[cargo.fields[tempFieldPos].itemArrayId].itemType ==
                            cargo.items[mainItemId].itemType)
                        {
                            tempField.Add(tempFieldPos);
                        }
                        else
                        {
                            break;
                        }
                    }  
                }
            }
            //right
            if (cargo.fields[mainNewPosition].indexX < cargo.columnCount - 1)
            {
                for (int i = cargo.fields[mainNewPosition].indexX + 1; i <= cargo.columnCount - 1; i++)
                {
                    if (mainOldPosition!=-1 && i == cargo.fields[mainOldPosition].indexX)
                    {
                        break;
                    }
                    else
                    {
                        tempFieldPos = mainNewPosition + (i - cargo.fields[mainNewPosition].indexX);
                        if (cargo.fields[tempFieldPos].itemArrayId == -1 ||
                            cargo.items[cargo.fields[tempFieldPos].itemArrayId].itemCase != ItemCase.inGame)
                        {
                            break;
                        }
                        if (cargo.items[cargo.fields[tempFieldPos].itemArrayId].itemType ==
                            cargo.items[mainItemId].itemType)
                        {
                            tempField.Add(tempFieldPos);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            //hor count
            if (tempField.Count>=2)
            {
                //Debug.Log("hor count");
                //string result="";
                //for (int i=0; i<tempField.Count; i++)
                //{
                //    result=result + " "+tempField[i];
                //}
                //Debug.Log("hor=" + tempField.Count + ": " + result);

                calculateItems(tempAllField, tempField);

                //result="";
                //for (int i=0; i<tempAllField.Count; i++)
                //{
                //    result=result+" "+tempAllField[i];
                //}                
                //Debug.Log(tempAllField.Count + ": " + result);
            }

            //vert
            tempField = new List<int>();
            //up
            if (cargo.fields[mainNewPosition].indexY > 0)
            {
                for (int i = cargo.fields[mainNewPosition].indexY - 1; i >= 0; i --)
                {
                    if (mainOldPosition!=-1 && i == cargo.fields[mainOldPosition].indexY)
                    {
                        break;
                    }
                    else
                    {
                        tempFieldPos = mainNewPosition - cargo.columnCount * (cargo.fields[mainNewPosition].indexY - i);
                            //(cargo.fields[mainNewPosition].indexY - i * cargo.columnCount);
                        if (cargo.fields[tempFieldPos].itemArrayId == -1 ||
                            cargo.items[cargo.fields[tempFieldPos].itemArrayId].itemCase != ItemCase.inGame)
                        {
                            break;
                        }
                        if (cargo.items[cargo.fields[tempFieldPos].itemArrayId].itemType ==
                            cargo.items[mainItemId].itemType)
                        {
                            tempField.Add(tempFieldPos);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            //down
            if (cargo.fields[mainNewPosition].indexY < cargo.rowCount - 1)
            {
                for (int i = cargo.fields[mainNewPosition].indexY + 1; i <= cargo.rowCount - 1; i++)
                {
                    if (mainOldPosition!=-1 && i == cargo.fields[mainOldPosition].indexY)
                    {
                        break;
                    }
                    else
                    {
                        tempFieldPos = mainNewPosition + cargo.columnCount * (i - cargo.fields[mainNewPosition].indexY);
                        if (cargo.fields[tempFieldPos].itemArrayId == -1 ||
                            cargo.items[cargo.fields[tempFieldPos].itemArrayId].itemCase != ItemCase.inGame)
                        {
                            break;
                        }
                        if (cargo.items[cargo.fields[tempFieldPos].itemArrayId].itemType ==
                            cargo.items[mainItemId].itemType)
                        {
                            tempField.Add(tempFieldPos);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            //ver count
            if (tempField.Count >= 2)
            {
                //Debug.Log("vert count");
                //string result = "";
                //for (int i = 0; i < tempField.Count; i++)
                //{
                //    result = result + " " + tempField[i];
                //}
                //Debug.Log("vart=" + tempField.Count + ": " + result);

                calculateItems(tempAllField, tempField);

                //result = "";
                //for (int i = 0; i < tempAllField.Count; i++)
                //{
                //    result = result + " " + tempAllField[i];
                //}
                //Debug.Log(tempAllField.Count + ": " + result);
            }

            if (tempAllField.Count>0)
            {
                tempAllField.Add(mainNewPosition);
            }

            return tempAllField;
        }
   
        private void addNewItems()
        {
            for (int i = cargo.fields.Count - 1; i >= 0; i--)
            {
                if (cargo.fields[i].itemArrayId==-1)
                {
                    int current=i;
                    bool stop = false;
                    int state = 0;
                    do
                    {
                        for (int j = 0; j < cargo.fields.Count; j++)
                        {
                            if (cargo.fields[j].nextFieldArrayId == current)
                            {
                                current = j;
                                if (cargo.fields[j].itemArrayId == -1)
                                {
                                    state = 1;
                                }
                                else
                                {
                                    state = 2;
                                }
                                break;
                            }
                        }
                        float tempY = 0;
                        switch (state)
                        {
                            case 0:
                                // no collision, the first item of this chain
                                stop = true;
                                cargo.fields[i].itemArrayId = cargo.freeItems[0];
                                cargo.fields[i].itemId = cargo.items[cargo.freeItems[0]].iid;
                                cargo.items[cargo.freeItems[0]].onFieldArray = i;
                                cargo.items[cargo.freeItems[0]].onFieldId = cargo.fields[i].fid;
                                cargo.items[cargo.freeItems[0]].itemX = cargo.fields[i].fieldX;
                                cargo.items[cargo.freeItems[0]].itemY = cargo.fields[i].fieldY;
                                tempY = cargo.top + cargo.sizeVertic;
                                if (cargo.fields[i].nextFieldArrayId!=-1 &&
                                    cargo.fields[cargo.fields[i].nextFieldArrayId].itemArrayId!=-1 &&
                                    cargo.items[cargo.fields[cargo.fields[i].nextFieldArrayId].itemArrayId].itemCase == ItemCase.inDrop)
                                {
                                    //also in drop
                                    tempY = cargo.items[cargo.fields[cargo.fields[i].nextFieldArrayId].itemArrayId].item.transform.position.y + cargo.sizeVertic;
                                }
                                cargo.items[cargo.freeItems[0]].item.transform.position = new Vector3(
                                    cargo.fields[i].fieldX,
                                    tempY,
                                    1);

                                int currRandom = rnd.Next(cargo.currentItemCount);
                                cargo.items[cargo.freeItems[0]].itemType = currRandom;
                                SpriteRenderer tempSR = new SpriteRenderer();
                                tempSR = cargo.items[cargo.freeItems[0]].item.GetComponent<SpriteRenderer>() as SpriteRenderer;                               
                                tempSR.sprite = newObject(cargo.path + cargo.icons[currRandom]);

                                cargo.freeItems.RemoveAt(0);

                                
                                break;
                            case 1:
                                //item exist, but also empty
                                break;
                            case 2:
                                //item exists, could be shifted
                                stop = true;
                                cargo.fields[i].itemArrayId = cargo.fields[current].itemArrayId;
                                cargo.fields[i].itemId = cargo.items[cargo.fields[current].itemArrayId].iid;
                                cargo.items[cargo.fields[current].itemArrayId].onFieldArray = i;
                                cargo.items[cargo.fields[current].itemArrayId].onFieldId = cargo.fields[i].fid;
                                cargo.items[cargo.fields[current].itemArrayId].itemCase = ItemCase.inDrop;
                                cargo.items[cargo.fields[current].itemArrayId].itemX = cargo.fields[i].fieldX;
                                cargo.items[cargo.fields[current].itemArrayId].itemY = cargo.fields[i].fieldY;
                                tempY = cargo.fields[current].fieldY;

                                cargo.fields[current].itemArrayId = -1;
                                cargo.fields[current].itemId = -1;
                                break;
                            default:
                                break;
                        }

                    } while (cargo.fields[i].itemArrayId != -1 && !stop);
                }
            }
        }

        private Sprite newObject(string obj)
        {
            return MonoBehaviour.Instantiate(Resources.Load(obj, typeof(Sprite))) as Sprite;
        }
   
        private void dropItems()
        {
            for (int i = 0; i < cargo.items.Count; i++)
            {
                if (cargo.items[i].itemCase == ItemCase.inDrop &&
                    cargo.items[i].onFieldArray != -1)
                {
                    Vector3 temp = cargo.items[i].item.transform.position;
                    float speedY = 0.09f;
                    if (temp.y - speedY <= cargo.items[i].itemY)
                    {
                        cargo.items[i].item.transform.position = new Vector3(temp.x, cargo.items[i].itemY, 1);
                        cargo.items[i].itemCase = ItemCase.inGame;
                        analyzeDrop(i);
                    }
                    else
                    {
                        cargo.items[i].item.transform.position = new Vector3(temp.x, temp.y - speedY, 1);
                    }
                    if (cargo.items[i].item.transform.position.y< cargo.top+ cargo.sizeVertic &&
                       !cargo.items[i].item.activeSelf )
                    {
                        cargo.items[i].item.SetActive(true);
                    }
                }
            }
        }

        private void analyzeDrop(int currItem)
        {
            //score icon cost
            List<int> temp = checkAroundItem(currItem, cargo.items[currItem].onFieldArray, -1);
            scoreIconCost(temp.Count);
            clearEnvolvedItems(temp);
        }
    }
}
