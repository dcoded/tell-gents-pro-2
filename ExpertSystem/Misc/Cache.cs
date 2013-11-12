namespace ExpertSystem
{
    class Cache<T>
    {
        private bool valid_ = false;
        private T value_ = default(T);

        public Cache() { }
        public Cache(T value) { value_ = value; valid_ = true; }

        public void Bad() { valid_ = false; }

        public static implicit operator Cache<T>(T value) { return new Cache<T>(value); }
        public static implicit operator T(Cache<T> o)     { return o.value_; }
        public static bool     operator !(Cache<T> o)     { return !o.valid_; }

    }
}
