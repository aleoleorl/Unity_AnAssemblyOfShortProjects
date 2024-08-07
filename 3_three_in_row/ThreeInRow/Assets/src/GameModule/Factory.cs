using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.src.GameModule.Data;
using UnityEngine;

namespace Assets.src.GameModule
{
    public class Factory
    {
        private Cargo cargo;

        public Factory()
        {
            cargo = Cargo.getInstance();
        }

        public void PrepareScene()
        {
            SpriteRenderer tempSR;
            Vector3 pos;
            Item item;
            Field field;
            GameObject tempGameObject;

            System.Random rnd = new System.Random();
            int currRandom = 0;
            int badRandom = -1;
            int x = 0;
            int y = 0;
            int n = 0;
            int nextFieldArrayId;

            cargo.Prepare();

            cargo.fg = new GameObject();
            tempSR = new SpriteRenderer();
            tempSR = cargo.fg.AddComponent<SpriteRenderer>() as SpriteRenderer;
            tempSR.sprite = newObject("fore01");
            pos = cargo.fg.transform.position;
            cargo.fg.transform.position = new Vector3(pos.x, pos.y, 0.5f);
            cargo.fg.name = "foreground";

            cargo.bg = new GameObject();
            tempSR = new SpriteRenderer();
            tempSR = cargo.bg.AddComponent<SpriteRenderer>() as SpriteRenderer;
            tempSR.sprite = newObject("back01");
            pos = cargo.bg.transform.position;
            cargo.bg.transform.position = new Vector3(pos.x, pos.y, 2);
            cargo.bg.name = "background";

            cargo.bg = new GameObject();
            tempSR = new SpriteRenderer();
            tempSR = cargo.bg.AddComponent<SpriteRenderer>() as SpriteRenderer;
            tempSR.sprite = newObject("superback");
            pos = cargo.bg.transform.position;
            cargo.bg.transform.position = new Vector3(pos.x, pos.y, 2.1f);
            cargo.bg.transform.localScale = new Vector3(1.5f, 1.5f);
            cargo.bg.name = "superback";

            cargo.iconCost = 10;
            cargo.currentScore = 0;
            cargo.leftScore = 500;
            cargo.leftScoreBase = 500;

            cargo.allItems = new List<ParentItem>();

            tempGameObject = new GameObject();
            tempGameObject.name = "currentScore";
            TextMesh tempTM;
            tempTM = tempGameObject.AddComponent<TextMesh>() as TextMesh;
            tempTM.text = Convert.ToString(cargo.currentScore);
            tempTM.anchor = TextAnchor.MiddleCenter;
            tempTM.characterSize = 0.2f;
            tempTM.fontSize = 14;
            tempGameObject.transform.position = new Vector3(-0.67f, 3f, -1f);
            TextItem tempText = new TextItem(tempGameObject, 1, true);
            cargo.allItems.Add(tempText); // common score

            tempGameObject = new GameObject();
            tempGameObject.name = "leftScore";
            tempTM = tempGameObject.AddComponent<TextMesh>() as TextMesh;
            tempTM.text = Convert.ToString(cargo.leftScore);
            tempTM.anchor = TextAnchor.MiddleCenter;
            tempTM.characterSize = 0.2f;
            tempTM.fontSize = 14;
            tempGameObject.transform.position = new Vector3(2.76f, 3f, -1f);
            tempText = new TextItem(tempGameObject, 2, true);
            cargo.allItems.Add(tempText); //left score

            tempGameObject = new GameObject();
            tempGameObject.name = "btnNextScore";
            tempTM = tempGameObject.AddComponent<TextMesh>() as TextMesh;
            tempTM.text = "next level";
            tempTM.anchor = TextAnchor.MiddleCenter;
            tempTM.characterSize = 0.2f;
            tempTM.fontSize = 12;
            tempGameObject.transform.position = new Vector3(2.76f, 3f, -1f);
            tempGameObject.SetActive(false);
            ButtonItem tempButton = new ButtonItem(tempGameObject, 3, false, 2.18f, 3.24f, 3.3f, 2.8f, "goToNextLevel");
            cargo.allItems.Add(tempButton); //button next level

            cargo.events.Add("newScore", false);
            cargo.events.Add("nextLevel", false);
            cargo.events.Add("goToNextLevel", false);

            cargo.path = "set02/";
            cargo.icons.Add("item01");
            cargo.icons.Add("item02");
            cargo.icons.Add("item03");
            cargo.icons.Add("item04");
            cargo.icons.Add("item05");
            cargo.icons.Add("item06");
            cargo.icons.Add("item07");
            cargo.icons.Add("item08");
            cargo.currentItemCount = 5;
            cargo.maxItemCount = 8;

            cargo.rowCount = 8;
            cargo.columnCount = 8;
            cargo.top = 2.36f;
            cargo.left = -1.330f;
            cargo.sizeHoriz = 0.68f;
            cargo.sizeVertic = 0.68f;

            for (int i = 0; i < cargo.rowCount * cargo.columnCount; i++)
            {
                currRandom = rnd.Next(cargo.currentItemCount);

                if (y > 1)
                {
                    while (cargo.items[i - cargo.columnCount].itemType == currRandom &&
                        cargo.items[i - 2 * cargo.columnCount].itemType == currRandom)
                    {
                        badRandom = currRandom;
                        currRandom = rnd.Next(cargo.currentItemCount);
                        n++;
                        if (n > 100)
                        {
                            n = 0;
                            currRandom = currRandom != 0 ? 0 : 1;
                            break;
                        }
                    }
                }
                //horiz analyze
                if (x > 1)
                {
                    while ((cargo.items[i - 1].itemType == currRandom &&
                        cargo.items[i - 2].itemType == currRandom) ||
                        currRandom == badRandom)
                    {
                        currRandom = rnd.Next(cargo.currentItemCount);
                        n++;
                        if (n > 100)
                        {
                            n = 0;
                            currRandom = currRandom != 0 ? 0 : 1;
                            break;
                        }
                    }
                }

                tempGameObject = new GameObject();
                tempSR = new SpriteRenderer();
                tempSR = tempGameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
                tempSR.sprite = newObject(cargo.path + cargo.icons[currRandom]);

                item = new Item(
                    tempGameObject, //gameobject item
                    currRandom, // itemType
                    i + 1,  // iid
                    i, //onSquareId
                    cargo.fields.Count, //onSquareArray
                    cargo.left + x * cargo.sizeHoriz, 
                    cargo.top - y * cargo.sizeVertic
                    );
                item.item.transform.position = new Vector3(cargo.left + x * cargo.sizeHoriz,
                                                   cargo.top - y * cargo.sizeVertic,
                                                     1);
                item.item.name = "item" + i;
                
                if (y<cargo.rowCount-1)
                {
                    nextFieldArrayId=i+cargo.columnCount;
                } else
                {
                    nextFieldArrayId = -1;
                }
                
                field = new Field(
                    i, //fid
                    item.iid, //itemId
                    cargo.items.Count, //itemArrayId
                    cargo.left + x * cargo.sizeHoriz, //fieldX
                    cargo.top - y * cargo.sizeVertic, //fieldY
                    x, //indexX
                    y, //indexY
                    nextFieldArrayId //nextFieldArrayId
                    );

                cargo.items.Add(item);
                cargo.fields.Add(field);

                x++;
                if (x==cargo.columnCount)
                {
                    x = 0;
                    y++;
                }
            }

            cargo.status = GameStatus.game;
        }

        private Sprite newObject(string obj)
        {
            return MonoBehaviour.Instantiate(Resources.Load(obj, typeof(Sprite))) as Sprite;
        }
    }
}
