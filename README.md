# Export-DAX-Lineage

A Tabular Editor macro that exports all DAX measures and calculated columns from Power BI models to JSON with complete dependency mapping, complexity analysis, and folder organization tracking. Enables impact analysis, documentation automation, and DAX architecture understanding through clean, structured output.

## Features

- **Complete DAX Inventory**: Exports all measures and calculated columns with expressions, descriptions, and metadata
- **Folder Structure Mapping**: Captures complete folder hierarchy and organization for both measures and columns
- **Dependency Mapping**: Shows which measures reference other measures for impact analysis
- **Complexity Analysis**: Automatically categorizes measures from Simple to Very Complex
- **Clean JSON Output**: Properly formatted, parseable JSON without escaping issues
- **Universal Compatibility**: Cross-platform file paths work on Windows, Mac, and Linux
- **Professional Documentation**: Ready-to-use output for technical documentation

## Requirements

- **Tabular Editor** (any version with C# scripting support)
- **Power BI Model** (.pbix, .pbit, or Analysis Services connection)

## Installation

1. **Download the script**: Save the code as `Export-DAX-Lineage.csx` in your preferred location

2. Option 1 - Load as Script:

   - Open Tabular Editor with your Power BI model
   - Go to **Advanced Scripting** tab
   - Click **File** → **Open Script**
   - Select `Export-DAX-Lineage.csx`
   - Click **Run**

3. Option 2 - Create Custom Action (Recommended):

   - In Tabular Editor: **File** → **Preferences** → **Custom Actions**
   - Click **Add** and paste the code
   - Name: "Export DAX Lineage to JSON"
   - Now available via right-click context menu

## Usage

### Quick Start

1. **Open your Power BI model** in Tabular Editor
2. **Run the macro** using either method above (no path configuration needed)
3. **Find your JSON file** in your Downloads folder as `DAX_Measures_Clean.json`

### Customization

```csharp
// Output automatically goes to Downloads folder - cross-platform compatible
var outputPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "Downloads", "DAX_Measures_Clean.json");

// Modify complexity thresholds (for measures)
if (refCount > 0) complexity = "Medium";        // 1+ dependencies = Medium
if (refCount > 2) complexity = "Complex";       // 3+ dependencies = Complex  
if (refCount > 5) complexity = "Very Complex";  // 6+ dependencies = Very Complex
```

## Output Format

The macro generates a JSON file with the following structure:

```json
{
  "exportDate": "2025-08-19 11:08:30",
  "totalMeasures": 56,
  "totalCalculatedColumns": 13,
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
  ],
  "calculatedColumns": [
    {
      "name": "YoY_Growth_Rate",
      "table": "Sales",
      "description": "Year-over-year growth calculation",
      "dataType": "Double",
      "folder": "Growth Metrics",
      "subfolder": "",
      "fullFolderPath": "Growth Metrics",
      "isHidden": false,
      "formatString": "0.00%"
    },
    {
      "name": "Month",
      "table": "DateTable",
      "description": "",
      "dataType": "String",
      "folder": "",
      "subfolder": "",
      "fullFolderPath": "",
      "isHidden": true,
      "formatString": ""
    }
  ]
}
```

## New Features in Latest Version

### Calculated Columns Support

The macro now exports calculated columns alongside measures:

- **Data Types**: Captures column data types (String, Double, Int64, DateTime, etc.)
- **Format Strings**: Documents number and date formatting
- **Folder Organization**: Same folder structure tracking as measures
- **Visibility Settings**: Hidden column identification
- **Complete Inventory**: Both custom and auto-generated columns

### Cross-Platform Compatibility

- **Universal File Paths**: Automatically saves to Downloads folder on any OS
- **Windows**: `C:\Users\[username]\Downloads\DAX_Measures_Clean.json`
- **Mac**: `/Users/[username]/Downloads/DAX_Measures_Clean.json`
- **Linux**: `/home/[username]/Downloads/DAX_Measures_Clean.json`

## Folder Structure Features

### Folder Organization Tracking

The macro captures complete folder hierarchy information for both measures and calculated columns:

- **`folder`**: Main/top-level folder name (e.g., "00 Base Calculations")
- **`subfolder`**: First subfolder level (e.g., "Revenue Metrics")
- **`fullFolderPath`**: Complete folder path with backslash separators

### Folder-Based Analysis

- **Organizational Insights**: Understand how DAX objects are grouped and categorized
- **Architecture Mapping**: Visualize folder-based organization
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

05 Advanced Analytics
├── Growth Metrics
└── Efficiency Scores
```

## Analysis Features

### Measure Classification

- **Simple**: No measure dependencies (foundation measures)
- **Medium**: 1-2 measure dependencies
- **Complex**: 3-5 measure dependencies
- **Very Complex**: 6+ measure dependencies

### Calculated Column Analysis

- **Data Type Distribution**: Understanding column types across tables
- **Format Consistency**: Identifying formatting patterns
- **Folder Organization**: How columns are categorized
- **Hidden vs Visible**: Column visibility analysis

### Dependency Analysis

- **Foundation Measures**: Building blocks with no dependencies
- **Derived Measures**: Reference other measures in calculations
- **Impact Mapping**: See what breaks if you change a measure
- **Hierarchy Understanding**: Visualize your DAX architecture
- **Cross-Folder Dependencies**: Track measure relationships across organizational boundaries

### Metadata Capture

- **Expressions**: Clean, readable DAX code (measures only)
- **Descriptions**: Object documentation
- **Table Context**: Source table for each object
- **Folder Organization**: Complete hierarchy and categorization
- **Hidden Status**: Visibility settings
- **Data Types**: Column type information
- **Format Strings**: Number and date formatting
- **Export Timestamp**: When analysis was performed

## Use Cases

- **Complete Model Documentation**: Generate technical documentation for all DAX objects
- **Impact Analysis**: "What happens if I modify this measure?"
- **Data Type Auditing**: Review calculated column types and formatting
- **Architecture Planning**: Understand DAX object relationships and folder organization
- **Knowledge Transfer**: Onboard new team members with complete DAX inventory
- **Quality Assurance**: Find measures with excessive complexity
- **Organizational Analysis**: Review folder structure and object categorization
- **Cross-Team Collaboration**: Understand which teams own which DAX objects
- **Model Optimization**: Identify redundant or inefficient calculated columns

## Integration Examples

### Python Analysis

```python
import json
with open('DAX_Measures_Clean.json', 'r') as f:
    dax_data = json.load(f)
    
# Analyze measures
complex_measures = [m for m in dax_data['measures'] 
                   if m['complexity'] == 'Very Complex']

# Analyze calculated columns
calc_columns = dax_data['calculatedColumns']
data_type_distribution = {}
for col in calc_columns:
    dtype = col['dataType']
    data_type_distribution[dtype] = data_type_distribution.get(dtype, 0) + 1

# Find custom vs auto-generated columns
custom_columns = [col for col in calc_columns 
                 if not col['table'].startswith('DateTableTemplate') 
                 and not col['table'].startswith('LocalDateTable')]

# Analyze by folder
from collections import Counter
measure_folders = Counter([m['folder'] for m in dax_data['measures']])
column_folders = Counter([col['folder'] for col in calc_columns if col['folder']])

print(f"Total measures: {dax_data['totalMeasures']}")
print(f"Total calculated columns: {dax_data['totalCalculatedColumns']}")
print(f"Custom calculated columns: {len(custom_columns)}")
print("Data type distribution:", data_type_distribution)
```

### Power BI Import

```powerquery
// Import measures
let
    Source = Json.Document(File.Contents("DAX_Measures_Clean.json")),
    measures = Source[measures],
    MeasuresTable = Table.FromList(measures, Splitter.SplitByNothing()),
    ExpandedMeasures = Table.ExpandRecordColumn(MeasuresTable, "Column1", 
        {"name", "table", "folder", "complexity", "directDependencies"})
in
    ExpandedMeasures

// Import calculated columns
let
    Source = Json.Document(File.Contents("DAX_Measures_Clean.json")),
    columns = Source[calculatedColumns],
    ColumnsTable = Table.FromList(columns, Splitter.SplitByNothing()),
    ExpandedColumns = Table.ExpandRecordColumn(ColumnsTable, "Column1", 
        {"name", "table", "dataType", "folder", "isHidden", "formatString"})
in
    ExpandedColumns
```

### Complete Model Analysis Dashboard

Create visualizations showing:

- **Object Distribution**: Measures vs calculated columns by folder
- **Complexity Analysis**: Measure complexity across folders
- **Data Type Analysis**: Column type distribution
- **Cross-Folder Dependencies**: Dependency networks
- **Hidden Object Analysis**: Visibility patterns
- **Format String Consistency**: Formatting standards compliance

## Technical Details

- **Language**: C# script for Tabular Editor
- **Dependencies**: None beyond standard Tabular Editor libraries
- **Output**: UTF-8 encoded JSON file
- **Performance**: Processes hundreds of DAX objects in seconds
- **Memory**: Minimal memory footprint
- **Compatibility**: Works with all Power BI model versions
- **Cross-Platform**: Universal file paths work on Windows, Mac, and Linux
- **Folder Support**: Handles nested folder structures of any depth

## Troubleshooting

### Common Issues

- **CS1061 Error**: Fixed in latest version - no longer attempts to access unsupported Expression property for calculated columns
- **File Path Issues**: Resolved - now uses cross-platform compatible paths
- **Permission Errors**: Verify write access to Downloads folder
- **Empty Output**: Check that your model contains DAX measures or calculated columns
- **Missing Folder Info**: Ensure objects are properly organized in folders within Power BI

### What's Exported

- **Measures**: All measures with DAX expressions and dependency analysis
- **Calculated Columns**: Custom and auto-generated columns with metadata
- **Auto-Generated Columns**: Date table columns are included but clearly identifiable
- **Hidden Objects**: Both visible and hidden objects are documented

### File Path Details

The script automatically saves to your Downloads folder regardless of operating system:

```csharp
// Cross-platform compatible path
var outputPath = System.IO.Path.Combine(
    System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), 
    "Downloads", 
    "DAX_Measures_Clean.json"
);
```

### Folder Structure Notes

- Empty folder fields indicate objects at root level or no folder organization
- Backslash separators used in `fullFolderPath` for consistency
- Subfolder captures only first level - deeper nesting shown in `fullFolderPath`
- Special characters in folder names are properly escaped in JSON output

## License

Export-DAX-Lineage © 2025 is licensed under [Creative Commons Attribution-NonCommercial-NoDerivatives 4.0 International](https://creativecommons.org/licenses/by-nc-nd/4.0/)
