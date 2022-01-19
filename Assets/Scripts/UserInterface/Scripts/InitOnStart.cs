using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SG
{
    [RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
    [DisallowMultipleComponent]
    public class InitOnStart : MonoBehaviour
    {
        public void SetMaxScroolerItem(int size)
        {
            var ls = GetComponent<LoopScrollRect>();
            ls.totalCount = size;
            ls.RefillCells();
        }
    }
}