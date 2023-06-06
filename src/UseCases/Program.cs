using System.Text.Json;
using YiJingFramework.Annotating;
using YiJingFramework.PrimitiveTypes;

var store = new AnnotationStore() {
    Title = "Sample Store"
};
store.Tags.Add("QianKun");

var qian = new Gua(Enumerable.Repeat(Yinyang.Yang, 3));
var kun = new Gua(Enumerable.Repeat(Yinyang.Yin, 3));

var namingGroup = store.AddGroup("Naming", "Name");
namingGroup.AddEntry(qian.ToString(), "QIAN");
namingGroup.AddEntry(kun.ToString(), "KUN");

Console.WriteLine(store.SerializeToJsonString(new JsonSerializerOptions {
    WriteIndented = true
}));
Console.WriteLine();
// Output:
// {
//   "n": "Sample Store",
//   "t": [
//     "QianKun"
//   ],
//   "g": [
//     {
//       "t": "Naming",
//       "e": [
//         {
//           "t": "111",
//           "c": "QIAN"
//         },
//         {
//           "t": "000",
//           "c": "KUN"
//         }
//       ],
//       "c": "Name"
//     }
//   ]
// }