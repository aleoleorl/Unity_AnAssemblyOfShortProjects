using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.src.GameModule.Data
{
    public class TextItem : ParentItem
    {
        public TextItem(GameObject item, int iid, bool isVisible)
        {
            this.item = item;
            this.iid = iid;
            this.isVisible = isVisible;
        }
    }
}