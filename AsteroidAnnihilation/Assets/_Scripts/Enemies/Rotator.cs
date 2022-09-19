using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class Rotator : MonoBehaviour
    {
        private float rotationSpeed;

        public void SetRotationSpeed(float speed)
        {
            rotationSpeed = speed;
        }

        public float GetRotationSpeed()
        {
            return rotationSpeed;
        }

        private void Update()
        {
            transform.Rotate(0,0,rotationSpeed * Time.deltaTime);
        }
    }
}
