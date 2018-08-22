using System.Runtime.Serialization;
using UnityEngine;

sealed class RaySerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context) {
        Ray ray = (Ray)obj;
        info.AddValue("origin", ray.origin);
        info.AddValue("direction", ray.direction);
    }

    public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {
        Ray ray = (Ray)obj;
        ray.origin = (Vector3)info.GetValue("origin", typeof(Vector3));
        ray.direction = (Vector3)info.GetValue("direction", typeof(Vector3));
        obj = ray;
        return obj;
    }
}
