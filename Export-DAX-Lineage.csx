using System.IO;

var outputPath = @"C:\Users\p.bachas-daunert\Downloads\DAX_Measures_Clean.json";

var lines = new List<string>();
lines.Add("{");
lines.Add("  \"exportDate\": \"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",");
lines.Add("  \"totalMeasures\": " + Model.AllMeasures.Count() + ",");
lines.Add("  \"measures\": [");

var measureCount = 0;
foreach (var m in Model.AllMeasures)
{
    if (measureCount > 0) lines.Add(",");

    // Count referenced measures
    var refCount = 0;
    var refMeasureNames = new List<string>();
    foreach (var otherMeasure in Model.AllMeasures)
    {
        if (m != otherMeasure && m.Expression.Contains("[" + otherMeasure.Name + "]"))
        {
            refCount++;
            refMeasureNames.Add(otherMeasure.Name);
        }
    }

    // Simple complexity
    var complexity = "Simple";
    if (refCount > 0) complexity = "Medium";
    if (refCount > 2) complexity = "Complex";
    if (refCount > 5) complexity = "Very Complex";

    // Clean strings - just replace problematic characters
    var cleanName = m.Name.Replace("\"", "'");
    var cleanExpr = m.Expression.Replace("\"", "'").Replace("\n", " ").Replace("\r", "");
    var cleanDesc = (m.Description ?? "").Replace("\"", "'");

    lines.Add("    {");
    lines.Add("      \"name\": \"" + cleanName + "\",");
    lines.Add("      \"table\": \"" + m.Table.Name + "\",");
    lines.Add("      \"expression\": \"" + cleanExpr + "\",");
    lines.Add("      \"description\": \"" + cleanDesc + "\",");
    lines.Add("      \"complexity\": \"" + complexity + "\",");
    lines.Add("      \"directDependencies\": " + refCount + ",");
    lines.Add("      \"isHidden\": " + m.IsHidden.ToString().ToLower() + ",");
    lines.Add("      \"referencedMeasures\": [");

    for (int i = 0; i < refMeasureNames.Count; i++)
    {
        lines.Add("        \"" + refMeasureNames[i] + "\"" + (i < refMeasureNames.Count - 1 ? "," : ""));
    }

    lines.Add("      ]");
    lines.Add("    }");

    measureCount++;
}

lines.Add("  ]");
lines.Add("}");

File.WriteAllLines(outputPath, lines);

Console.WriteLine("Exported " + Model.AllMeasures.Count() + " measures to: " + outputPath);