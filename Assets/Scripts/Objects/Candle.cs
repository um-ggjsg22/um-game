
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Objects
{
    public class Candle:RoomObject, IClickable
    {
        [SerializeField] private CollidableBase fireEffect;
        [SerializeField] private Transform gameObjectParent;    // for spawning
        [SerializeField] private float cooldownDuration = 4;
        private bool OnCooldown = false;

        public IEnumerator Cooldown()
        {
            GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            OnCooldown = true;
            yield return new WaitForSeconds(cooldownDuration);
            GetComponent<Image>().color = Color.white;
            OnCooldown = false;
        }
        public void OnClick()
        {
            if (OnCooldown) return;
            StartCoroutine(Cooldown());
            //TODO some animation or sth
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    var coord = GetGridPosition() + new Vector2(i, j);
                    if (!GridManager.IsGridOccupied(null, coord))
                    {
                        var obj = Instantiate(fireEffect, gameObjectParent);
                        obj.gameObject.SetActive(true);
                        obj.SetPosition(coord);
                    }
                }
            }
        }
    }
}