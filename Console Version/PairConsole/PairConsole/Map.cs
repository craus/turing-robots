using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using System.Linq;

public class Map<K, V> : Dictionary<K, V>
{
    public Func<V, bool> removeDefaultValues = v => false;
    private Func<V> defaultValueProvider = null;
		
    public new V this[K key] {
        get {
            if (!ContainsKey(key)) {
                return this[key] = GetDefaultValue();
            }
            return base[key];
        }
        set {
            if (removeDefaultValues(value)) {
                base.Remove(key);
                return;
            }
            base[key] = value;
        }
    }

    private V GetDefaultValue() {
        if (defaultValueProvider == null) {
            return default(V);
        } else {
            return defaultValueProvider();
        }
    }

    public Map(Func<V> defaultValueProvider = null) {
        this.defaultValueProvider = defaultValueProvider;
    }

	public static Dictionary<K, V> Merge(IEnumerable<Map<K, V>> maps) {
		return maps.SelectMany(dict => dict)
							 .ToLookup(pair => pair.Key, pair => pair.Value)
							 .ToDictionary(group => group.Key, group => group.First());
	}

	public static Dictionary<K, V> Merge(params Map<K, V>[] maps) {
		return maps.SelectMany(dict => dict)
							 .ToLookup(pair => pair.Key, pair => pair.Value)
				   .ToDictionary(group => group.Key, group => group.First());
	}

	public static Map<K, V> FromDictionary(Dictionary<K, V> d) {
		var result = new Map<K, V>();
		d.Keys.ToList().ForEach(k => result[k] = d[k]);
		return result;
	}

	public Map<K, V> Merge(Map<K, V> other) {
		return FromDictionary(Merge(this, other));
	}

    public override string ToString() {
        string result = "";
        foreach (K key in Keys) {
            if (result != "") {
                result += "\n";
            }
            string value = this[key] != null ? this[key].ToString() : "null";
            result += key.ToString() + ": " + value;
        }
        return result;
    }
}