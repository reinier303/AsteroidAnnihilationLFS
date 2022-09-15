using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace AsteroidAnnihilation
{
    public class CameraOffset : MonoBehaviour
    {
        public static CameraOffset Instance;

        public Camera cam;
        private CinemachineCameraOffset offset;
        public float OffsetIntensity = 0.1f;
        public float Damping = 0.1f;
        public GameManager gameManager;
        public Vector2 Offset;

        //Confine Values
        public bool EdgeConfinerEnabled;
        [HideInInspector] public bool IsConfing;
        public Vector2 ScreenEdgeValues;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            offset = GetComponent<CinemachineCameraOffset>();
            Offset = Vector2.zero;
            IsConfing = false;

            gameManager = GameManager.Instance;
        }
        // Update is called once per frame
        private void Update()
        {
            if(Time.timeScale == 0)
            {
                return;
            }

            if(EdgeConfinerEnabled)
            {
                //intensity = 1.35f, damping = 0.08f/
                ConfineEdges();
            }

            if (IsConfing)
            {
                return;
            }
            MouseOffset();
        }

        private void ConfineEdges()
        {
            if(transform.localPosition.x > ScreenEdgeValues.x)
            {
                Offset.x = ScreenEdgeValues.x - transform.localPosition.x;
            }
            if (transform.localPosition.x < -ScreenEdgeValues.x)
            {
                Offset.x = -ScreenEdgeValues.x - transform.localPosition.x;
            }
            if (transform.localPosition.y > ScreenEdgeValues.y)
            {
                Offset.y = ScreenEdgeValues.y - transform.localPosition.y;
            }
            if (transform.localPosition.y < -ScreenEdgeValues.y)
            {
                Offset.y = -ScreenEdgeValues.y - transform.localPosition.y;
            }
            if(Offset.x != 0 || Offset.y != 0)
            {
                IsConfing = true;
            }
            else
            {
                IsConfing = false;
            }
            offset.m_Offset = Offset;
        }

        private void MouseOffset()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            Vector2 target = hit.point;

            Vector2 OffsetTwo = ((hit.point - transform.position) / 2) * OffsetIntensity;
            Vector3 finalOffset = new Vector3(OffsetTwo.x, OffsetTwo.y, 0);

            Vector3 SmoothedOffset = Vector3.Lerp(offset.m_Offset, finalOffset, Damping);

            offset.m_Offset = SmoothedOffset;
        }
    }
}