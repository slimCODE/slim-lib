using System;
using System.Collections.Generic;
using System.Text;

namespace slimCODE.Validation
{
    public static partial class ValidationExtensions
    {
        public static Validation<T> Validate<T>(this T value, string name)
        {
            return new Validation<T>(name, value);
        }

        public static Validation<T> IsNotNull<T>(this Validation<T> validation)
            where T : class
        {
            if (validation.Value == null)
            {
                throw new ArgumentNullException(validation.Name);
            }

            return validation;
        }

        public static Validation<string> IsNotNullOrEmpty(this Validation<string> validation)
        {
            if (validation.Value == null)
            {
                throw new ArgumentNullException(validation.Name);
            }

            if (validation.Value.Length == 0)
            {
                throw new ArgumentException("String parameter cannot be empty.", validation.Name);
            }

            return validation;
        }
    }
}
