Console Dynamic Data Sync
A lightweight, configuration-driven console application for synchronizing data from API or SQL sources to a target HTTP endpoint, with support for batching, paging, and dynamic payload mapping — all without DTOs or hard-coded models.
✨ Key Features
🔁 Supports API and SQL as data sources
📦 Sends data as paged batches (configurable page size)
🧩 Dynamic field mapping using JSON configuration
🧱 No DTOs, no reflection, no hard-coded payloads
⚙️ Fully data-driven via JSON profiles
🧼 Clean architecture with clear separation of concerns
🚀 .NET 6+ / .NET 8 compatible
🏗 Project Architecture
Copy code

ConsoleDataSync
│
├── Program.cs                → Clean entry point
│
├── Extensions
│   └── DependencyExtensions.cs
│
├── Configuration
│   ├── Profiles.cs           → Strongly-typed config models
│   └── ProfileLoader.cs      → JSON profile loader
│
├── Services
│   ├── IDataSourceReader.cs
│   ├── ApiDataSourceReader.cs
│   ├── SqlDataSourceReader.cs
│   ├── PayloadTransformer.cs
│   ├── HttpDataSender.cs
│   └── AppRunner.cs          → Main execution flow
🔄 How It Works (Execution Flow)
User provides the path to a JSON profile
The profile defines:
Source type (API or SQL)
Field mappings
Destination endpoint
Payload template
Page size
Data is read from the source
Each record is transformed using the payload template
Records are collected into batches
Batches are sent page-by-page to the target endpoint
▶️ Running the Application


dotnet run
When prompted:

JSON profile path:
Provide the full path to your JSON configuration file.
📄 JSON Configuration Examples
1️⃣ API Source Profile

Json
{
  "Source": {
    "Type": "Api",
    "Endpoint": "https://api.example.com/products",
    "RootArray": "items",
    "PageSize": 100,
    "Fields": {
      "Id": "id",
      "Name": "name",
      "Price": "price"
    }
  },
  "Destination": {
    "Endpoint": "https://target.api/products",
    "PageSize": 50,
    "PayloadTemplate": "{ \"id\": \"{Id}\", \"name\": \"{Name}\", \"price\": {Price} }"
  }
}
2️⃣ SQL Source Profile

Json
{
  "Source": {
    "Type": "Sql",
    "ConnectionString": "Data Source=products.db",
    "Table": "Products",
    "Fields": {
      "Id": "Id",
      "Name": "Name",
      "Price": "Price"
    }
  },
  "Destination": {
    "Endpoint": "https://target.api/products",
    "PageSize": 100,
    "PayloadTemplate": "{ \"id\": \"{Id}\", \"name\": \"{Name}\", \"price\": {Price} }"
  }
}
🧩 Payload Template Explained
The PayloadTemplate is a JSON string with placeholders:

Json
"{ \"id\": \"{Id}\", \"name\": \"{Name}\", \"price\": {Price} }"
{Id}, {Name}, {Price} are replaced dynamically
Output is parsed and sent as a real JSON object
No serialization duplication or escaped payloads
📦 Batch & Paging Behavior
Records are not sent one-by-one
All records are collected, then sent in pages
Page size is controlled by:

Json
"Destination": {
  "PageSize": 50
}
Example output sent to the API:
Copy code
Json
[
  { "id": "1", "name": "Product A", "price": 9.99 },
  { "id": "2", "name": "Product B", "price": 19.99 }
]
⚠️ Important Notes
This project performs full sync (no state / no deduplication)
Each run sends all available data
Ideal for:
Initial data loads
Scheduled full exports
One-way data push jobs
🛠 Requirements
.NET 6 or newer
API endpoint that accepts JSON arrays
Valid JSON profile file
🧪 Extensibility Ideas
This architecture allows easy extension:
Retry per page
Parallel batch sending with throttling
Optional state tracking
Validation of JSON templates
Support for additional data sources
📄 License
MIT (or your preferred license)
👤 Author
Designed as a clean, configuration-first data sync engine
Focused on clarity, flexibility, and operational safety
