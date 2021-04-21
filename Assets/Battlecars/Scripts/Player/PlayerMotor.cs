using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battlecars.Player
{
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;

        public bool isSetup = false;

        public void Setup()
        {
            isSetup = true;
        }

        private void Update()
        {
            if (!isSetup) return;

            transform.position += transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
            transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        }
    }
}