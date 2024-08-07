using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.src.GameModule.Data
{
    public class Cargo
    {
        private Cargo()
        {
            Prepare();
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

        public List<Field> fields;
        public List<Item> items;
        public GameObject fg;
        public GameObject bg;

        public List<int> freeItems;

        public GameStatus status;

        public string path;
        public List<string> icons;        
        public int currentItemCount;
        public int maxItemCount;

        public int iconCost;
        public int currentScore;
        public int leftScore;
        public int leftScoreBase;

        public int rowCount;
        public int columnCount;
        public float top;
        public float left;
        public float sizeHoriz;
        public float sizeVertic;

        public MouseActivity mouseActivity;
        public float mousePositionX;
        public float mousePositionY;
        public int mouseArrayItem;
        public int mouseArrayField;
        public int verHorMoving; // 1- horiz, -1 - vertic
        public int secondArrayItem;
        public int secondArrayField;

        public Dictionary<string, bool> events;

        public List<ParentItem> allItems;

        public void Prepare()
        {
            fields = new List<Field>();
            items = new List<Item>();
            freeItems = new List<int>();
            status = GameStatus.prepare;
            path = "";
            icons = new List<string>();
            currentItemCount = 0;
            maxItemCount = 0;
            rowCount = 0;
            columnCount = 0;
            top = 0;
            left = 0;
            sizeHoriz = 0;
            sizeVertic = 0;
            mouseActivity = MouseActivity.free;
            mousePositionX = 0;
            mousePositionY = 0;
            mouseArrayItem = -1;
            mouseArrayField = -1;
            verHorMoving = 0;
            secondArrayItem = -1;
            secondArrayField = -1;

            iconCost = 0;
            currentScore = 0;
            leftScore = 0;
            leftScoreBase = 0;

            allItems = new List<ParentItem>();
            events = new Dictionary<string, bool>();
        }
    }
}
