using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units
{
    public class ReferenceCamera : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        
        public Camera GameCamera;

        private void Start()
        {
            canvas.worldCamera = GameCamera;
        }
    }
}