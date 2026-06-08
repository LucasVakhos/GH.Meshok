# Changelog

All notable changes to this project will be documented in this file.

## [1.0.0] - 2024-06-08

### Added
- Initial release of VSCode Helper
- Auto comment generation with timestamps
- Git changes parsing and integration
- Keyboard shortcut for quick comment insertion
- Auto-paste functionality for Git panel

### Changed
- Ported from Visual Studio extension to VSCode extension
- Rewrote in TypeScript for VSCode API
- Changed from C# EnvDTE to VSCode API

### Features
- Generate comments with `[YYYY-MM-DD HH:MM:SS]` timestamp format
- Parse Git changes and create detailed comments
- Automatic insertion on Git panel activation
- Support for clipboard-based Git changes
- Command palette integration
