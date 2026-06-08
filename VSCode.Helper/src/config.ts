import * as vscode from 'vscode';

export interface ExtensionConfig {
	timestampFormat: string;
	autoInsert: boolean;
	gitAutoDetect: boolean;
}

export function getConfiguration(): ExtensionConfig {
	const config = vscode.workspace.getConfiguration('vscode-helper');

	return {
		timestampFormat: config.get('timestampFormat', '[YYYY-MM-DD HH:MM:SS]'),
		autoInsert: config.get('autoInsert', true),
		gitAutoDetect: config.get('gitAutoDetect', true)
	};
}

export function onConfigurationChange(callback: (config: ExtensionConfig) => void): vscode.Disposable {
	return vscode.workspace.onDidChangeConfiguration((event) => {
		if (event.affectsConfiguration('vscode-helper')) {
			callback(getConfiguration());
		}
	});
}
