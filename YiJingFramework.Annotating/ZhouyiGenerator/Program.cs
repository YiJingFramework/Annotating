using System.Text.Json;
using YiJingFramework.Annotating;
using YiJingFramework.References.Zhouyi;

var store = new AnnotationStore() {
    Title = "Zhouyi"
};
store.Tags.Add("TEST");

var indexing = store.AddPaintingGroup("卦序", "TEST");
var naming = store.AddPaintingGroup("卦名");
var text = store.AddPaintingGroup("卦辞");
var lineText = store.AddPaintingLinesGroup("爻辞");
var yong = store.AddPaintingGroup("用辞");

var zhouyi = new Zhouyi("./jing.json");
for (int i = 1; i <= 64; i++)
{
    var hexagram = zhouyi.GetHexagram(i);
    var painting = hexagram.GetPainting();
    _ = indexing.AddEntry(painting, hexagram.Index.ToString());
    _ = naming.AddEntry(painting, hexagram.Name);
    _ = text.AddEntry(painting, hexagram.Text);
    for (int j = 0; j < 6; j++)
    {
        _ = lineText.AddEntry(new(painting, j), hexagram.GetLine(j + 1).LineText);
    }
    if(hexagram.ApplyNinesOrApplySixes is not null)
    {
        _ = yong.AddEntry(painting, hexagram.ApplyNinesOrApplySixes.LineText);
    }
}

var s = store.SerializeToJsonString(new JsonSerializerOptions {
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    WriteIndented = true
});
Console.WriteLine(s);

var d = AnnotationStore.DeserializeFromJsonString(s);
Console.WriteLine("Done");