# VSCode Helper

A helper extension for Visual Studio Code that automatically generates comments with timestamps and Git integration.

## Features

- **Auto Comment Generation**: Generate comments with timestamps (`[YYYY-MM-DD HH:MM:SS]`)
- **Git Changes Integration**: Parse and create comments from Git changes
- **Auto-paste on Git Panel**: Automatically insert comments when Git panel is active
- **Keyboard Shortcut**: `Ctrl+Shift+;` (or `Cmd+Shift+;` on Mac) to create comment

## Installation

1. Open the VSCode.Helper folder
2. Run `npm install` to install dependencies
3. Press `F5` to open the extension in debug mode
4. Or build with `npm run esbuild` to create the production build

## Usage

### Create Simple Comment

Press `Ctrl+Shift+;` to insert a comment with current timestamp:

```
// [2024-06-08 14:30:45]
```

### Create Comment from Git Changes

1. Copy Git changes information (e.g., from terminal)
2. Run command "VSCode Helper: Create Comment from Git Changes"
3. Comment with Git file changes will be inserted:

```
// [2024-06-08 14:30:45]
// Git Changes:
// Modified: file.ts
// Added: new_file.cs
// Deleted: old_file.js
```

### Commands

- `vscode-helper.createComment` - Create comment with timestamp
- `vscode-helper.createCommentFromGit` - Create comment from Git changes

## Configuration

Configure in VSCode settings:

```json
{
  "vscode-helper.timestampFormat": "[YYYY-MM-DD HH:MM:SS]",
  "vscode-helper.autoInsert": true
}
```

## Development

- `npm run esbuild` - Build the extension
- `npm run esbuild-watch` - Watch mode
- `npm run lint` - Run ESLint
- Press `F5` in VSCode to debug

## License

MIT