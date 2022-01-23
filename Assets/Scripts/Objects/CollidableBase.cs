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

        [SerializeField] private float effectDuration = 2f;

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
                runner.Stun(effectDuration);
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