# Export-DAX-Lineage

A Tabular Editor macro that exports all DAX measures from Power BI models to JSON with complete dependency mapping and complexity analysis. Enables impact analysis, documentation automation, and DAX architecture understanding through clean, structured output.

## Features

- **Complete DAX Inventory**: Exports all measures with expressions, descriptions, and metadata
- **Dependency Mapping**: Shows which measures reference other measures for impact analysis
- **Complexity Analysis**: Automatically categorizes measures from Simple to Very Complex
- **Clean JSON Output**: Properly formatted, parseable JSON without escaping issues
- **Universal Compatibility**: Works with any Power BI model in Tabular Editor
- **Professional Documentation**: Ready-to-use output for technical documentation

## Requirements

- **Tabular Editor** (any version with C# scripting support)
- **Power BI Model** (.pbix, .pbit, or Analysis Services connection)

## Installation

1. **Download the script**:
   Save the code as `Export-DAX-Lineage.csx` in your preferred location

2. **Option 1 - Load as Script**:
   - Open Tabular Editor with your Power BI model
   - Go to **Advanced Scripting** tab
   - Click **File** → **Open Script**
   - Select `Export-DAX-Lineage.csx`
   - Click **Run**

3. **Option 2 - Create Custom Action** (Recommended):
   - In Tabular Editor: **File** → **Preferences** → **Custom Actions**
   - Click **Add** and paste the code
   - Name: "Export DAX Lineage to JSON"
   - Now available via right-click context menu

## Usage

### Quick Start

1. **Open your Power BI model** in Tabular Editor
2. **Update the output path** in the script (line 2) to your desired location
3. **Run the macro** using either method above
4. **Find your JSON file** at the specified output path

### Customization

```csharp
// Change output location (line 2)
var outputPath = @"C:\Your\Preferred\Path\DAX_Measures_Clean.json";

// Modify complexity thresholds (lines 22-25)
if (refCount > 0) complexity = "Medium";        // 1+ dependencies = Medium
if (refCount > 2) complexity = "Complex";       // 3+ dependencies = Complex  
if (refCount > 5) complexity = "Very Complex";  // 6+ dependencies = Very Complex
```

## Output Format

The macro generates a JSON file with the following structure:

```json
{
  "exportDate": "2025-08-07 12:03:27",
  "totalMeasures": 29,
  "measures": [
    {
      "name": "Total_Revenue",
      "table": "Sales",
      "expression": "SUM(Sales[Amount])",
      "description": "Total sales revenue",
      "complexity": "Simple",
      "directDependencies": 0,
      "isHidden": false,
      "referencedMeasures": []
    },
    {
      "name": "Revenue_Growth",
      "table": "Sales", 
      "expression": "DIVIDE([Total_Revenue] - [Previous_Revenue], [Previous_Revenue])",
      "description": "YoY revenue growth percentage",
      "complexity": "Medium",
      "directDependencies": 2,
      "isHidden": false,
      "referencedMeasures": ["Total_Revenue", "Previous_Revenue"]
    }
  ]
}
```

## Analysis Features

### Measure Classification

- **Simple**: No measure dependencies (foundation measures)
- **Medium**: 1-2 measure dependencies  
- **Complex**: 3-5 measure dependencies
- **Very Complex**: 6+ measure dependencies

### Dependency Analysis

- **Foundation Measures**: Building blocks with no dependencies
- **Derived Measures**: Reference other measures in calculations
- **Impact Mapping**: See what breaks if you change a measure
- **Hierarchy Understanding**: Visualize your DAX architecture

### Metadata Capture

- **Expressions**: Clean, readable DAX code
- **Descriptions**: Measure documentation
- **Table Context**: Source table for each measure
- **Hidden Status**: Visibility settings
- **Export Timestamp**: When analysis was performed

## Use Cases

- **Impact Analysis**: "What happens if I modify this measure?"
- **Documentation**: Generate technical documentation automatically
- **Code Reviews**: Identify overly complex measures needing refactoring
- **Architecture Planning**: Understand measure relationships and dependencies
- **Knowledge Transfer**: Onboard new team members with visual DAX maps
- **Quality Assurance**: Find measures with excessive complexity

## Integration Examples

### Python Analysis
```python
import json
with open('DAX_Measures_Clean.json', 'r') as f:
    dax_data = json.load(f)
    
# Find most complex measures
complex_measures = [m for m in dax_data['measures'] 
                   if m['complexity'] == 'Very Complex']
```

### Power BI Import
```powerquery
// Import back into Power BI for visualization
let
    Source = Json.Document(File.Contents("DAX_Measures_Clean.json")),
    measures = Source[measures],
    #"Converted to Table" = Table.FromList(measures, Splitter.SplitByNothing()),
    #"Expanded Column1" = Table.ExpandRecordColumn(#"Converted to Table", "Column1", 
        {"name", "complexity", "directDependencies", "referencedMeasures"})
in
    #"Expanded Column1"
```

## Technical Details

- **Language**: C# script for Tabular Editor
- **Dependencies**: None beyond standard Tabular Editor libraries
- **Output**: UTF-8 encoded JSON file
- **Performance**: Processes hundreds of measures in seconds
- **Memory**: Minimal memory footprint
- **Compatibility**: Works with all Power BI model versions

## Troubleshooting

### Common Issues

- **CS1512 Error**: Ensure you're using the simplified version without helper functions
- **File Path Issues**: Use absolute paths and ensure directory exists
- **Permission Errors**: Verify write access to output directory
- **Empty Output**: Check that your model contains DAX measures

### File Path Examples
```csharp
// Windows
var outputPath = @"C:\Users\YourName\Documents\DAX_Export.json";

// Network drive  
var outputPath = @"\\server\share\DAX_Export.json";

// Relative path
var outputPath = @".\DAX_Export.json";
```

## License

Export-DAX-Lineage © 2025 is licensed under [Creative Commons Attribution-NonCommercial-NoDerivatives 4.0 International](https://creativecommons.org/licenses/by-nc-nd/4.0/)
