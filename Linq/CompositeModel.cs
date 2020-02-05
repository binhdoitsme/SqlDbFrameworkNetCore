using Newtonsoft.Json.Linq;
using static Newtonsoft.Json.JsonConvert;

namespace SqlDbFrameworkNetCore.Linq
{
    public class CompositeModel<T1, T2>
    {
        protected readonly string SerializedObject;
        public T1 First { get; set; }
        public T2 Second { get; set; }
        public CompositeModel(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }

        internal CompositeModel(JObject obj)
        {
            SerializedObject = SerializeObject(obj);
            First = DeserializeObject<T1>(SerializedObject);
            Second = DeserializeObject<T2>(SerializedObject);
        }
    }

    public class CompositeModel<T1, T2, T3> : CompositeModel<T1, T2>
    {
        public T3 Third { get; set; }
        public CompositeModel(T1 first, T2 second, T3 third) : base(first, second)
        {
            Third = third;
        }

        internal CompositeModel(JObject obj) : base(obj)
        {
            Third = DeserializeObject<T3>(SerializedObject);
        }
    }

    public class CompositeModel<T1, T2, T3, T4> : CompositeModel<T1, T2, T3>
    {
        public T4 Fourth { get; set; }
        public CompositeModel(T1 first, T2 second, T3 third, T4 fourth) : base(first, second, third)
        {
            Fourth = fourth;
        }

        internal CompositeModel(JObject obj) : base(obj)
        {
            Fourth = DeserializeObject<T4>(SerializedObject);
        }
    }

    public class CompositeModel<T1, T2, T3, T4, T5> : CompositeModel<T1, T2, T3, T4>
    {
        public T5 Fifth { get; set; }
        public CompositeModel(T1 first, T2 second, T3 third, T4 fourth, T5 fifth) 
            : base(first, second, third, fourth)
        {
            Fifth = fifth;
        }

        internal CompositeModel(JObject obj) : base(obj)
        {
            Fifth = DeserializeObject<T5>(SerializedObject);
        }
    }

    public class CompositeModel<T1, T2, T3, T4, T5, T6> : CompositeModel<T1, T2, T3, T4, T5>
    {
        public T6 Sixth { get; set; }
        public CompositeModel(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth)
            : base(first, second, third, fourth, fifth)
        {
            Sixth = sixth;
        }

        internal CompositeModel(JObject obj) : base(obj)
        {
            Sixth = DeserializeObject<T6>(SerializedObject);
        }
    }

    public class CompositeModel<T1, T2, T3, T4, T5, T6, T7> : CompositeModel<T1, T2, T3, T4, T5, T6>
    {
        public T7 Seventh { get; set; }
        public CompositeModel(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh)
            : base(first, second, third, fourth, fifth, sixth)
        {
            Seventh = seventh;
        }

        internal CompositeModel(JObject obj) : base(obj)
        {
            Seventh = DeserializeObject<T7>(SerializedObject);
        }
    }

    public class CompositeModel<T1, T2, T3, T4, T5, T6, T7, T8> : CompositeModel<T1, T2, T3, T4, T5, T6, T7>
    {
        public T8 Eighth { get; set; }
        public CompositeModel(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth)
            : base(first, second, third, fourth, fifth, sixth, seventh)
        {
            Eighth = eighth;
        }

        internal CompositeModel(JObject obj) : base(obj)
        {
            Eighth = DeserializeObject<T8>(SerializedObject);
        }
    }
}
