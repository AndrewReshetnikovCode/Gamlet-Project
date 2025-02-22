using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterRow
{
    public ColumnValue<Spawner> spawner = new();
    public ColumnValue<bool> isAlive = new();
    public ColumnValue<GameObject> gameObject = new();
}
public class ColumnValue<T>
{
    public event Action<T,CharacterRow> onChange;
    public CharacterRow row;
    T _v;
    public T Value { get => _v; set{ _v = value ; onChange?.Invoke(_v,row); } }
}