using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.src.GameModule.Data
{
    public class ParentItem
    {
        public GameObject item;
        public int iid;
        public bool isVisible;

        public virtual void go()
        {

        }
    }
}
