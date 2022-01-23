
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Objects
{
    public class LegoBox: Furniture, IClickable
    {
        [SerializeField] private CollidableBase legos;
        [SerializeField] private Transform gameObjectParent;    // for spawning
        [SerializeField] private float cooldownDuration = 4;
        [SerializeField] private Sprite toppledSprite;
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
            GridMap.Instance.RemoveRoomObjectFromOccupancyGrid(this);
            var obj = Instantiate(legos, gameObjectParent);
            obj.gameObject.SetActive(true);
            obj.SetPosition(this.GetGridPosition());
            Destroy(gameObject);
            // // Swap to brokenSprite
            // Image imageComponent = GetComponent<Image>();
            // imageComponent.sprite = toppledSprite;
            // for (var i = 0; i < 6; i++)
            // {
            //     var x = (int)Math.Floor((double)Random.Range(-1, 2));
            //     var y = (int)Math.Floor((double)Random.Range(-1, 2));
            //     var coord = GetGridPosition() + new Vector2(x, y);
            //     if (!GridManager.IsGridOccupied(coord))
            //     {
            //         var obj = Instantiate(legos, gameObjectParent);
            //         obj.gameObject.SetActive(true);
            //         obj.SetPosition(coord);
            //     }
            // }
        }
    }
}