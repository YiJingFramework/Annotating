using System.Text.Json;
using YiJingFramework.Annotating;
using YiJingFramework.References.Zhouyi;

var annotationFile = new AnnotationStore();

var indexing = annotationFile.AddGroup("卦序");
var naming = annotationFile.AddGroup("卦名");
var text = annotationFile.AddGroup("卦辞");
var lineText = annotationFile.AddGroup("爻辞");
var yong = annotationFile.AddGroup("用辞");

var zhouyi = new Zhouyi("./jing.json");
for (int i = 1; i <= 64; i++)
{
    var hexagram = zhouyi.GetHexagram(i);
    var painting = hexagram.GetPainting();
    _ = indexing.AddEntryTargetsPainting(painting, hexagram.Index.ToString());
    _ = naming.AddEntryTargetsPainting(painting, hexagram.Name);
    _ = text.AddEntryTargetsPainting(painting, hexagram.Text);
    for (int j = 0; j < 6; j++)
    {
        _ = lineText.AddEntryTargetsLine(painting, j, hexagram.GetLine(j + 1).LineText);
    }
    if(hexagram.ApplyNinesOrApplySixes is not null)
    {
        _ = yong.AddEntryTargetsPainting(painting, hexagram.ApplyNinesOrApplySixes.LineText);
    }
}

File.WriteAllText("out.json", JsonSerializer.Serialize(annotationFile, new JsonSerializerOptions {
     Encoder=System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
}));