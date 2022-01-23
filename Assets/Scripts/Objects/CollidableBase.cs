using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class CollidableBase: MonoBehaviour
    {
        public Vector2 _position;
        [SerializeField] private float timeToLive;
        [SerializeField] private Runner runner;
        private void Start()
        {
        }

        private IEnumerator Expire()
        {
            yield return new WaitForSeconds(timeToLive);
            Destroy(gameObject);
        }

        private void Update()
        {
            if (GridManager.GetRunnerPosition() == _position)
            {
                StartCoroutine(runner.Stun(2));
                Destroy(gameObject);
            }
        }

        public void SetPosition(Vector2 coord)
        {
            this._position = coord;
            transform.position = GridMap.Instance.GetPositionCoordinate((int)coord.x, (int)coord.y);
            StartCoroutine(Expire());
        }
    }
}