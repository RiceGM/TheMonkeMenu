using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheMonkeMenu.Menu.Mods.Platforms
{
    public class PlatformObject : MonoBehaviour
    {
        public bool isUnEquipped = false;

        void FixedUpdate()
        {
            if(isUnEquipped)
            {
                transform.localScale -= new Vector3(0.025f, 0.025f, 0.025f);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (!isUnEquipped) return;

            GameObject.Destroy(gameObject);
        }
    }
}
