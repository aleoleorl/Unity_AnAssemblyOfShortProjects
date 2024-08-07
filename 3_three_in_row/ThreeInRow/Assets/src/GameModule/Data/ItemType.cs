using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.src.GameModule.Data
{
    public enum ItemCase
    {
        inGame, //could use during game
        inDrop,  //going to it place, can't be used
        inHand,
        inSecondHand,
        inDeletePrepare //item should be deleted, waiting for animations
    }
}
