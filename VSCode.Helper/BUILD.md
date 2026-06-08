# VSCode Helper - Build Instructions

## Project Structure

```
VSCode.Helper/
├── src/
│   ├── extension.ts       # Main extension code
│   └── config.ts          # Configuration handling
├── package.json           # NPM project configuration
├── tsconfig.json          # TypeScript configuration
├── .eslintrc.json         # ESLint configuration
├── .vscodeignore          # Files to exclude from VSIX
├── .gitignore             # Git ignore rules
├── README.md              # User documentation
└── CHANGELOG.md           # Version history
```

## Setup & Build

### Prerequisites

- Node.js 16+ 
- npm

### Installation

1. Navigate to the VSCode.Helper folder:
   ```bash
   cd e:\CS\26\GH.Meshok.Sln\VSCode.Helper
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

### Build

- **Development build with watch mode:**
  ```bash
  npm run esbuild-watch
  ```

- **Production build:**
  ```bash
  npm run esbuild-base -- --minify
  ```

### Debug

1. Open VSCode
2. Press `F5` to start debugging extension
3. A new VSCode window will open with the extension loaded

### Package

To create a .vsix file for distribution:
```bash
npx vsce package
```

## Features

- **Create Comment**: Press `Ctrl+Shift+;` to insert timestamped comment
- **Git Integration**: Parse Git changes and create detailed comments
- **Auto-paste**: Automatically suggest comments when Git panel is active
- **Timestamp Format**: Configurable timestamp format (default: `[YYYY-MM-DD HH:MM:SS]`)

## Commands

From the Command Palette (`Ctrl+Shift+P`):

- **VSCode Helper: Create Comment with Timestamp**
- **VSCode Helper: Create Comment from Git Changes**

## Configuration

Edit VSCode settings.json:
```json
{
  "vscode-helper.timestampFormat": "[YYYY-MM-DD HH:MM:SS]",
  "vscode-helper.autoInsert": true,
  "vscode-helper.gitAutoDetect": true
}
```

## Notes

This is a port of the Visual Studio extension "VS.Helper" to VSCode, rewritten in TypeScript using the VSCode Extension API.
