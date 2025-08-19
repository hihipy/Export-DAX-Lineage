using System.IO;
var outputPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "Downloads", "DAX_Measures_Clean.json");
var lines = new List<string>();
lines.Add("{");
lines.Add("  \"exportDate\": \"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",");
lines.Add("  \"totalMeasures\": " + Model.AllMeasures.Count() + ",");

// Count calculated columns
var calculatedColumnsCount = 0;
foreach (var table in Model.Tables)
{
    calculatedColumnsCount += table.Columns.Where(c => c.Type == ColumnType.Calculated).Count();
}
lines.Add("  \"totalCalculatedColumns\": " + calculatedColumnsCount + ",");

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
    // Parse folder structure
    var folderPath = m.DisplayFolder ?? "";
    var folderParts = folderPath.Split('\\');
    var mainFolder = folderParts.Length > 0 ? folderParts[0] : "";
    var subFolder = folderParts.Length > 1 ? folderParts[1] : "";
    var fullFolderPath = folderPath;
    // Clean strings - just replace problematic characters
    var cleanName = m.Name.Replace("\"", "'");
    var cleanExpr = m.Expression.Replace("\"", "'").Replace("\n", " ").Replace("\r", "");
    var cleanDesc = (m.Description ?? "").Replace("\"", "'");
    var cleanMainFolder = mainFolder.Replace("\"", "'");
    var cleanSubFolder = subFolder.Replace("\"", "'");
    var cleanFullPath = fullFolderPath.Replace("\"", "'");
    lines.Add("    {");
    lines.Add("      \"name\": \"" + cleanName + "\",");
    lines.Add("      \"table\": \"" + m.Table.Name + "\",");
    lines.Add("      \"expression\": \"" + cleanExpr + "\",");
    lines.Add("      \"description\": \"" + cleanDesc + "\",");
    lines.Add("      \"folder\": \"" + cleanMainFolder + "\",");
    lines.Add("      \"subfolder\": \"" + cleanSubFolder + "\",");
    lines.Add("      \"fullFolderPath\": \"" + cleanFullPath + "\",");
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
lines.Add("  ],");

// Add calculated columns section
lines.Add("  \"calculatedColumns\": [");
var columnCount = 0;
foreach (var table in Model.Tables)
{
    foreach (var column in table.Columns.Where(c => c.Type == ColumnType.Calculated))
    {
        if (columnCount > 0) lines.Add(",");
        
        // Parse folder structure for columns (if they have display folders)
        var columnFolderPath = column.DisplayFolder ?? "";
        var columnFolderParts = columnFolderPath.Split('\\');
        var columnMainFolder = columnFolderParts.Length > 0 ? columnFolderParts[0] : "";
        var columnSubFolder = columnFolderParts.Length > 1 ? columnFolderParts[1] : "";
        var columnFullFolderPath = columnFolderPath;
        
        // Clean strings for columns
        var cleanColumnName = column.Name.Replace("\"", "'");
        var cleanColumnDesc = (column.Description ?? "").Replace("\"", "'");
        var cleanColumnMainFolder = columnMainFolder.Replace("\"", "'");
        var cleanColumnSubFolder = columnSubFolder.Replace("\"", "'");
        var cleanColumnFullPath = columnFullFolderPath.Replace("\"", "'");
        
        lines.Add("    {");
        lines.Add("      \"name\": \"" + cleanColumnName + "\",");
        lines.Add("      \"table\": \"" + table.Name + "\",");
        lines.Add("      \"description\": \"" + cleanColumnDesc + "\",");
        lines.Add("      \"dataType\": \"" + column.DataType + "\",");
        lines.Add("      \"folder\": \"" + cleanColumnMainFolder + "\",");
        lines.Add("      \"subfolder\": \"" + cleanColumnSubFolder + "\",");
        lines.Add("      \"fullFolderPath\": \"" + cleanColumnFullPath + "\",");
        lines.Add("      \"isHidden\": " + column.IsHidden.ToString().ToLower() + ",");
        lines.Add("      \"formatString\": \"" + (column.FormatString ?? "").Replace("\"", "'") + "\"");
        lines.Add("    }");
        columnCount++;
    }
}
lines.Add("  ]");

lines.Add("}");
File.WriteAllLines(outputPath, lines);
Console.WriteLine("Exported " + Model.AllMeasures.Count() + " measures and " + calculatedColumnsCount + " calculated columns to: " + outputPath);
Console.WriteLine("Added folder structure information:");
Console.WriteLine("- folder: Main folder name");
Console.WriteLine("- subfolder: First subfolder name");
Console.WriteLine("- fullFolderPath: Complete folder path");
Console.WriteLine("- Calculated columns include: name, table, expression, dataType, folders, and formatting info");