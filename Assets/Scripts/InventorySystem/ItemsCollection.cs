using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace InventorySystem
{


    public partial class ItemsCollection : MonoBehaviour, ICollection<ItemEntry>
    {
        public Action onChange;
        [SerializeField] ItemEntry[] _itemEntries;
        public int Count => _itemEntries.Length;
        public bool RemoveWhenQuantityZero { get; set; } = true;
        public bool CanAdd(Item item, int quantity, int slot)   
        {
            if (_itemEntries[slot].item == null)   
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public void Add(ItemEntry e)
        {
            TryAdd(e);  
        }
        /// <returns>true - предмет был добавлен в количестве 1 или более штуки, false - предмет не добавлен</returns>
        public bool TryAdd(ItemEntry e)
        {
            return TryAdd(e.item, e.quantity);    
        }
        /// <returns>true - предмет был добавлен в количестве 1 или более штуки, false - предмет не добавлен</returns>
        public bool TryAdd(Item item, int quantity)         
        {
            int num = GetFree(item);
            if (num == -1)
            {
                return false;
            }
            _itemEntries[num] = new ItemEntry() { item = item, quantity = quantity };

            onChange?.Invoke();
            return true;
        }
        public bool SetItemAt(ItemEntry entry, int num)
        {
            if (_itemEntries[num].item != null)
            {
                return false;
            }
            _itemEntries[num] = entry;
            return true;
        }
        public void Remove(Item item)
        {
            int entryToRemove = Array.IndexOf(_itemEntries, Array.Find(_itemEntries, e => e.item == item));
            _itemEntries[entryToRemove] = null;

            onChange?.Invoke();
        }
        public void Remove(int itemIndex)
        {
            _itemEntries[itemIndex].item = null;

            onChange?.Invoke();
        }
        public bool Remove(ItemEntry itemEntry)
        {
            Remove(Array.IndexOf(_itemEntries, itemEntry));
            return true;
        }
        /// <returns>был ли предмет удален</returns>
        public bool Reduce(int itemIndex, int reduceAmount)
        {
            ItemEntry e = _itemEntries[itemIndex];
            int newQuantity = reduceAmount >= e.quantity ? 0 : e.quantity - reduceAmount;

            _itemEntries[itemIndex].quantity = newQuantity;
            onChange?.Invoke();
            if (RemoveWhenQuantityZero && newQuantity == 0)
            {
                Remove(e);
                return true;
            }
            return false;
        }
        public ItemEntry GetItemAt(int num)
        {
            if (num >= _itemEntries.Length)
            {
                return null;
            }
            return _itemEntries[num];
        }
        public ItemEntry Find(Item i)
        {
            return Array.Find(_itemEntries, e => e.item == i);
        }
        public int FindIndex(Item i)
        {
            return Array.IndexOf(_itemEntries, Array.Find(_itemEntries, e => e.item == i));
        }
        public int FindIndex(ItemEntry e)
        {
            return Array.IndexOf(_itemEntries, e);
        }
        public int GetFirstValidSlot(Item i, int quantity)
        {
            //HACK
            ItemEntry finded = Array.Find(_itemEntries, i => i.item == null);

            if (finded == null)
                return -1;
            else
                return Array.IndexOf(_itemEntries, finded);
        }
        public IEnumerator<ItemEntry> GetEnumerator()
        {
            return new ItemCollectionEnumerator(_itemEntries);
        }
        public void Clear()
        {
            Array.Clear(_itemEntries, 0, _itemEntries.Length);
            onChange?.Invoke();
        }
        public bool Contains(ItemEntry e)
        {
            return _itemEntries.Contains(e);
        }
        public void CopyTo(ItemEntry[] c, int index)
        {
            c = _itemEntries;
        }
        public bool IsReadOnly => false;
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public ItemEntry this[int index]
        {
            get => _itemEntries[index];
            set
            {
                if (index < _itemEntries.Length)
                {
                    _itemEntries[index] = value;
                    onChange?.Invoke();
                }
            }
        }
        int GetFree(Item item)
        {
            for (int i = 0; i < _itemEntries.Length; i++)
            {
                if (_itemEntries[i].item == null)
                {
                    return i;
                }
            }
            return -1;
        }
    }
    public class ItemCollectionEnumerator : IEnumerator<ItemEntry>
    {
        private ItemEntry[] _items;
        private int _position = -1;

        public ItemCollectionEnumerator(ItemEntry[] items)
        {
            _items = items;
        }

        public ItemEntry Current
        {
            get
            {
                if (_position < 0 || _position >= _items.Length)
                    throw new InvalidOperationException();
                return _items[_position];
            }
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _position++;
            return _position < _items.Length;
        }

        public void Reset()
        {
            _position = -1;
        }

        public void Dispose()
        {

        }
    }
}
