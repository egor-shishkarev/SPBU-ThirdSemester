using Reflection;

var refl = new Reflector("//");

refl.PrintStructure(typeof(SomeClass));

public static class SomeClass
{
    private static int size = 1;

    public static string name = "False";

    public static void Print()
    {
        Console.WriteLine($"{name} - {size}");
    }

    static class UnderClass
    {
        private static int size;
    }
}
