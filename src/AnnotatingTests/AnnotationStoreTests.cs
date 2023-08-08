using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YiJingFramework.Annotating.Tests;

[TestClass()]
public class AnnotationStoreTests
{
    [TestMethod()]
    public void AnnotationStoreTest()
    {
        var store = new AnnotationStore()
        {
            Title = "Sample Store"
        };

        store.Tags.Add("Tag1");
        store.Tags.Add("Tag2");
        store.Tags.Add("Tag3");

        var namingGroup = store.AddGroup(title: "Gua Name", comment: "Names of the Guas");
        namingGroup.AddEntry("111", "Qian");
        namingGroup.AddEntry("000", "KKK");
        var entry = namingGroup.GetEntry("000");
        Assert.IsNotNull(entry);
        entry.Content = "Kun";

        var serialized = store.SerializeToJsonString();
        Assert.AreEqual(
            "{\"n\":\"Sample Store\",\"t\":[\"Tag1\",\"Tag2\",\"Tag3\"]" +
            ",\"g\":[{\"t\":\"Gua Name\",\"e\":[{\"t\":\"111\",\"c\":\"Qian\"}" +
            ",{\"t\":\"000\",\"c\":\"Kun\"}],\"c\":\"Names of the Guas\"}]}",
            serialized);

        var d = AnnotationStore.DeserializeFromJsonString(serialized);
        var serialized2 = d?.SerializeToJsonString();
        Assert.AreEqual(serialized, serialized2);
    }
}