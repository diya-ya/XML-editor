# XML Editor

A modern XML editor built with React frontend and C# backend, featuring syntax highlighting, validation, formatting, and file operations.

## Features

- **Interactive Tree View**: Visual XML structure editor with expandable/collapsible nodes
- **Split-Pane Layout**: Side-by-side tree view and code editor with resizable panels
- **Real-time Editing**: Edit XML elements directly in the tree view with instant code sync
- **Context Menus**: Right-click operations for adding, editing, and deleting XML nodes
- **Modern UI**: Clean, dark-themed interface with Monaco Editor
- **XML Validation**: Real-time XML validation with error highlighting
- **XML Formatting**: Automatic XML formatting and indentation
- **File Operations**: Open, save, and manage XML files
- **Syntax Highlighting**: Full XML syntax highlighting
- **Server Integration**: Save and load files from the backend server
- **Multiple View Modes**: Switch between split view, tree-only, or code-only modes
- **Responsive Design**: Works on desktop and mobile devices

## Technology Stack

### Frontend
- React 18
- Monaco Editor (VS Code editor)
- React Split Pane (resizable panels)
- Lucide React (icons)
- Axios (HTTP client)

### Backend
- .NET 8
- ASP.NET Core Web API
- System.Xml for XML processing

## Getting Started

### Prerequisites
- Node.js (v16 or higher)
- .NET 8 SDK
- npm or yarn

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd xml-editor
   ```

2. **Install frontend dependencies**
   ```bash
   npm run install-frontend
   ```

3. **Build and run the backend**
   ```bash
   cd backend
   dotnet restore
   dotnet run
   ```

4. **Start the frontend**
   ```bash
   npm start
   ```

5. **Open your browser**
   Navigate to `http://localhost:3000`

### Development Mode

To run both frontend and backend simultaneously:

```bash
npm run dev
```

This will start:
- Frontend on `http://localhost:3000`
- Backend on `http://localhost:5000`

## API Endpoints

### XML Validation
- **POST** `/api/xml/validate`
- **Body**: `{ "xmlContent": "your xml content" }`
- **Response**: `{ "isValid": boolean, "message": "string" }`

### XML Formatting
- **POST** `/api/xml/format`
- **Body**: `{ "xmlContent": "your xml content" }`
- **Response**: `{ "success": boolean, "formattedXml": "string", "message": "string" }`

### XML Parsing
- **POST** `/api/xml/parse`
- **Body**: `{ "xmlContent": "your xml content" }`
- **Response**: `{ "success": boolean, "treeStructure": object, "message": "string" }`

### Save XML
- **POST** `/api/xml/save`
- **Body**: `{ "xmlContent": "your xml content", "fileName": "filename.xml" }`
- **Response**: `{ "success": boolean, "message": "string" }`

### List Files
- **GET** `/api/xml/files`
- **Response**: `{ "files": ["file1.xml", "file2.xml"] }`

### Load File
- **GET** `/api/xml/load/{fileName}`
- **Response**: `{ "success": boolean, "xmlContent": "string", "fileName": "string" }`

## Usage

### Interactive Tree Editing
1. **Tree View**: Use the left panel to navigate and edit XML structure visually
2. **Expand/Collapse**: Click the arrow icons to expand or collapse XML nodes
3. **Edit Elements**: Click on element names or text content to edit inline
4. **Context Menu**: Right-click on any node to access editing options
5. **Add Elements**: Use the context menu or toolbar to add new XML elements
6. **Delete Elements**: Right-click and select "Delete" to remove elements

### Code Editing
1. **Monaco Editor**: Use the right panel for traditional code editing
2. **Syntax Highlighting**: Full XML syntax highlighting and IntelliSense
3. **Auto-formatting**: Automatic indentation and formatting as you type

### View Modes
1. **Split View**: See both tree and code editors side by side (default)
2. **Tree Only**: Focus on the interactive tree view
3. **Code Only**: Traditional code editor view

### File Operations
1. **Create New File**: Click the "New" button to start with a blank XML document
2. **Open File**: Click "Open" to load an XML file from your local system
3. **Validate**: XML is automatically validated as you type
4. **Format**: Click "Format" to automatically format and indent your XML
5. **Save**: Use "Save" to download the file locally or "Save to Server" to store on the backend

## File Structure

```
xml-editor/
├── frontend/
│   ├── public/
│   │   └── index.html
│   ├── src/
│   │   ├── App.js
│   │   ├── App.css
│   │   ├── index.js
│   │   └── index.css
│   └── package.json
├── backend/
│   ├── Program.cs
│   ├── XMLEditor.csproj
│   ├── appsettings.json
│   └── appsettings.Development.json
├── package.json
└── README.md
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License.
