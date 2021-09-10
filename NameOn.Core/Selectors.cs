namespace NameOn.Core
{
    public static class Selectors
    {
        public static (T1, T2) MakeTuple<T1, T2>(T1 value1, T2 value2) => (value1, value2);
    }
}
