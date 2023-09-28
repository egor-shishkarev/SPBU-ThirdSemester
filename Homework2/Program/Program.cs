using Lazy;

static int func(int x)
{
    return x + 7;
}

LazyOneThread<int> lazy = new(() => func(3));

lazy.Get();
lazy.Get();