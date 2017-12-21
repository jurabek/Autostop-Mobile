using System;
using System.Collections.Generic;

namespace Google.Maps.Internal
{
    public class ValueTextComparer : IComparer<ValueText>
    {
        private readonly StringComparer _stringComparer;

        public ValueTextComparer(StringComparer stringComparer)
        {
            if (stringComparer == null) throw new ArgumentNullException("stringComparer");
            _stringComparer = stringComparer;
        }

        public int Compare(ValueText x, ValueText y)
        {
            if (x == null) return -1;
            if (y == null) return 1;

            int test;

            test = _stringComparer.Compare(x.Text, y.Text);
            if (test != 0) return test;

            if (x.Value < y.Value) return -1;
            if (x.Value > y.Value) return 1;

            //i guess they're the same.
            return 0;
        }
    }
}