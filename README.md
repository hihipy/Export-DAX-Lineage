# Export-DAX-Lineage

A Tabular Editor macro that exports all DAX measures from Power BI models to JSON with complete dependency mapping, complexity analysis, and folder organization tracking. Enables impact analysis, documentation automation, and DAX architecture understanding through clean, structured output.

## Features

- **Complete DAX Inventory**: Exports all measures with expressions, descriptions, and metadata
- **Folder Structure Mapping**: Captures complete folder hierarchy and organization
- **Dependency Mapping**: Shows which measures reference other measures for impact analysis
- **Complexity Analysis**: Automatically categorizes measures from Simple to Very Complex
- **Clean JSON Output**: Properly formatted, parseable JSON without escaping issues
- **Universal Compatibility**: Works with any Power BI model in Tabular Editor
- **Professional Documentation**: Ready-to-use output for technical documentation

## Requirements

- **Tabular Editor** (any version with C# scripting support)
- **Power BI Model** (.pbix, .pbit, or Analysis Services connection)

## Installation

1. **Download the script**: Save the code as `Export-DAX-Lineage.csx` in your preferred location
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
  "exportDate": "2025-08-11 09:57:13",
  "totalMeasures": 32,
  "measures": [
    {
      "name": "Total_Revenue",
      "table": "Sales",
      "expression": "SUM(Sales[Amount])",
      "description": "Total sales revenue",
      "folder": "00 Base Calculations",
      "subfolder": "Revenue Metrics",
      "fullFolderPath": "00 Base Calculations\\Revenue Metrics",
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
      "folder": "01 KPIs",
      "subfolder": "",
      "fullFolderPath": "01 KPIs",
      "complexity": "Medium",
      "directDependencies": 2,
      "isHidden": false,
      "referencedMeasures": ["Total_Revenue", "Previous_Revenue"]
    }
  ]
}
```

## New Folder Structure Features

### Folder Organization Tracking

The macro now captures complete folder hierarchy information:

- **`folder`**: Main/top-level folder name (e.g., "00 Base Calculations")
- **`subfolder`**: First subfolder level (e.g., "Revenue Metrics")
- **`fullFolderPath`**: Complete folder path with backslash separators

### Folder-Based Analysis

- **Organizational Insights**: Understand how measures are grouped and categorized
- **Architecture Mapping**: Visualize folder-based measure organization
- **Impact by Category**: Analyze dependencies within and across folder structures
- **Documentation by Function**: Generate reports organized by business function

### Example Folder Structures

```
00 Base Calculations
├── Revenue Metrics
├── Cost Calculations  
└── Time Intelligence

01 KPIs
├── Financial KPIs
└── Operational KPIs

02 RDD PI
├── Primary Calculations
└── Alternative Methods

03 RDD Department
└── Aggregation Methods

04 RDD Building
└── Space Analytics
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
- **Cross-Folder Dependencies**: Track measure relationships across organizational boundaries

### Metadata Capture

- **Expressions**: Clean, readable DAX code
- **Descriptions**: Measure documentation
- **Table Context**: Source table for each measure
- **Folder Organization**: Complete hierarchy and categorization
- **Hidden Status**: Visibility settings
- **Export Timestamp**: When analysis was performed

## Use Cases

- **Impact Analysis**: "What happens if I modify this measure?"
- **Documentation**: Generate technical documentation automatically organized by folder
- **Code Reviews**: Identify overly complex measures needing refactoring
- **Architecture Planning**: Understand measure relationships and folder organization
- **Knowledge Transfer**: Onboard new team members with visual DAX maps
- **Quality Assurance**: Find measures with excessive complexity
- **Organizational Analysis**: Review folder structure and measure categorization
- **Cross-Team Collaboration**: Understand which teams own which measure categories

## Integration Examples

### Python Analysis

```python
import json
with open('DAX_Measures_Clean.json', 'r') as f:
    dax_data = json.load(f)
    
# Find most complex measures
complex_measures = [m for m in dax_data['measures'] 
                   if m['complexity'] == 'Very Complex']

# Analyze by folder
from collections import Counter
folder_distribution = Counter([m['folder'] for m in dax_data['measures']])
print("Measures per folder:", folder_distribution)

# Find cross-folder dependencies
cross_folder_deps = []
for measure in dax_data['measures']:
    if measure['referencedMeasures']:
        # Check if dependencies are in different folders
        measure_folder = measure['folder']
        for ref_measure_name in measure['referencedMeasures']:
            ref_measure = next((m for m in dax_data['measures'] 
                              if m['name'] == ref_measure_name), None)
            if ref_measure and ref_measure['folder'] != measure_folder:
                cross_folder_deps.append({
                    'measure': measure['name'],
                    'folder': measure_folder,
                    'references': ref_measure['name'],
                    'ref_folder': ref_measure['folder']
                })
```

### Power BI Import

```powerquery
// Import back into Power BI for visualization
let
    Source = Json.Document(File.Contents("DAX_Measures_Clean.json")),
    measures = Source[measures],
    #"Converted to Table" = Table.FromList(measures, Splitter.SplitByNothing()),
    #"Expanded Column1" = Table.ExpandRecordColumn(#"Converted to Table", "Column1", 
        {"name", "folder", "subfolder", "complexity", "directDependencies", "referencedMeasures"})
in
    #"Expanded Column1"
```

### Folder Analysis Dashboard

Create visualizations showing:

- Measure count by folder
- Complexity distribution across folders
- Cross-folder dependency networks
- Folder-based impact analysis

## Technical Details

- **Language**: C# script for Tabular Editor
- **Dependencies**: None beyond standard Tabular Editor libraries
- **Output**: UTF-8 encoded JSON file
- **Performance**: Processes hundreds of measures in seconds
- **Memory**: Minimal memory footprint
- **Compatibility**: Works with all Power BI model versions
- **Folder Support**: Handles nested folder structures of any depth

## Troubleshooting

### Common Issues

- **CS1512 Error**: Ensure you're using the simplified version without helper functions
- **File Path Issues**: Use absolute paths and ensure directory exists
- **Permission Errors**: Verify write access to output directory
- **Empty Output**: Check that your model contains DAX measures
- **Missing Folder Info**: Ensure measures are properly organized in folders within Power BI

### File Path Examples

```csharp
// Windows
var outputPath = @"C:\Users\YourName\Documents\DAX_Export.json";

// Network drive  
var outputPath = @"\\server\share\DAX_Export.json";

// Relative path
var outputPath = @".\DAX_Export.json";
```

### Folder Structure Notes

- Empty folder fields indicate measures at root level or no folder organization
- Backslash separators used in `fullFolderPath` for Windows compatibility
- Subfolder captures only first level - deeper nesting shown in `fullFolderPath`
- Special characters in folder names are properly escaped in JSON output

## License

Export-DAX-Lineage © 2025 is licensed under [Creative Commons Attribution-NonCommercial-NoDerivatives 4.0 International](https://creativecommons.org/licenses/by-nc-nd/4.0/)
