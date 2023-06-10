using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheMonkeMenu.Menu.Mods
{
    public class ModHelper : MonoBehaviour
    {
        public static ModHelper instance;



        void Start()
        {
            instance = this;
        }
    }
}
