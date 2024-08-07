using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.src.GameModule.Data
{
    public class Field
    {
        public int fid; // id of element

        public int itemId; // index of connected item (-1 if empty)
        public int itemArrayId; //index of item position in the List of items

        public float fieldX; //world coordinats of the field
        public float fieldY;

        public int indexX; //x,y indexes in fields array
        public int indexY;

        public int nextFieldArrayId; //next field where should item go if next field will be free, 
                                     //-1 - the last in chain

        public Field(int fid, int itemId, int itemArrayId, float fieldX, float fieldY, int indexX, int indexY, int nextFieldArrayId)
        {
            this.fid = fid;
            this.itemId = itemId;
            this.itemArrayId = itemArrayId;
            this.fieldX = fieldX;
            this.fieldY = fieldY;
            this.indexX = indexX;
            this.indexY = indexY;
            this.nextFieldArrayId = nextFieldArrayId;
        }
    }
}
