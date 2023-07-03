
using System.Runtime.Serialization;
using AutoRegister;

namespace AutoRegisterTest.Services
{
    [Scoped]
    public class ClassWithJustISerializable : ISerializable
    {
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}