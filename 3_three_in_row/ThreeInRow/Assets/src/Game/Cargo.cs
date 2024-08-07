using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.src.Game
{
    public class ComplexItem
    {
        public int itemId;
        public int itemType;
        public int xx;
        public int yy;

        private ComplexItem()
        {

        }
        public ComplexItem(int itemId, int itemType, int xx, int yy)
        {
            this.itemId = itemId;
            this.itemType = itemType;
            this.xx = xx;
            this.yy = yy;
        }
    }
    public class Item
    {
        public GameObject item;
        public int sid;

        public int onSquareId;
        public int onSquareArray;

        public float itemX;
        public float itemY;

        public int itemType;

        public Item()
        {
            item = new GameObject();
            onSquareId = -1;
            onSquareArray = -1;
            sid = -1;
            itemX = 0;
            itemY = 0;
            itemType = -1;
        }
    }
     public class Field
    {
        public int sid;

        public int iconId;
        public int iconArray;

        public float fieldX;
        public float fieldY;

        public int indexX;
        public int indexY;

        public Field()
        {
            sid = -1;
            iconId = -1;
            iconArray = -1;
            fieldX = 0;
            fieldY = 0;
            indexX = -1;
            indexY = -1;
        }
    }

    public class Cargo
    {
        public GameObject fg;
        public GameObject bg;
        public EGameStatus status = EGameStatus.prepare;

        public float top;
        public float left;
        public float sizeHoriz;
        public float sizeVertic;
        public int rowNumber;
        public int columnNumber;
        public List<Item> items;
        public List<Field> fields;

        public string path;
        public List<string> icons;

        public bool mouseActivity;
        public bool mouseDown;
        public bool mouseUp;
        public float mousePositionX;
        public float mousePositionY;
        public int mouseItem;
        public int mouseField;
        public int verHorMoving; // 1- horiz, -1 - vertic
        public int secondItem;
        public int secondField;

        public void Clear()
        {
            status = EGameStatus.prepare;
            fg = null;
            bg = null;
            top = 0;
            left = 0;
            sizeHoriz = 0;
            sizeVertic = 0;
            rowNumber = 0;
            columnNumber = 0;
            items = new List<Item>();
            fields = new List<Field>();
            path = "";
            icons = new List<string>();

            mouseDown = false;
            mouseUp = false;
            mouseActivity = false;
            mousePositionX = -10000f;
            mousePositionY = -10000f;
            mouseItem = -1;
            verHorMoving = 0;
            mouseField = -1;
            secondItem = -1;
            secondField = -1;
        }

        public void MouseClear()
        {
            if (mouseItem != -1)
            {
                if (items[mouseItem].onSquareId != -1)
                {
                    items[mouseItem].item.transform.position = new Vector3(items[mouseItem].itemX, items[mouseItem].itemY, 1);
                }
            }
            if (secondItem != -1)
            {
                if (items[secondItem].onSquareId != -1)
                {
                    items[secondItem].item.transform.position = new Vector3(items[secondItem].itemX, items[secondItem].itemY, 1);
                }
            }
            mouseDown = false;
            mouseUp = false;
            mouseActivity = false;
            mousePositionX = -10000f;
            mousePositionY = -10000f;
            mouseItem = -1;
            verHorMoving = 0;
            mouseField = -1;
            secondItem = -1;
            secondField = -1;

        }
        private Cargo()
        {
            Clear();

        }


        private static Cargo item;
        public static Cargo getInstance()
        {
            if (item == null)
            {
                item = new Cargo();
            }
            return item;
        }
    }
}
