using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace TwinTower
{
    public class ArrowColliderManager : Manager<ArrowColliderManager>
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            Destroy(other.gameObject);
        }
    }
}

