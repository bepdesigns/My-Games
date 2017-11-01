using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Invector;
using UnityEngine.Events;
using System;

namespace Invector.ItemManager
{
    public class vItemCollection : vTriggerGenericAction
    {
        [Header("--- Item Collection Options ---")]
        public float onCollectDelay;
        [Tooltip("Immediately equip the item ignoring the Equip animation")]
        public bool immediate = false;

        [Header("---Items List Data---")]
        public vItemListData itemListData;

        [Header("---Items Filter---")]      
        public List<vItemType> itemsFilter = new List<vItemType>() { 0 };

        [HideInInspector]
        public List<ItemReference> items = new List<ItemReference>();
      

        protected override void Start()
        {
            base.Start();
            if(destroyAfter && destroyDelay < onCollectDelay)
            {
                destroyDelay = onCollectDelay + 0.25f;
            }
        }

        public void CollectItems(vItemManager itemManager)
        {            
            if (items.Count > 0)
            {                
                StartCoroutine(SetItemsToItemManager(itemManager));
            }
        }       

        IEnumerator SetItemsToItemManager(vItemManager itemManager)
        {
            yield return new WaitForSeconds(onCollectDelay);

            itemManager.CollectItem(items, immediate);
            items.Clear();
        }
    }
}

