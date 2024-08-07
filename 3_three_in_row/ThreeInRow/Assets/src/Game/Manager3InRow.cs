using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.src.Game
{
    public class Manager3InRow
    {
        private Cargo cargo;

        public Manager3InRow()
        {
            cargo = Cargo.getInstance();
        }

        public void Go()
        {
            switch (cargo.status)
            {
                case EGameStatus.prepare:
                    PrepareMap();
                    break;
                case EGameStatus.game:
                    MouseAnalyze();
                    BallAnalyze();
                    break;
                default:
                    break;
            }
        }

        private int findField(int itemId)
        {
            int result = -1;

            for (int i = 0; i < cargo.fields.Count; i++)
            {
                if (cargo.fields[i].iconId==itemId)
                {
                    result = i;
                }
            }
            return result;
        }

        private void MouseAnalyze()
        {
            //mouse bottom, mouse position, mouse up OR mouse click
            if (!cargo.mouseActivity && Input.GetMouseButton(0))//GetMouseButtonDown(0))
            {
                cargo.mousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                cargo.mousePositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

                for (int i = 0; i < cargo.items.Count; i++ )
                {
                    if (cargo.items[i].itemX-0.5f<=cargo.mousePositionX &&
                        cargo.items[i].itemX+0.5f>=cargo.mousePositionX &&
                        cargo.items[i].itemY-0.5f<=cargo.mousePositionY &&
                        cargo.items[i].itemY+0.5f>=cargo.mousePositionY)
                    {
                        cargo.mouseActivity = true;
                        cargo.mouseDown = true;
                        cargo.mouseItem = i;
                        cargo.mouseField = findField(cargo.mouseItem);
                        break;
                    }
                }                   
            }

            if (cargo.mouseActivity && Input.GetMouseButton(0))
            {
                if (cargo.verHorMoving == 0)
                {
                    float dx = Math.Abs(cargo.items[cargo.mouseItem].itemX - Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
                    float dy = Math.Abs(cargo.items[cargo.mouseItem].itemY - Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
 
                    if (dx>=dy && dx>0.11f)
                    {
                        cargo.verHorMoving = 1;
                    }
                    else if (dx < dy && dy > 0.1f)
                    {
                        cargo.verHorMoving = -1;
                    }
                }
            }

            if (cargo.mouseActivity && Input.GetMouseButtonUp(0))
            {
                FieldAnalyze();
                //TODO: analyze of items before clear
                
                cargo.MouseClear();
            }
        }
        private void BallAnalyze()
        {
            //ball shifted
            if (cargo.mouseActivity)
            {
                float dx = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                float dy = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

                if (cargo.verHorMoving == 1)
                {
                    if (cargo.mouseField!=-1)
                    {
                        //1.main item
                        Vector3 temp = cargo.items[cargo.mouseItem].item.transform.position;
                        if (dx >= cargo.fields[cargo.mouseField].fieldX - cargo.sizeHoriz &&
                           dx <= cargo.fields[cargo.mouseField].fieldX + cargo.sizeHoriz)
                        {
                            cargo.items[cargo.mouseItem].item.transform.position = new Vector3(dx, temp.y, temp.z);
                        }
                        else if (dx < cargo.fields[cargo.mouseField].fieldX - cargo.sizeHoriz)
                        {
                            cargo.items[cargo.mouseItem].item.transform.position = new Vector3(cargo.fields[cargo.mouseField].fieldX - cargo.sizeHoriz, temp.y, temp.z);
                        }
                        else
                        {
                            cargo.items[cargo.mouseItem].item.transform.position = new Vector3(cargo.fields[cargo.mouseField].fieldX + cargo.sizeHoriz, temp.y, temp.z);
                        }

                        //2.second item

                        //left or right
                        bool lessThen = dx < cargo.fields[cargo.mouseField].fieldX;

                        //clear secondItem
                        if (cargo.secondItem != -1 &&
                            ((lessThen && cargo.secondField > cargo.mouseField) || (!lessThen && cargo.secondField < cargo.mouseField))
                            )
                        {
                            cargo.items[cargo.secondItem].item.transform.position = new Vector3(cargo.items[cargo.secondItem].itemX, cargo.items[cargo.secondItem].itemY, 1);            
                            cargo.secondItem = -1;
                            cargo.secondField = -1;
                        }

                        //find second item
                        if (cargo.secondItem == -1)
                        {
                            //field
                            if (lessThen && cargo.fields[cargo.mouseField].indexX > 0)
                            {
                                cargo.secondField = cargo.mouseField - 1;   
                            }
                            if (!lessThen && cargo.fields[cargo.mouseField].indexX < cargo.columnNumber - 1)
                            {
                                cargo.secondField = cargo.mouseField + 1;                                
                            }
                            //item
                            if (cargo.secondField != -1)
                            {
                                for (int i = 0; i < cargo.items.Count; i++)
                                {
                                    if (cargo.items[i].sid == cargo.fields[cargo.secondField].iconId)
                                    {
                                        cargo.secondItem = i;
                                        break;
                                    }
                                }
                            }
                        }
                        //position of second item
                        if (cargo.secondItem != -1)
                        {
                            float tx = cargo.fields[cargo.mouseField].fieldX - cargo.items[cargo.mouseItem].item.transform.position.x;
                            cargo.items[cargo.secondItem].item.transform.position = new Vector3(cargo.fields[cargo.secondField].fieldX + tx, cargo.fields[cargo.secondField].fieldY, 1);
                            
                        }
                    }
                    
                }
                if (cargo.verHorMoving == -1)
                {
                    if (cargo.mouseField != -1) 
                    {
                        //1.first item
                        Vector3 temp = cargo.items[cargo.mouseItem].item.transform.position;
                        if (dy >= cargo.fields[cargo.mouseField].fieldY - cargo.sizeVertic &&
                           dy <= cargo.fields[cargo.mouseField].fieldY + cargo.sizeVertic)
                        {
                            cargo.items[cargo.mouseItem].item.transform.position = new Vector3(temp.x, dy, temp.z);
                        }
                        else if (dy < cargo.fields[cargo.mouseField].fieldY - cargo.sizeVertic)
                        {
                            cargo.items[cargo.mouseItem].item.transform.position = new Vector3(temp.x, cargo.fields[cargo.mouseField].fieldY - cargo.sizeVertic, temp.z);                            
                        }
                        else
                        {
                            cargo.items[cargo.mouseItem].item.transform.position = new Vector3(temp.x, cargo.fields[cargo.mouseField].fieldY + cargo.sizeVertic, temp.z);
                        }

                        //2.second item
                        //up or down
                        bool lessThen = dy < cargo.fields[cargo.mouseField].fieldY;

                        //clear secondItem
                        if (cargo.secondItem != -1 &&
                            ((!lessThen && cargo.secondField > cargo.mouseField) || (lessThen && cargo.secondField < cargo.mouseField))
                            )
                        {
                            cargo.items[cargo.secondItem].item.transform.position = new Vector3(cargo.items[cargo.secondItem].itemX, cargo.items[cargo.secondItem].itemY, 1);
                            cargo.secondItem = -1;
                            cargo.secondField = -1;
                        }
                        //find second item
                        if (cargo.secondItem == -1)
                        {
                            //field
                            if (!lessThen && cargo.fields[cargo.mouseField].indexY > 0)
                            {
                                cargo.secondField = cargo.mouseField - cargo.columnNumber;                                
                            }
                            if (lessThen && cargo.fields[cargo.mouseField].indexY < cargo.rowNumber - 1)
                            {
                                cargo.secondField = cargo.mouseField + cargo.columnNumber;

                            }
                            //item
                            if (cargo.secondField != -1)
                            {
                                for (int i = 0; i < cargo.items.Count; i++)
                                {
                                    if (cargo.items[i].sid == cargo.fields[cargo.secondField].iconId)
                                    {
                                        cargo.secondItem = i;
                                        break;
                                    }
                                }
                            }
                        }
                        if (cargo.secondItem != -1)
                        {
                            float ty = cargo.fields[cargo.mouseField].fieldY - cargo.items[cargo.mouseItem].item.transform.position.y;
                            cargo.items[cargo.secondItem].item.transform.position = new Vector3(cargo.fields[cargo.secondField].fieldX, cargo.fields[cargo.secondField].fieldY+ty, 1);

                        }
                    }

                }
            }
        }
        private void FieldAnalyze()
        {
            //analyze chanoging of items positions

            List<int> checkId = new List<int>();
            int currentArray = -1;

            checkId.Add(cargo.mouseItem);
            checkId.Add(cargo.secondItem);
            

            //prepare map
            List<ComplexItem> items = new List<ComplexItem>();
            for (int i = 0; i < cargo.fields.Count; i++)
            {
                int itemType = -1;
                int itemId = -1;
                int xx = cargo.fields[i].indexX;
                int yy = cargo.fields[i].indexY;

                if (i != cargo.mouseField && i != cargo.secondField)
                {                    
                    for (int j = 0; j < cargo.items.Count; j++)
                    {
                        if (cargo.items[j].sid == cargo.fields[i].iconId)
                        {
                            itemType = cargo.items[j].itemType;
                            itemId = cargo.items[j].sid;
                            break;
                        }
                    }                   
                }
                else
                {
                    //if (cargo.mouseField != -1 && cargo.secondItem != -1)
                    {
                        if (i == cargo.mouseField)
                        {
                            itemType = cargo.items[cargo.secondField].itemType;
                            itemId = cargo.items[cargo.secondField].sid;
                        }
                        else
                        {
                            itemType = cargo.items[cargo.mouseField].itemType;
                            itemId = cargo.items[cargo.mouseField].sid;
                        }
                    }
                    //else
                    //{
                    //    return;
                    //}
                }
                items.Add(new ComplexItem(itemId, itemType, xx, yy));
            }

            while (checkId.Count>0)
            {
                if (currentArray==-1)
                {
                    for (int i=0; i<items.Count; i++)
                    {
                        if (items[i].itemId==checkId[0])
                        {
                            currentArray = i;
                            break;
                        }
                    }
                }

                int hor = 1;
                int ver = 1;
                List<int> horArray = new List<int>();
                List<int> verArray = new List<int>();

                if (currentArray != -1)
                {
                    //hor
                    if (items[currentArray].xx > 0)
                    {
                        if (items[currentArray - 1].itemType == items[currentArray].itemType)
                        {
                            horArray.Add(currentArray - 1);
                            hor++;
                            if (items[currentArray - 1].xx > 0)
                            {
                                if (items[currentArray - 2].itemType == items[currentArray].itemType)
                                {
                                    horArray.Add(currentArray - 2);
                                    hor++;
                                }
                            }
                        }
                    }
                    if (items[currentArray].xx < cargo.columnNumber - 1)
                    {
                        if (items[currentArray + 1].itemType == items[currentArray].itemType)
                        {
                            horArray.Add(currentArray + 1);
                            hor++;
                            if (items[currentArray + 1].xx < cargo.columnNumber - 1)
                            {
                                if (items[currentArray + 2].itemType == items[currentArray].itemType)
                                {
                                    horArray.Add(currentArray + 2);
                                    hor++;
                                }
                            }
                        }
                    }
                    //vert
                    if (items[currentArray].yy > 0)
                    {
                        if (items[currentArray - cargo.columnNumber].itemType == items[currentArray].itemType)
                        {
                            verArray.Add(currentArray - cargo.columnNumber);
                            ver++;
                            if (items[currentArray - cargo.columnNumber].yy > 0)
                            {
                                if (items[currentArray - 2 * cargo.columnNumber].itemType == items[currentArray].itemType)
                                {
                                    verArray.Add(currentArray - 2 * cargo.columnNumber);
                                    ver++;
                                }
                            }
                        }
                    }
                    if (items[currentArray].yy < cargo.rowNumber - 1)
                    {
                        if (items[currentArray + cargo.columnNumber].itemType == items[currentArray].itemType)
                        {
                            verArray.Add(currentArray + cargo.columnNumber);
                            ver++;
                            if (items[currentArray + cargo.columnNumber].yy < cargo.rowNumber - 1)
                            {
                                if (items[currentArray + 2 * cargo.columnNumber].itemType == items[currentArray].itemType)
                                {
                                    verArray.Add(currentArray + 2 * cargo.columnNumber);
                                    ver++;
                                }
                            }
                        }
                    }
                }
                if (ver>=3 || hor>=3)
                {
                    if (cargo.mouseItem != -1 && cargo.secondItem != -1)
                    {
                        int temponSquareArray = cargo.items[cargo.mouseItem].onSquareArray;
                        int temponSquareId = cargo.items[cargo.mouseItem].onSquareId;
                        cargo.items[cargo.mouseItem].onSquareArray = cargo.items[cargo.secondItem].onSquareArray;
                        cargo.items[cargo.mouseItem].onSquareArray = cargo.items[cargo.secondItem].onSquareId;
                        cargo.items[cargo.secondItem].onSquareArray = temponSquareArray;
                        cargo.items[cargo.secondItem].onSquareArray = temponSquareId;
                        cargo.items[cargo.mouseItem].itemX = cargo.fields[cargo.secondItem].fieldX;
                        cargo.items[cargo.mouseItem].itemY = cargo.fields[cargo.secondItem].fieldY;
                        cargo.items[cargo.mouseItem].item.transform.position = new Vector3(cargo.items[cargo.mouseItem].itemX, cargo.items[cargo.mouseItem].itemY, 1);
                        cargo.fields[cargo.items[cargo.mouseItem].onSquareArray].iconId = cargo.items[cargo.mouseItem].sid;
                        cargo.fields[cargo.items[cargo.mouseItem].onSquareArray].iconArray = cargo.mouseItem;
                        cargo.fields[cargo.items[cargo.secondItem].onSquareArray].iconId = cargo.items[cargo.secondItem].sid;
                        cargo.fields[cargo.items[cargo.secondItem].onSquareArray].iconArray = cargo.secondItem;
                        cargo.items[cargo.secondItem].itemX = cargo.fields[cargo.mouseItem].fieldX;
                        cargo.items[cargo.secondItem].itemY = cargo.fields[cargo.mouseItem].fieldY;
                        cargo.items[cargo.secondItem].item.transform.position = new Vector3(cargo.items[cargo.secondItem].itemX, cargo.items[cargo.secondItem].itemY, 1);
                        cargo.mouseItem = -1;
                        cargo.secondItem = -1;
                    }
                    clearField(checkId[0]);
                    if (hor>=3)
                    {
                        for (int j=0; j<horArray.Count; j++)
                        {
                            clearField(horArray[j]);
                        }
                    }
                    if (ver >= 3)
                    {
                        for (int j = 0; j < verArray.Count; j++)
                        {
                            clearField(verArray[j]);
                        }
                    }
                }


                hor=1;
                ver=1;
                currentArray = -1;
                checkId.RemoveAt(0);
            }
        }
        private void clearField(int id)
        {
            for (int i=0; i<cargo.fields.Count; i++)
                    {
                        if (cargo.fields[i].iconId == id)
                        {
                            cargo.fields[i].iconId = -1;
                            cargo.fields[i].iconArray = -1;
                            break;
                        }
                    }
                    for (int i = 0; i < cargo.items.Count; i++)
                    {
                        if (cargo.items[i].sid == id)
                        {
                            cargo.items[i].onSquareId = -1;
                            cargo.items[i].onSquareArray = -1;
                            cargo.items[i].item.transform.position = new Vector3(cargo.fields[0].fieldX - cargo.sizeHoriz, cargo.fields[0].fieldY +- cargo.sizeVertic, 1);
                            break;
                        }
                    }
        }

        private void PrepareMap()
        {
            SpriteRenderer tempSR;
            Vector3 pos;
            Item item;
            Field field;

            cargo.Clear();
            cargo.status = EGameStatus.game;

            cargo.fg = new GameObject();
            tempSR = new SpriteRenderer();           
            tempSR = cargo.fg.AddComponent<SpriteRenderer>() as SpriteRenderer;
            tempSR.sprite = newObject("foreground");
            pos = cargo.fg.transform.position;
            cargo.fg.transform.position = new Vector3(pos.x, pos.y, 0);

            cargo.bg = new GameObject();
            tempSR = new SpriteRenderer();
            tempSR = cargo.bg.AddComponent<SpriteRenderer>() as SpriteRenderer;
            tempSR.sprite = newObject("background");
            pos = cargo.bg.transform.position;
            cargo.bg.transform.position = new Vector3(pos.x, pos.y, 2);

            cargo.top = 2.36f;
            cargo.left = -1.050f;
            cargo.sizeHoriz = 0.68f;
            cargo.sizeVertic = 0.68f;
            cargo.rowNumber = 8;
            cargo.columnNumber = 8;

            cargo.path = "seq1/";
            cargo.icons.Add("icon1");
            cargo.icons.Add("icon2");
            cargo.icons.Add("icon3");
            cargo.icons.Add("icon4");
            cargo.icons.Add("icon5");
            cargo.icons.Add("icon6");

            int x = 0;
            int y = 0;
            int n = 0;
            int currRandom = 0;
            int badRandom = -1;
            System.Random rnd = new System.Random();
            for (int i = 0; i < cargo.rowNumber * cargo.columnNumber; i++)
            {
                item = new Item();
                tempSR = new SpriteRenderer();
                tempSR = item.item.AddComponent<SpriteRenderer>() as SpriteRenderer;

                //tempSR.sprite = newObject("seq1/icon1");
                currRandom = rnd.Next(cargo.icons.Count);
                //vertic analyze
                if (y>1)
                {                    
                    while (cargo.items[i-cargo.columnNumber].itemType==currRandom &&
                        cargo.items[i-2*cargo.columnNumber].itemType==currRandom)
                    {
                        badRandom = currRandom;
                        currRandom = rnd.Next(cargo.icons.Count);
                        n++;
                        if (n>100)
                        {
                            n = 0;
                            currRandom = currRandom != 0 ? 0 : 1;
                            break;
                        }
                    }
                }
                //horiz analyze
                if (x>1)
                {
                    while ((cargo.items[i - 1].itemType == currRandom &&
                        cargo.items[i - 2].itemType == currRandom) ||
                        currRandom == badRandom)
                    {
                        currRandom = rnd.Next(cargo.icons.Count);
                        n++;
                        if (n > 100)
                        {
                            n = 0;
                            currRandom = currRandom != 0 ? 0 : 1;
                            break;
                        }
                    }
                }
                tempSR.sprite = newObject(cargo.path + cargo.icons[currRandom]);

                item.item.transform.position = new Vector3(cargo.left + x * cargo.sizeHoriz, 
                                                   cargo.top - y*cargo.sizeVertic, 
                                                     1);
                item.sid = i;
                item.onSquareId = i;
                item.onSquareArray = cargo.fields.Count;
                item.itemX = cargo.left + x * cargo.sizeHoriz;
                item.itemY = cargo.top - y * cargo.sizeVertic;
                item.itemType = currRandom;

                field = new Field();
                field.sid = i;
                field.iconId = item.sid;
                field.iconArray = cargo.items.Count;
                field.fieldX = cargo.left + x * cargo.sizeHoriz;
                field.fieldY = cargo.top - y * cargo.sizeVertic;
                field.indexX = x;
                field.indexY = y;

                cargo.items.Add(item);
                cargo.fields.Add(field);

                x++;
                if (x==cargo.columnNumber)
                {
                    x = 0;
                    y++;
                }
            }

        }

        private Sprite newObject(string obj)
        {
            return MonoBehaviour.Instantiate(Resources.Load(obj, typeof(Sprite))) as Sprite;
        }
    }
}
