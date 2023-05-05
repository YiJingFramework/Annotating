using System.Text.Json;
using YiJingFramework.Annotating;
using YiJingFramework.Annotating.Entities;
using YiJingFramework.PrimitiveTypes;

var store = new AnnotationStore() {
    Title = "Sample Store"
};
store.Tags.Add("QianKun");

var qian = new Gua(Enumerable.Repeat(Yinyang.Yang, 3));
var kun = new Gua(Enumerable.Repeat(Yinyang.Yin, 3));

var namingGroup = store.AddPaintingGroup("Naming", "Name");
namingGroup.AddEntry(qian, "QIAN");
namingGroup.AddEntry(kun, "KUN");

var linesGroup = store.AddPaintingLinesGroup("Line");
linesGroup.AddEntry(new PaintingLines(qian, 0), "Q1");
linesGroup.AddEntry(new PaintingLines(qian, 1), "Q2");
linesGroup.AddEntry(new PaintingLines(qian, 2), "Q3");
linesGroup.AddEntry(new PaintingLines(kun, 0), "K1");
linesGroup.AddEntry(new PaintingLines(kun, 1), "K2");
linesGroup.AddEntry(new PaintingLines(kun, 2), "K3");

Console.WriteLine(store.SerializeToJsonString(new JsonSerializerOptions {
    WriteIndented = true
}));

/*
 * Output:
 * {
 *   "n": "Sample Store",
 *   "t": [
 *     "QianKun"
 *   ],
 *   "gp": [
 *     {
 *       "t": "Naming",
 *       "e": [
 *         {
 *           "t": "111",
 *           "c": "QIAN"
 *         },
 *         {
 *           "t": "000",
 *           "c": "KUN"
 *         }
 *       ],
 *       "c": "Name"
 *     }
 *   ],
 *   "gl": [
 *     {
 *       "t": "Line",
 *       "e": [
 *         {
 *           "t": "111100",
 *           "c": "Q1"
 *         },
 *         {
 *           "t": "111010",
 *           "c": "Q2"
 *         },
 *         {
 *           "t": "111001",
 *           "c": "Q3"
 *         },
 *         {
 *           "t": "000100",
 *           "c": "K1"
 *         },
 *         {
 *           "t": "000010",
 *           "c": "K2"
 *         },
 *         {
 *           "t": "000001",
 *           "c": "K3"
 *         }
 *       ]
 *     }
 *   ]
 * }
 */