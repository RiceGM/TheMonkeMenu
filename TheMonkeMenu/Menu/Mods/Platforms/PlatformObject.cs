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
        bool hasInvokedSelfDeath = false;

        void FixedUpdate()
        {
            if(isUnEquipped)
            {
                if (!hasInvokedSelfDeath) Invoke("SelfDeath", 10f);
                transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f) * Time.fixedDeltaTime * 5f;
                if (transform.localScale.x < 0) GameObject.Destroy(gameObject);
            }
        }

        void SelfDeath()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
