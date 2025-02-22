using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public partial class SlotCollectionBase : MonoBehaviour, ICollection<object>
{
    public event Action onChange;

    public int maxCount = 10;
    [SerializeField] object[] _itemEntries;
    public int Count => _itemEntries.Length;
    public bool RemoveWhenQuantityZero { get; set; } = true;
    public virtual bool CanAdd(object item, int quantity, int slot)
    {
        return _itemEntries[slot] == null;
    }
    public void Add(object e)
    {
        int index = GetFree(e);
        _itemEntries[index] = e;
    }
    public void SetItemAt(object entry, int num)
    {
        _itemEntries[num] = entry;
    }
    public void Remove(int itemIndex)
    {
        _itemEntries[itemIndex] = null;

        onChange?.Invoke();
    }
    public bool Remove(object itemEntry)
    {
        Remove(Array.IndexOf(_itemEntries, itemEntry));
        return true;
    }
    public object GetItemAt(int num)
    {
        if (num >= _itemEntries.Length)
        {
            return null;
        }
        return _itemEntries[num];
    }
    public int FindIndex(object e)
    {
        return Array.IndexOf(_itemEntries, e);
    }
    public int GetFirstValidSlot(object i, int quantity)
    {
        //HACK
        object finded = Array.Find(_itemEntries, i => i == null);

        if (finded == null)
            return -1;
        else
            return Array.IndexOf(_itemEntries, finded);
    }
    public IEnumerator<object> GetEnumerator()
    {
        return new ItemCollectionEnumerator(_itemEntries);
    }
    public void Clear()
    {
        Array.Clear(_itemEntries, 0, _itemEntries.Length);
        onChange?.Invoke();
    }
    public bool Contains(object e)
    {
        return _itemEntries.Contains(e);
    }
    public void CopyTo(object[] c, int index)
    {
        c = _itemEntries;
    }
    public bool IsReadOnly => false;
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public object this[int index]
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
    int GetFree(object item)
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (_itemEntries[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
}
public class ItemCollectionEnumerator : IEnumerator<object>
{
    private object[] _items;
    private int _position = -1;

    public ItemCollectionEnumerator(object[] items)
    {
        _items = items;
    }

    public object Current
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

