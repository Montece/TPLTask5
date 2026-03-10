using Moq;
using Xunit;

namespace TPLTask4.Tests;

public class CatalogTests
{
    [Fact]
    public void Add_ShouldPutNewItemToHead()
    {
        var logger = new Mock<Action<string>>();
        var catalog = new Catalog(logger.Object);

        catalog.AddToHead("B");
        catalog.AddToHead("A");

        catalog.Show();

        logger.Verify(x => x("A"), Times.Once);
        logger.Verify(x => x("B"), Times.Once);

        var sequence = logger.Invocations.Select(i => i.Arguments.First().ToString()).ToArray();
        Assert.Equal(new[] { "A", "B" }, sequence);
    }

    [Fact]
    public void Sort_ShouldSortItemsAscendingIgnoreCase()
    {
        var logger = new Mock<Action<string>>();
        var catalog = new Catalog(logger.Object);

        catalog.AddToHead("delta");
        catalog.AddToHead("Bravo");
        catalog.AddToHead("alpha");

        catalog.Sort();
        catalog.Show();

        var logged = logger.Invocations.Select(i => i.Arguments.First().ToString()).ToArray();
        Assert.Equal(new[] { "alpha", "Bravo", "delta" }, logged);
    }

    [Fact]
    public void Add_Null_ShouldThrow()
    {
        var catalog = new Catalog(_ => { });

        Assert.Throws<ArgumentNullException>(() => catalog.AddToHead(null!));
    }

    [Fact]
    public void Show_AfterDispose_ShouldThrow()
    {
        var catalog = new Catalog(_ => { });
        catalog.AddToHead("x");
        catalog.Dispose();

        Assert.Throws<ObjectDisposedException>(catalog.Show);
    }

    [Fact]
    public void Sort_EmptyCatalog_ShouldNotThrow()
    {
        var catalog = new Catalog(_ => { });

        catalog.Sort();
    }

    [Fact]
    public void Sort_EmptyCatalog_ShouldNotCallLogger()
    {
        var logger = new Mock<Action<string>>();
        var catalog = new Catalog(logger.Object);

        catalog.Sort();
        catalog.Show();

        logger.Verify(x => x(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Sort_OneItem_ShouldKeepSingleItem()
    {
        var logger = new Mock<Action<string>>();
        var catalog = new Catalog(logger.Object);

        catalog.AddToHead("only");

        catalog.Sort();
        catalog.Show();

        var logged = logger.Invocations.Select(i => i.Arguments.First().ToString()).ToArray();

        Assert.Single(logged);
        Assert.Equal("only", logged.First());
    }

    [Fact]
    public void Sort_TwoItems_AlreadySorted_ShouldKeepOrder()
    {
        var logger = new Mock<Action<string>>();
        var catalog = new Catalog(logger.Object);

        catalog.AddToHead("alpha");
        catalog.AddToHead("bravo");

        catalog.Sort();
        catalog.Show();

        var logged = logger.Invocations.Select(i => i.Arguments.First().ToString()).ToArray();

        Assert.Equal(new[] { "alpha", "bravo" }, logged);
    }

    [Fact]
    public void Sort_TwoItems_Reversed_ShouldSwapToAscending()
    {
        var logger = new Mock<Action<string>>();
        var catalog = new Catalog(logger.Object);

        catalog.AddToHead("bravo");
        catalog.AddToHead("alpha");

        catalog.Sort();
        catalog.Show();

        var logged = logger.Invocations.Select(i => i.Arguments.First().ToString()).ToArray();
        Assert.Equal(new[] { "alpha", "bravo" }, logged);
    }

    [Fact]
    public void Sort_TwoItems_DifferentCasing_ShouldSortIgnoreCase()
    {
        var logger = new Mock<Action<string>>();
        var catalog = new Catalog(logger.Object);

        catalog.AddToHead("Bravo");
        catalog.AddToHead("alpha");

        catalog.Sort();
        catalog.Show();

        var logged = logger.Invocations.Select(i => i.Arguments.First().ToString()).ToArray();
        Assert.Equal(new[] { "alpha", "Bravo" }, logged);
    }

    [Fact]
    public void Sort_ItemsWithSameValue_ShouldNotLoseOrDuplicateItems()
    {
        var logger = new Mock<Action<string>>();
        var catalog = new Catalog(logger.Object);

        catalog.AddToHead("same");
        catalog.AddToHead("same");
        catalog.AddToHead("same");

        catalog.Sort();
        catalog.Show();

        var logged = logger.Invocations.Select(i => i.Arguments.First().ToString()).ToArray();

        Assert.Equal(3, logged.Length);
        Assert.All(logged, v => Assert.Equal("same", v));
    }

    [Fact]
    public void Sort_ItemsWithSameValueDifferentCase_ShouldTreatThemAsEqual()
    {
        var logger = new Mock<Action<string>>();
        var catalog = new Catalog(logger.Object);

        catalog.AddToHead("Alpha");
        catalog.AddToHead("alpha");
        catalog.AddToHead("ALPHA");

        catalog.Sort();
        catalog.Show();

        var logged = logger.Invocations.Select(i => i.Arguments.First().ToString()).ToArray();

        Assert.Equal(3, logged.Length);
        Assert.Contains("Alpha", logged);
        Assert.Contains("alpha", logged);
        Assert.Contains("ALPHA", logged);
    }

    [Fact]
    public void Sort_AlreadySortedManyItems_ShouldKeepOrder()
    {
        var logger = new Mock<Action<string>>();
        var catalog = new Catalog(logger.Object);

        catalog.AddToHead("delta");
        catalog.AddToHead("charlie");
        catalog.AddToHead("bravo");
        catalog.AddToHead("alpha");

        catalog.Sort();
        catalog.Show();

        var logged = logger.Invocations.Select(i => i.Arguments.First().ToString()).ToArray();

        Assert.Equal(new[] { "alpha", "bravo", "charlie", "delta" }, logged);
    }

    [Fact]
    public void Sort_ReverseSortedManyItems_ShouldBeSortedAscending()
    {
        var logger = new Mock<Action<string>>();
        var catalog = new Catalog(logger.Object);

        catalog.AddToHead("alpha");
        catalog.AddToHead("bravo");
        catalog.AddToHead("charlie");
        catalog.AddToHead("delta");

        catalog.Sort();
        catalog.Show();

        var logged = logger.Invocations.Select(i => i.Arguments.First().ToString()).ToArray();

        Assert.Equal(new[] { "alpha", "bravo", "charlie", "delta" }, logged);
    }

    [Fact]
    public void Sort_MixedOrderWithDuplicates_ShouldSortAndPreserveCount()
    {
        var logger = new Mock<Action<string>>();
        var catalog = new Catalog(logger.Object);

        catalog.AddToHead("beta");
        catalog.AddToHead("alpha");
        catalog.AddToHead("beta");
        catalog.AddToHead("gamma");

        catalog.Sort();
        catalog.Show();

        var logged = logger.Invocations.Select(i => i.Arguments.First().ToString()).ToArray();

        Assert.Equal(4, logged.Length);
        Assert.Equal(new[] { "alpha", "beta", "beta", "gamma" }, logged);
    }

    [Fact]
    public void Dispose_ShouldDisposeAllElements()
    {
        var catalog = new Catalog(_ => { });
        catalog.AddToHead("c");
        catalog.AddToHead("b");
        catalog.AddToHead("a");

        catalog.Dispose();

        Assert.Throws<ObjectDisposedException>(() => catalog.AddToHead("zzz"));
    }
}