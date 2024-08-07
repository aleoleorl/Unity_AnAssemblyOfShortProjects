using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.src.GameModule.Data
{
    public class Item
    {
        public GameObject item;
        public int itemType;
        public int iid;

        public int onFieldId;
        public int onFieldArray;

        public float itemX;
        public float itemY;

        public ItemCase itemCase;
        public Item(GameObject item, int itemType, int iid, int onSquareId, int onSquareArray, float itemX, float itemY)
        {
            itemCase = ItemCase.inGame;
            this.item = item;
            this.itemType = itemType;
            this.iid = iid;
            this.onFieldId = onSquareId;
            this.onFieldArray = onSquareArray;
            this.itemX = itemX;
            this.itemY = itemY;
        }
    }
}
