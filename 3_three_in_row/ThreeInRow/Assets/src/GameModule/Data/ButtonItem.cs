using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.src.GameModule.Data
{
    public class ButtonItem : ParentItem
    {
        public float left;
        public float top;
        public float right;
        public float bottom;
        public string eventer;

        public Cargo cargo;

        public ButtonItem(GameObject item, int iid, bool isVisible,
            float left, float top, float right, float bottom,
            string eventer)
        {
            this.item = item;
            this.iid = iid;
            this.isVisible = isVisible;
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.eventer = eventer;

            cargo = Cargo.getInstance();
            Debug.Log("this.iid=" + this.iid);
        }

        override public void go()
        {
            if (Input.GetMouseButton(0))
            {
                //Debug.Log("in");
                float mousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                float mousePositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
                if (mousePositionX >= left &&
                    mousePositionX <= right &&
                    mousePositionY <= top &&
                    mousePositionY >= bottom)
                {
                    //Debug.Log("cliiick");
                    cargo.events[eventer] = true;
                }
            }
        }
    }
}