using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// A generic circular buffer implementation, holding a constant number of items.
/// 
/// buffer[0] = most recently added item
/// buffer[1] = second most recent item
/// etc.
/// 
/// Number of items is constant from initialisation, even if none have been explicitly added yet
/// (use the two-argument constructor to set the initial item value).
/// 
/// IsReadOnly = false, which means that Add(), Remove() and Clear() are allowed to throw NotSupportedException.
/// Add() is allowed anyways by this specialisation - I believe this still complies with the interface & the substitution priciple
/// (see learn.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1)
/// 
/// </summary>
/// <typeparam name="T"> the type contained in the buffer </typeparam>
public class CircularBuffer<T>: IReadOnlyList<T>, ICollection<T>
{
    private T[] values;
    private int head = 0; // head points just past the most recently added element (i.e. where the next instertion will be)

    public CircularBuffer() : this(0) { }

    public CircularBuffer(int count)
    {
        values = new T[count];
    }

    public CircularBuffer(int count, T initial) : this(count)
    {
        Array.Fill(values, initial);
    }

    // indices are relative to the head, reversed, and wrap around the array bounds
    // remember that head points one item past the most recently inserted one
    public T this[int index] => values[Math.Sign(head-index-1) * (head-index-1) % values.Length];

    public int Count => values.Length;

    // IsReadOnly refers to the ability to add & remove elements, not to modify each element
    // CircularBuffer does not support removal, so it does not comply with IsReadOnly = false
    public bool IsReadOnly => true;

    // Add() is supported by CircularBuffer despite the fact that ICollection<T> does not require it when IsReadOnly = true
    public void Add(T item)
    {
        // intentional post-increment
        values[head++] = item;
        head %= values.Length;
    }

    public void Clear()
    {
        throw new NotSupportedException("CircularBuffer is read-only with the exception of Add()");
    }

    public void Clear(T value)
    {
        Array.Fill(values, value);
    }

    public bool Contains(T item)
    {
        return ((ICollection<T>)values).Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        values.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return (IEnumerator<T>) values.GetEnumerator();
    }

    public bool Remove(T item)
    {
        throw new NotSupportedException("CircularBuffer is read-only with the exception of Add()");
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return values.GetEnumerator();
    }
}
