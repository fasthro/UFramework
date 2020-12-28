using UnityEngine;

namespace UFramework.Lockstep
{
    public enum ViewType
    {
        Player,
        EnemyPlayer,
    }

    public class ViewObject : MonoBehaviour
    {
        public ViewData viewData;
    }
}