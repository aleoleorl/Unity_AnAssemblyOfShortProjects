using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.src.GameModule.Data
{
    public class Eventer
    {
        public Cargo cargo;

        public Eventer()
        {
            cargo = Cargo.getInstance();
        }

        public void Go()
        {
            checkScores();
            checkLevel();
        }

        private void checkScores()
        {
            //if (pair.Key=="newScore" && pair.Value==true)
            if (cargo.events["newScore"] == true)
            {
                cargo.events["newScore"] = false;

                for (int i = 0; i < cargo.allItems.Count; i++)
                {
                    if (cargo.allItems[i].iid == 1)
                    {
                        TextMesh tempTM;
                        tempTM = cargo.allItems[i].item.GetComponent<TextMesh>() as TextMesh;
                        tempTM.text = Convert.ToString(cargo.currentScore);
                    }
                    if (cargo.allItems[i].iid == 2)
                    {
                        TextMesh tempTM;
                        tempTM = cargo.allItems[i].item.GetComponent<TextMesh>() as TextMesh;
                        tempTM.text = Convert.ToString(cargo.leftScore);
                    }
                }
            }
        }

        private void checkLevel()
        {
            if (cargo.leftScore == 0 && cargo.currentItemCount < cargo.maxItemCount)
            {
                if (cargo.events["nextLevel"] == false)
                {
                    cargo.events["nextLevel"] = true;
                    //cargo.events["goToNextLevel"] = true;
                    for (int i = 0; i < cargo.allItems.Count; i++)
                    {
                        if (cargo.allItems[i].iid == 2)
                        {
                            cargo.allItems[i].item.SetActive(false);
                            cargo.allItems[i].isVisible = false;
                        }
                        if (cargo.allItems[i].iid == 3)
                        {
                            //Debug.Log("is");
                            cargo.allItems[i].item.SetActive(true);
                            cargo.allItems[i].isVisible = true;
                        }
                    }

                }
                if (cargo.events["goToNextLevel"] == true)
                {
                    for (int i = 0; i < cargo.allItems.Count; i++)
                    {
                        if (cargo.allItems[i].iid == 2)
                        {
                            cargo.allItems[i].item.SetActive(true);
                            cargo.allItems[i].isVisible = true;
                        }
                        if (cargo.allItems[i].iid == 3)
                        {
                            Debug.Log("is");
                            cargo.allItems[i].item.SetActive(false);
                            cargo.allItems[i].isVisible = false;
                        }
                    }

                    cargo.events["goToNextLevel"] = false;
                    cargo.currentItemCount++;
                    cargo.events["nextLevel"] = false;
                    cargo.leftScoreBase = Convert.ToInt32(cargo.leftScoreBase * 0.9);
                    cargo.leftScore = cargo.leftScoreBase;

                    cargo.events["newScore"] = true;
                }
            }
            //if (cargo.events["nextLevel"] == true)
            //{

            //}
        }
    }
}
