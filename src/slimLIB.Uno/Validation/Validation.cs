using System;
using System.Collections.Generic;
using System.Text;

namespace slimCODE.Validation
{
    public class Validation<T>
    {
        internal Validation(string name, T value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; private set; }
        public T Value { get; private set; }

        public static implicit operator T(Validation<T> validation)
        {
            return validation.Value;
        }
    }
}
