using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ContainerBass<ItemType>
{
    protected List<object> items = new();

    public void Add<T>(T instance) where T : ItemType
    {
        if(items.Any(i => i is T || i is IEnumerable<T>))
        {
            throw new Exception("already registered");
        }

        items.Add(instance);
    }

    public void AddAll<T>(IEnumerable<T> instances) where T : ItemType
    {
        if(items.Any(i => i is T || i is IEnumerable<T>))
        {
            throw new Exception("already registered");
        }

        items.Add(instances);
    }

    public T Get<T>() where T : ItemType
    {
        var instance = items.Find(i => i is T);

        if(instance == null)
        {
            throw new Exception("is not found");
        }

        return (T)instance;
    }

    public IEnumerable<T> GetAll<T>() where T : ItemType
    {
        var instances = items.Find(i => i is IEnumerable<T>);

        if(instances == null)
        {
            throw new Exception("is not found");
        }

        return (IEnumerable<T>)instances;
    }

    public void Remove<T>() where T : ItemType
    {
        var instance = items.Find(i => i is T);
        
        if(instance == null)
        {
            throw new Exception("is not found");
        }

        items.Remove(instance);
    }

    public void RemoveAll<T>() where T : ItemType
    {
        var instances = items.Find(i => i is IEnumerable<T>);

        if(instances == null)
        {
            throw new Exception("is not found");
        }

        items.Remove(instances);
    }
}

public abstract class ContainerBass
{
    protected List<object> items = new();

    public void Add<T>(T instance)
    {
        if(items.Any(i => i is T || i is IEnumerable<T>))
        {
            throw new Exception("already registered");
        }

        items.Add(instance);
    }

    public void AddAll<T>(IEnumerable<T> instances)
    {
        if(items.Any(i => i is T || i is IEnumerable<T>))
        {
            throw new Exception("already registered");
        }

        items.Add(instances);
    }

    public T Get<T>()
    {
        var instance = items.Find(i => i is T);

        if(instance == null)
        {
            throw new Exception("is not found");
        }

        return (T)instance;
    }

    public IEnumerable<T> GetAll<T>()
    {
        var instances = items.Find(i => i is IEnumerable<T>);

        if(instances == null)
        {
            throw new Exception("is not found");
        }

        return (IEnumerable<T>)instances;
    }
}


public class FactoryContainer : ContainerBass<IFactory>{}
public class GameSpaceContainer : ContainerBass<MonoBehaviour>{}
public class DBContainer : ContainerBass<IDB>{}
public class GameMetaSpaceContainer : ContainerBass{}
public class UIContainer : ContainerBass{}

public interface IFactory{}
public interface IDB{}
