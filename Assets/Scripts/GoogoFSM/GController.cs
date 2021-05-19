using UnityEngine;

namespace GoogoFSM
{
    public class GController : MonoBehaviour
    {
        private GLayer[] layers = null; //HSM的层

        public static GController Create()
        {
            GController controller = new GController();
            return controller;
        }

        internal void Init()
        {

        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}