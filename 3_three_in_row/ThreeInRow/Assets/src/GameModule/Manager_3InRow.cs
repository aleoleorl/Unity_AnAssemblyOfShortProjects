using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.src.GameModule.Data;

namespace Assets.src.GameModule
{
    public class Manager_3InRow
    {
        private Cargo cargo;
        private Factory factory;
        private Game game;
        private Eventer eventer;

        public Manager_3InRow()
        {
            cargo = Cargo.getInstance();
            factory = new Factory();
            game = new Game();
            eventer = new Eventer();
        }

        public void Go()
        {
            switch (cargo.status)
            {
                case GameStatus.prepare:
                    factory.PrepareScene();
                    break;
                case GameStatus.game:
                    game.Go();
                    eventer.Go();
                    for (int i = 0; i < cargo.allItems.Count; i++)
                    {
                        cargo.allItems[i].go();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
